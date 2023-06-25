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

    public static Matrix4x4 ToMatrix(this Quaternion quaternion)
    {
        return Matrix4x4.CreateFromQuaternion(quaternion);
    }

    public static Quaternion ToQuaternion(this Vector3 v)
    {
        v.X /= (float)(180f / Math.PI);
        v.Y /= (float)(180f / Math.PI);
        v.Z /= (float)(180f / Math.PI);
        
        float cy = (float)Math.Cos(v.Z * 0.5);
        float sy = (float)Math.Sin(v.Z * 0.5);
        float cp = (float)Math.Cos(v.Y * 0.5);
        float sp = (float)Math.Sin(v.Y * 0.5);
        float cr = (float)Math.Cos(v.X * 0.5);
        float sr = (float)Math.Sin(v.X * 0.5);

        /*
        return new Quaternion
        {
            W = (cr * cp * cy + sr * sp * sy),
            X = (sr * cp * cy - cr * sp * sy),
            Y = (cr * sp * cy + sr * cp * sy),
            Z = (cr * cp * sy - sr * sp * cy)
        };
        */

        return new Quaternion
        {
            W = ((float)(Math.Cos(v.X * 0.5) * Math.Cos(v.Y * 0.5) * Math.Cos(v.Z * 0.5) + Math.Sin(v.X * 0.5) * Math.Sin(v.Y * 0.5) * Math.Sin(v.Z * 0.5))).FixFloat(),
            X = ((float)(Math.Sin(v.X * 0.5) * Math.Cos(v.Y * 0.5) * Math.Cos(v.Z * 0.5) - Math.Cos(v.X * 0.5) * Math.Sin(v.Y * 0.5) * Math.Sin(v.Z * 0.5))).FixFloat(),
            Y = ((float)(Math.Cos(v.X * 0.5) * Math.Sin(v.Y * 0.5) * Math.Cos(v.Z * 0.5) + Math.Sin(v.X * 0.5) * Math.Cos(v.Y * 0.5) * Math.Sin(v.Z * 0.5))).FixFloat(),
            Z = ((float)(Math.Cos(v.X * 0.5) * Math.Cos(v.Y * 0.5) * Math.Sin(v.Z * 0.5) - Math.Sin(v.X * 0.5) * Math.Sin(v.Y * 0.5) * Math.Cos(v.Z * 0.5))).FixFloat()
        };

    }

    public static Vector3 ToEulerAngles(this Quaternion q)
    {
        Vector3 angles = new();

        // roll / x
        double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
        double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
        angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

        // pitch / y
        double sinp = 2 * (q.W * q.Y - q.Z * q.X);
        if (Math.Abs(sinp) >= 1)
        {
            angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
        }
        else
        {
            angles.Y = (float)Math.Asin(sinp);
        }

        // yaw / z
        double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
        double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
        angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

        angles.X *= (float)(180f / Math.PI);
        angles.Y *= (float)(180f / Math.PI);
        angles.Z *= (float)(180f / Math.PI);

        return angles;
    }

    public static float FixFloat(this float number)
    {
        if (Math.Abs(number) < 1E-6f) // Check if number is close to zero
        {
            return 0f; // Return zero if number is close to zero
        }

        if (Math.Abs(number - 0.5)  < 1E-6f) // Check if number is close to zero
        {
            var amt = Math.Abs(number) - 0.5;
            return 0.5f; // Return zero if number is close to zero
        }

        if (Math.Abs(number - (int)number) < 1E-6f) // Check if number is close to an integer
        {
            return (int)number; // Return the integer value if number is close to an integer
        }

        return number;
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

    public static void WriteMatrix4X4(this BinaryWriter writer, Matrix4x4 matrix)
    {
        writer.Write(matrix.M11);
        writer.Write(matrix.M12);
        writer.Write(matrix.M13);
        writer.Write(matrix.M41);

        writer.Write(matrix.M21);
        writer.Write(matrix.M22);
        writer.Write(matrix.M23);
        writer.Write(matrix.M42);

        writer.Write(matrix.M31);
        writer.Write(matrix.M32);
        writer.Write(matrix.M33);
        writer.Write(matrix.M43);

        writer.Write(matrix.M44);
        writer.Write(matrix.M34);
        writer.Write(matrix.M24);
        writer.Write(matrix.M14);
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
