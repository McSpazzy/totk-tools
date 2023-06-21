using SkiaSharp;

namespace IconBuilder
{
    public class ColorReplacer
    {
        public static SKBitmap Load(string filename, SKColor white, SKColor black, float opacity = 1.0f)
        {
            var grayScaleImage = SKBitmap.Decode(filename);
            var modifiedImage = new SKBitmap(grayScaleImage.Width, grayScaleImage.Height);

            try
            {
                using var canvas = new SKCanvas(modifiedImage);
                using var paint = new SKPaint();
                for (var x = 0; x < grayScaleImage.Width; x++)
                {
                    for (var y = 0; y < grayScaleImage.Height; y++)
                    {
                        var pixel = grayScaleImage.GetPixel(x, y);
                        var factor = pixel.Red / 255f;
                  //      var alpha = (byte)(pixel.Alpha * opacity);

                        paint.Color = InterpolateColors(black, white, factor, opacity);
                        canvas.DrawPoint(x, y, paint);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return modifiedImage;
        }

        public static SKBitmap Process(SKBitmap bitmap, SKColor white, SKColor black, float opacity = 1.0f)
        {
            var modifiedImage = new SKBitmap(bitmap.Width, bitmap.Height);
            using var canvas = new SKCanvas(modifiedImage);
            using var paint = new SKPaint();
            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var factor = pixel.Red / 255f;
                    paint.Color = InterpolateColors(black, white, factor, opacity);
                    canvas.DrawPoint(x, y, paint);
                }
            }
            return modifiedImage;
        }

        private static SKColor InterpolateColors(SKColor startColor, SKColor endColor, float factor, float opacity)
        {
            var r = (byte)(startColor.Red + (endColor.Red - startColor.Red) * factor);
            var g = (byte)(startColor.Green + (endColor.Green - startColor.Green) * factor);
            var b = (byte)(startColor.Blue + (endColor.Blue - startColor.Blue) * factor);
            var a = (byte)(startColor.Alpha + (endColor.Alpha - startColor.Alpha) * factor);
            return new SKColor(ApplyGammaCorrection(r), ApplyGammaCorrection(g), ApplyGammaCorrection(b), (byte)(a * opacity));
        }

        private static byte ApplyGammaCorrection(byte channel, float amount = 2.2f)
        {
            return (byte)(Math.Pow(channel / 255f, 1 / amount) * 255);
        }
    }
}
