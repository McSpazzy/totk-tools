using SkiaSharp;
using ToolLib;

namespace IconBuilder
{
    public class Icon
    {
        public int Height { get; }
        public int Width { get; }

        private SKBitmap _bitMap;
        private SKCanvas _canvas;

        private readonly List<IconLayer> _layers;

        public Icon(int height, int width)
        {
            Height = height;
            Width = width;

            _layers = new List<IconLayer>();
            _bitMap = new SKBitmap(width, height);
            _canvas = new SKCanvas(_bitMap);
        }

        public IconLayer AddLayer(string fileName)
        {
            var newLayer = new IconLayer(fileName);
            _layers.Add(newLayer);
            return newLayer;
        }

        public void RemoveLayer(IconLayer layer)
        {
            if (_layers.Contains(layer))
            {
                _layers.Remove(layer);
            }
        }

        public void Save(string filename, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
        {
            DrawCanvas();
            _bitMap.Save(filename, format, quality);
        }

        private void DrawCanvas()
        {
            foreach (var layer in _layers)
            {
                var interpolatedImage = layer.GetBitmap();
                var posX = (int) ((Width - (interpolatedImage.Width * layer.Scale)) / 2.0);
                var posY = (int) ((Height - (interpolatedImage.Height * layer.Scale)) / 2.0);
                _canvas.DrawBitmap(interpolatedImage, new SKRect(posX, posY, posX + interpolatedImage.Width * layer.Scale, posY + interpolatedImage.Height * layer.Scale));
            }
        }
    }

    public class IconLayer
    {
        private readonly SKBitmap _bitMap;

        public SKColor WhiteColor { get; set; } = new(255, 255, 255);
        public SKColor BlackColor { get; set; } = new(0,0,0);
        public float Opacity { get; set; } = 1.0f;
        public float Scale { get; set; } = 1f;

        public IconLayer(string fileName)
        {
            _bitMap = SKBitmap.Decode(fileName);
        }

        public SKBitmap GetBitmap()
        {
            return ColorReplacer.Process(_bitMap, WhiteColor, BlackColor, Opacity);
        }
    }
}
