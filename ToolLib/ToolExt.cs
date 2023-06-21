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
