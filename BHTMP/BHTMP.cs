using ToolLib;
using SkiaSharp;

namespace BHTMP;

public class BHTMPFile : FileReader<BHTMPFile, BHTMPFileOptions>
{
    protected override byte[] Magic { get; set; } = Array.Empty<byte>();

    public SKBitmap? HeightMap;

    public float MaxValue { get; set; } = -5000f;
    public float MinValue { get; set; } = 5000f;

    public void Save(string filename, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        HeightMap?.Encode(format, quality).SaveTo(new FileStream(filename, FileMode.OpenOrCreate));
    }

    public static SKColor ConvertToGrayScale(double value, double minValue, double maxValue)
    {
        if (minValue >= maxValue)
        {
            throw new ArgumentException("The minimum value must be less than the maximum value");
        }

        if (value < minValue)
        {
            value = minValue;
        }

        if (value > maxValue)
        {
            value = maxValue;
        }

        var intensity = (byte) (255 * ((value - minValue) / (maxValue - minValue)));
        return new SKColor(intensity, intensity, intensity);
    }

    public BHTMPFile(BinaryReader reader, BHTMPFileOptions? options = null) : base(reader, options)
    {
        var height = Reader.ReadInt32();
        var width = Reader.ReadInt32();
        var unk1 = Reader.ReadInt32();
        var unk2 = Reader.ReadInt32();

        HeightMap = new SKBitmap(height, width);


        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var value = Reader.ReadSingle();
                var unkValue = Reader.ReadSingle();
              
                if (value != 0.0)
                {
                    MaxValue = Math.Max(MaxValue, value);
                    MinValue = Math.Min(MinValue, value);
                }
                
                HeightMap.SetPixel(j, i, ConvertToGrayScale(value, Options.Floor, Options.Ceiling));
            }
        }
    }
}

public class BHTMPFileOptions
{
    public float Floor { get; set; } = 0f;
    public float Ceiling { get; set; } = 1000f;
}
