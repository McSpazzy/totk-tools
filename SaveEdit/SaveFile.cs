using System.Text;
using CombinedActorInfo;
using ToolLib;

namespace SaveEdit
{
    public class SaveFile : FileReader<SaveFile, SaveFileOptions>
    {
        protected override byte[] Magic { get; set; } = {4,3,2,1};

        public List<SaveFileEntry> Entries = new();
        private Dictionary<uint, string> _hashList = new();
        
        public SaveFile(BinaryReader reader, SaveFileOptions? options) : base(reader, options)
        {
            if (options?.WordFile != null)
            {
                if (!File.Exists(options.WordFile))
                {
                    Console.WriteLine($"WordFile '{options.WordFile}' does not exist.");
                    return;
                }

                _hashList = File.ReadAllLines(options.WordFile).ToDictionary(MurMurHash3.Hash, l => l);
            }

            var unknown1 = reader.ReadInt32();
            var dataOffset = reader.ReadInt32();
            var unknown2 = reader.ReadBytes(20);

            var currentType = DataType.Bool; // Always First One
            var skip = new SaveFileEntry(ref reader, DataType.Bool, options);
            var currentCount = 0;
            var offsetStart = reader.BaseStream.Position;

            while (reader.BaseStream.Position < dataOffset)
            {
                var entry = new SaveFileEntry(ref reader, currentType, options);
                if (entry.Hash == 0)
                {
                    if (options?.ShowDetails ?? false)
                    {
                        Console.WriteLine($"DataStart: {offsetStart,-6} | DataEnd: {reader.BaseStream.Position,-6} | {currentType, -14} | {currentCount}");
                    }

                    currentType = (DataType) entry.Offset;
                    entry.DataType = currentType;
                    currentCount = 0;
                    offsetStart = reader.BaseStream.Position + 8;
                    continue;
                }

                if (_hashList.ContainsKey(entry.Hash))
                {
                    entry.PropertyName = _hashList[entry.Hash];
                }

                currentCount++;
                Entries.Add(entry);
            }

            if (options?.ShowDetails ?? false) // To handle last one outside of loop
            {
                Console.WriteLine($"DataStart: {offsetStart,-6} | DataEnd: {reader.BaseStream.Position,-6} | {currentType,-14} | {currentCount}");
            }
        }
    }

