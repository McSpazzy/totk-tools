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
