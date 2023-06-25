using System;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using SkiaSharp;
using ToolLib;

namespace CombinedActorInfo
{

    public class CombinedActorInfoFile : FileReader<CombinedActorInfoFile>
    {
        public CombinedActorInfo CombinedActorInfo;

        public static CombinedActorInfo FromFile(string fileName)
        {
            return FromJson(File.ReadAllText(fileName));
        }

        public static CombinedActorInfo FromJson(string json)
        {
            var cai = JsonConvert.DeserializeObject<CombinedActorInfo>(json, new JsonSerializerSettings() { });
            cai.RebuildMatrices();

            return cai;
        }

        protected override byte[] Magic { get; set; } = "CmbAct"u8.ToArray();

        public CombinedActorInfoFile(BinaryReader reader) : base(reader)
        {
            CombinedActorInfo = new CombinedActorInfo(reader);
        }
    }

    public class CombinedActorInfo
    {
        public byte[] UnknownHeaderBytes = Array.Empty<byte>();
        public List<Actor> Actors = new List<Actor>();
        public List<float> MiscFloat = new List<float>();
        public byte[] UnknownData = Array.Empty<byte>();

        public CombinedActorInfo() { }

        public CombinedActorInfo(BinaryReader reader)
        {
            var unknown1 = reader.ReadUInt16();
            var unknown2 = reader.ReadByte(); // Version?

            var entries = reader.ReadByte();
            UnknownHeaderBytes = reader.ReadBytes(2);

            for (var i = 0; i < 21; i++) // There is always 21 entries, even if they are unused
            {
                var matrix = reader.ReadMatrix4X4();
                var matrix2 = reader.ReadMatrix4X4();

                var otherData = new List<float>();

                for (int j = 0; j < 6; j++)
                {
                    otherData.Add(reader.ReadSingle());
                }

                var name = reader.ReadString(128);
                if (Actors.Count < entries)
                {
                    Actors.Add(new Actor(name, matrix, matrix2, otherData));
                }
            }

            for (var i = 0; i < 21; i++) // There is more data here?
            {
                var b = reader.ReadBytes(12);
                if (i < entries)
                {
                    Actors[i].Extra = b;
                }
            }

            for (int j = 0; j < 6; j++)
            {
                MiscFloat.Add(reader.ReadSingle());
            }

            UnknownData = reader.ReadBytes(520);
        }

        public void RebuildMatrices()
        {
            foreach (var actor in Actors)
            {
                actor.Matrix.Rebuild();
                actor.Matrix2.Rebuild();
            }
        }

        public void Save(string fileName)
        {
            var memoryStream = new MemoryStream();
            var writer = new BinaryWriter(memoryStream);

            writer.Write("CmbAct"u8.ToArray());
            writer.Write((short) 0);
            writer.Write((byte) 2); // Version?
            writer.Write((byte) Actors.Count);
            writer.Write(UnknownHeaderBytes);

            foreach (var actor in Actors)
            {
                writer.WriteMatrix4X4(actor.Matrix.Matrix);
                writer.WriteMatrix4X4(actor.Matrix2.Matrix);
                foreach (var f in actor.Data)
                {
                    writer.Write(f);
                }

                var nameBytes = Encoding.ASCII.GetBytes(actor.Name);

                writer.Write(nameBytes);
                writer.Write(Enumerable.Repeat((byte) 0, 128 - actor.Name.Length).ToArray());
            }

            for (var i = 0; i < 21 - Actors.Count; i++)
            {
                writer.WriteMatrix4X4(ActorMatrix.Blank.Matrix);
                writer.WriteMatrix4X4(ActorMatrix.Blank.Matrix);

                foreach (var f in new float[] {0, 0, 1, 0, 1, 0})
                {
                    writer.Write(f);
                }
                writer.Write(Enumerable.Repeat((byte) 0, 128).ToArray());
            }


            foreach (var actor in Actors)
            {
                writer.Write(actor.Extra);
            }

            for (var i = 0; i < 21 - Actors.Count; i++)
            {
                writer.Write(Enumerable.Repeat((byte)0, 12).ToArray());
            }
            
            foreach (var f in MiscFloat)
            {
                writer.Write(f);
            }

            writer.Write(UnknownData);
            writer.Flush();

            File.WriteAllBytes(fileName, memoryStream.ToArray());

        }
    }

    public class Actor
    {
        public string Name;
        public ActorMatrix Matrix;
        public ActorMatrix Matrix2;
        public List<float> Data;

        public byte[] Extra;

        public Actor() { }

        public Actor(string? name, Matrix4x4 matrix, Matrix4x4 matrix2, List<float> other)
        {
            Name = name ?? "Unknown";
            Matrix = new ActorMatrix(matrix);
            Matrix2 = new ActorMatrix(matrix2);
            Data = other;
        }
    }

    public class ActorMatrix
    {
        [JsonIgnore]
        public Matrix4x4 Matrix;
        public Quaternion Quaternion;
        public Vector3 Position;
        public Vector3 Rotation;

        public ActorMatrix() { }

        public ActorMatrix(Matrix4x4 matrix)
        {
            Matrix = matrix;
            Quaternion = matrix.ToQuaternion();
            Position = matrix.Translation;
            Rotation = Quaternion.ToEulerAngles();
        }

        public void Rebuild()
        {
            Quaternion = Rotation.ToQuaternion();
            Matrix = Quaternion.ToMatrix();
            Matrix.Translation = Position;
        }

        public static ActorMatrix Blank
        {
            get
            {
                var matrix = new ActorMatrix {Rotation = new Vector3(), Position = new Vector3()};
                matrix.Rebuild();
                return matrix;
            }
        }
    }
}