    public class SaveFileEntry
    {
        public SaveFileEntry(ref BinaryReader reader, DataType type, SaveFileOptions? options)
        {
            Hash = reader.ReadUInt32();
            var valueBytes = reader.ReadBytes(4);
            Offset = BitConverter.ToUInt32(valueBytes);
            DataType = type;

            if (Hash == 0)
            {
                return;
            }

            var currentOffset = reader.BaseStream.Position;

            try
            {
                switch (DataType)
                {
                    case DataType.Bool:
                        Value = BitConverter.ToBoolean(valueBytes);
                        Offset = 0;
                        break;
                    case DataType.BoolArray:
                        var bitLength = reader.ReadInt32At(Offset) / 8;
                        Value = reader.ReadBytes(bitLength == 0 ? 1 : bitLength).ToBinaryString();
                        break;
                    case DataType.Int32:
                        Value = BitConverter.ToInt32(valueBytes);
                        Offset = 0;
                        break;
                    case DataType.Int32Array:
                        Value = reader.ReadInt32(reader.ReadInt32At(Offset));
                        break;
                    case DataType.Float:
                        Value = BitConverter.ToSingle(valueBytes);
                        Offset = 0;
                        break;
                    case DataType.FloatArray:
                        Value = reader.ReadSingle(reader.ReadInt32At(Offset));
                        break;
                    case DataType.Enum:
                        Value = SaveStructs.Enum[Offset];
                        Offset = 0;
                        break;
                    case DataType.EnumArray:
                        Value = reader.ReadUInt32(reader.ReadInt32At(Offset)).Select(e => SaveStructs.Enum[e]);
                        break;
                    case DataType.Vector2Array:
                        Value = reader.ReadVector2(reader.ReadInt32At(Offset));
                        break;
                    case DataType.Vector3:
                        Value = reader.ReadVector3At(Offset);
                        break;
                    case DataType.Vector3Array:
                        Value = reader.ReadVector3(reader.ReadInt32At(Offset));
                        break;
                    case DataType.String32:
                        Value = reader.ReadStringAt(Offset, 32);
                        break;
                    case DataType.String64:
                        Value = reader.ReadStringAt(Offset, 64);
                        break;
                    case DataType.String64Array:
                        Value = reader.ReadString(64, reader.ReadInt32At(Offset));
                        break;
                    case DataType.BinaryArray:
                        reader.BaseStream.Seek(Offset, SeekOrigin.Begin);
                        var dataCount = reader.ReadInt32();

                        var dataArray = new byte[dataCount][];

                        for (var j = 0; j < dataCount; j++)
                        {
                            var data = reader.ReadBytes(reader.ReadInt32());
                            dataArray[j] = data;
                        }

                        Value = dataArray;

                        if (Hash == 2774999734)
                        {
                            if (options?.SerializeAutoBuildData ?? false)
                            {
                                Value = dataArray.Select(a => CombinedActorInfoFile.Open(a));
                            }

                            if (!string.IsNullOrEmpty(options?.ExportAutoBuild))
                            {
                                try
                                {
                                    Directory.CreateDirectory(options.ExportAutoBuild);
                                    for (var i = 0; i < dataArray.Length; i++)
                                    {
                                        File.WriteAllBytes($"{options.ExportAutoBuild}\\AutoBuild-{i}.cai", dataArray[i]);
                                    }
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine("Unable to export CAI files. " + ex.Message);
                                }
                            }
                        }
                        break;
                    case DataType.UInt32:
                        Value = BitConverter.ToUInt32(valueBytes);
                        Offset = 0;
                        break;
                    case DataType.UInt32Array:
                        Value = reader.ReadUInt32(reader.ReadInt32At(Offset));
                        break;
                    case DataType.UInt64:
                        Value = reader.ReadUInt64At(Offset);
                        break;
                    case DataType.UInt64Array:
                        Value = reader.ReadUInt64(reader.ReadInt32At(Offset));
                        break;
                    case DataType.WString16Array:
                        Value = reader.ReadString(32, reader.ReadInt32At(Offset), Encoding.Unicode);
                        break;
                    case DataType.UnknownData:
                        Value = "TODO";
                        break;
                    default:
                        Console.WriteLine($"Missing DataType: {DataType}");
                        return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Hash: {Hash} | Offset: {currentOffset}");
                Console.WriteLine(ex.Message);
            }

            reader.BaseStream.Seek(currentOffset, SeekOrigin.Begin);
        }

        public uint Hash { get; set; } 
        public uint Offset { get; set; }
        public DataType DataType { get; set; }
        public string? PropertyName { get; set; } = null;
        public object? Value { get; set; }
    }


    public enum DataType
    {
        Bool = 0,
        BoolArray = 1,
        Int32 = 2,
        Int32Array = 3,
        Float = 4,
        FloatArray = 5,
        Enum = 6,
        EnumArray = 7,
        Vector2 = 8, // Probably
        Vector2Array = 9,
        Vector3 = 10,
        Vector3Array = 11,
        String16 = 12, // Maybe
        String16Array = 13, // A guess, doesn't exist in GameDataList
        String32 = 14,
        String32Array = 15, // Probably
        String64 = 16,
        String64Array = 17,
        Binary = 18, // Maybe?
        BinaryArray = 19,
        UInt32 = 20,
        UInt32Array = 21,
        UInt64 = 24,
        UInt64Array = 25,
        WString16 = 26, // Probably
        WString16Array = 27,
        UnknownData = 32, // bytes that have some kind of sequence
    }

    public class SaveFileOptions
    {
        /// <summary>
        /// WordFile used to apply property names to the fields. Default available <see href="https://github.com/McSpazzy/totk-gamedata/blob/master/WordList.txt">Here</see>
        /// </summary>
        public string? WordFile { get; set; }

        /// <summary>
        /// Show entry counts in console
        /// </summary>
        public bool ShowDetails { get; set; }

        /// <summary>
        /// Convert the AutoBuild data into readable json in export
        /// </summary>
        public bool SerializeAutoBuildData { get; set; }

        /// <summary>
        /// Write CAI files to specified directory
        /// </summary>
        public string? ExportAutoBuild { get; set; }
    }

}

