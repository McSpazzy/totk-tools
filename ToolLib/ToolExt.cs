using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using SkiaSharp;

namespace ToolLib;

public static class ToolExt
{
    public static string? BytesToString(this byte[] sBytes, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;

        if (sBytes.Length == 0)
        {
            return null;
        }

        var handle = GCHandle.Alloc(sBytes, GCHandleType.Pinned);
        try
        {
            switch (encoding)
            {
                case { } when Equals(encoding, Encoding.ASCII):
                case { } when Equals(encoding, Encoding.UTF8):
                    return Marshal.PtrToStringUTF8(handle.AddrOfPinnedObject());
                case { } when Equals(encoding, Encoding.Unicode):
                    return Marshal.PtrToStringUni(handle.AddrOfPinnedObject());
                default:
                    return Marshal.PtrToStringAuto(handle.AddrOfPinnedObject());
            }
        }
        finally
        {
            handle.Free();
        }
    }

    public static Quaternion ToQuaternion(this Matrix4x4 matrix)
    {
        return Quaternion.CreateFromRotationMatrix(matrix);
    }

    public static Vector3 ToVector3(this Quaternion quaternion)
    {
        float pitch;

        double sinr_cosp = 2 * (quaternion.W * quaternion.X + quaternion.Y * quaternion.Z);
        double cosr_cosp = 1 - 2 * (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y);
        var roll = (float)((float)Math.Atan2(sinr_cosp, cosr_cosp) * 180f / Math.PI);

        double sinp = 2 * (quaternion.W * quaternion.Y - quaternion.Z * quaternion.X);
        if (Math.Abs(sinp) >= 1)
        {
            pitch = (float)Math.CopySign(90f, sinp); // use 90 degrees if out of range
        }
        else
        {
            pitch = (float)((float)Math.Asin(sinp) * 180f / Math.PI);
        }

        double siny_cosp = 2 * (quaternion.W * quaternion.Z + quaternion.X * quaternion.Y);
        double cosy_cosp = 1 - 2 * (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z);
        var yaw = (float)((float)Math.Atan2(siny_cosp, cosy_cosp) * 180f / Math.PI);

        return new Vector3(roll, pitch, yaw);
    }

    public static string ToBinaryString(this byte[] sBytes)
    {
        return string.Join("", sBytes.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));
    }

    public static Vector2 ReadVector2(this BinaryReader reader)
    {
        return new Vector2(reader.ReadSingle(), reader.ReadSingle());
    }

    public static Vector3 ReadVector3(this BinaryReader reader)
    {
        return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }

    public static Vector3 ReadVector3At(this BinaryReader reader, uint offset)
    {
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }

    public static Vector2[] ReadVector2(this BinaryReader reader, int count)
    {
        return Enumerable.Range(0, count).Select(i => reader.ReadVector2()).ToArray();
    }

    public static Vector3[] ReadVector3(this BinaryReader reader, int count)
    {
        return Enumerable.Range(0, count).Select(i => reader.ReadVector3()).ToArray();
    }

    public static float[] ReadSingle(this BinaryReader reader, int count)
    {
        return Enumerable.Range(0, count).Select(i => reader.ReadSingle()).ToArray();
    }

    public static Matrix4x4 ReadMatrix4X4(this BinaryReader reader)
    {
        var r1 = reader.ReadSingle();
        var r2 = reader.ReadSingle();
        var r3 = reader.ReadSingle();
        var t1 = reader.ReadSingle();
        var r4 = reader.ReadSingle();
        var r5 = reader.ReadSingle();
        var r6 = reader.ReadSingle();
        var t2 = reader.ReadSingle();
        var r7 = reader.ReadSingle();
        var r8 = reader.ReadSingle();
        var r9 = reader.ReadSingle();
        var t3 = reader.ReadSingle();
        var a1 = reader.ReadSingle();
        var a2 = reader.ReadSingle();
        var a3 = reader.ReadSingle();
        var a4 = reader.ReadSingle();

        return new Matrix4x4(r1, r2, r3, a4, r4, r5, r6, a3, r7, r8, r9, a2, t1, t2, t3, a1);
    }

    public static string? ReadString(this BinaryReader reader, int length, Encoding? encoding = null)
    {
        return reader.ReadBytes(length).BytesToString(encoding);
    }

    public static string? ReadStringAt(this BinaryReader reader, uint offset, int length, Encoding? encoding = null)
    {
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        return reader.ReadBytes(length).BytesToString(encoding);
    }

    public static string?[] ReadString(this BinaryReader reader, int length, int count, Encoding? encoding = null)
    {
        return Enumerable.Range(0, count).Select(i => reader.ReadString(length, encoding)).ToArray();
    }

    public static int[] ReadInt32(this BinaryReader reader, int count)
    {
        return Enumerable.Range(0, count).Select(i => reader.ReadInt32()).ToArray();
    }

    public static uint[] ReadUInt32(this BinaryReader reader, int count)
    {
        return Enumerable.Range(0, count).Select(i => reader.ReadUInt32()).ToArray();
    }

    public static long[] ReadInt64(this BinaryReader reader, int count)
    {
        return Enumerable.Range(0, count).Select(i => reader.ReadInt64()).ToArray();
    }

    public static ulong[] ReadUInt64(this BinaryReader reader, int count)
    {
        return Enumerable.Range(0, count).Select(i => reader.ReadUInt64()).ToArray();
    }

    public static int ReadInt32At(this BinaryReader reader, uint offset)
    {
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        return reader.ReadInt32();
    }

    public static long ReadInt64At(this BinaryReader reader, uint offset)
    {
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        return reader.ReadInt64();
    }

    public static ulong ReadUInt64At(this BinaryReader reader, uint offset)
    {
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        return reader.ReadUInt64();
    }

    public static uint ReadUInt32At(this BinaryReader reader, uint offset)
    {
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        return reader.ReadUInt32();
    }

    public static byte[] ReadBytesAt(this BinaryReader reader, long offset, int count)
    {
        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
        return reader.ReadBytes(count);
    }

    public static long Goto(this BinaryReader reader, long offset)
    {
        return reader.BaseStream.Seek(offset, SeekOrigin.Begin);
    }

    public static void Save(this SKSurface surface, string filename, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        var memoryStream = new MemoryStream();
        surface.Snapshot().Encode(format, quality).SaveTo(memoryStream);
        File.WriteAllBytes(filename, memoryStream.ToArray());
    }
    
    public static void Save(this SKBitmap bitmap, string filename, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        var memoryStream = new MemoryStream();
        bitmap.Encode(format, quality).SaveTo(memoryStream);
        File.WriteAllBytes(filename, memoryStream.ToArray());
    }
}
