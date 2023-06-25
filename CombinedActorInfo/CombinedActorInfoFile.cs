using System.Numerics;
using System.Runtime.InteropServices;
using ToolLib;

namespace CombinedActorInfo
{
    public class CombinedActorInfoFile : FileReader<CombinedActorInfoFile>
    {

        protected override byte[] Magic { get; set; } = "CmbAct"u8.ToArray();

        public List<Actor> Actors = new List<Actor>();

        public List<float> MiscFloat = new List<float>();

        public CombinedActorInfoFile(BinaryReader reader) : base(reader)
        {
            var unknown1 = reader.ReadUInt16();
            var unknown2 = reader.ReadByte(); // Version?

            var entries = reader.ReadByte();
            var unknown3 = reader.ReadUInt16();

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
                reader.ReadUInt32();
                reader.ReadUInt32();
                reader.ReadUInt16();
                reader.ReadUInt16();
            }

            for (int j = 0; j < 6; j++)
            {
                MiscFloat.Add(reader.ReadSingle());
            }

        }

        public static string? BytesToString(byte[] sBytes)
        {
            var handle = GCHandle.Alloc(sBytes, GCHandleType.Pinned);
            try
            {
                return Marshal.PtrToStringAnsi(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }
    }

    public class Actor
    {
        public string Name;
        public ActorMatrix Matrix;
        public ActorMatrix Matrix2;
        public List<float> Data;

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
        private Matrix4x4 Matrix;
        public Quaternion Quaternion;
        public Vector3 Position;
        public Vector3 Rotation;

        public ActorMatrix(Matrix4x4 matrix)
        {
            Matrix = matrix;
            Quaternion = matrix.ToQuaternion();
            Position = matrix.Translation;
            Rotation = Quaternion.ToVector3();
        }
    }
}
