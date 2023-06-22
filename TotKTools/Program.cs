using BHTMP;
using IconBuilder;
using SkiaSharp;
using ToolLib;

namespace TotKTools
{
    internal class Program
    {
        public static void Main()
        {

                 var size = (int)(58f * (58f / 40f));

                 var newIcon = new Icon(58, 58);


               /*var shadowLayer = newIcon.AddLayer(@"D:\TotK-Sorted\Icons\MapIconImportantShopSh_58x58^s.png");
               shadowLayer.Opacity = 150 / 255f;
               shadowLayer.Scale = 58f/40f;
             shadowLayer.WhiteColor = new SKColor(0, 0, 0);
             shadowLayer.BlackColor = new SKColor(0, 0, 0, 0);*/

             var iconLayer = newIcon.AddLayer(@"D:\TotK-Sorted\Icons\MapIconBuildingStableInn_58x58^w.png");
            // iconLayer.Scale = 0.65f;
             iconLayer.WhiteColor = new SKColor(150, 120, 40);
             iconLayer.BlackColor = new SKColor(111, 66, 16);

              newIcon.Save(@"D:\TotK-Sorted\Icons\HouseIcon.png");


            /*
            var black = new SKColor(0, 0, 255, 0);
            var white = new SKColor(0, 150, 255, 255);

            var image = ColorReplacer.Load(@"D:\TotK-Sorted\Icons\Text.png", white, black);
              image.Save(@"D:\TotK-Sorted\Icons\Text-Color.png");*/
            /*
            var imageFrame = ColorReplacer.Load(@"D:\TotK-Sorted\Icons\Out.png", white, black, 220 / 255f);
            imageFrame.Save(@"D:\TotK-Sorted\Icons\Out-Color.png");

            var imageText = ColorReplacer.Load(@"D:\TotK-Sorted\Icons\Text.png", white, black, 220 / 255f);
            imageText.Save(@"D:\TotK-Sorted\Icons\Text-Color.png");*/

            //    var fream = ColorReplacer.Load(@"D:\TotK-Sorted\Icons\Frame.png", white, black, 220 / 255f);
            //    fream.Save(@"D:\TotK-Sorted\Icons\Frame-Color.png");

            //     var white = new SKColor(80, 80, 13, 255);
            //     var black = new SKColor(255,140, 30, 0);
            //var white = new SKColor(255, 255, 100, 255);
            // var black = new SKColor(0, 0, 0, 0);



            //var image = ColorReplacer.Load(@"D:\TotK-Sorted\Icons\Light-Circle.png", white, black, 256 / 255f);
            //  image.Save(@"D:\TotK-Sorted\Icons\Light-Circle-Color.png");

        }

        public static void CreateHeightMaps(string pathToRaw, string pathOutput, bool writeSubImages = false)
        {
            void CreateHeightMap(string type, float floor, float ceiling)
            {
                var files = Directory.GetFiles(pathToRaw, $"{type}_*.bhtmp");
                var surface = SKSurface.Create(new SKImageInfo(1200, 1000));
                var canvas = surface.Canvas;
                var max = -5000f;
                var min = 5000f;
                foreach (var file in files)
                {
                    var coords = Path.GetFileNameWithoutExtension(file).Replace($"{type}_", "").Split("-");
                    var x = Convert.ToSingle(coords[0]) * 100;
                    var y = Convert.ToSingle(coords[1]) * 100;
                    var fil = BHTMPFile.Open(file, new BHTMPFileOptions { Ceiling = ceiling, Floor = floor });
                    canvas.DrawBitmap(fil.HeightMap, x, y);

                    max = Math.Max(fil.MaxValue, max);
                    min = Math.Min(fil.MinValue, min);

                    if (writeSubImages)
                    {
                        var smallFileOut = $"{pathOutput.TrimEnd('\\')}\\HeightMap-{type}_{x}_{y}.png";
                        fil.Save(smallFileOut);
                    }
                }

                var fileOut = $"{pathOutput.TrimEnd('\\')}\\HeightMap-{type}.png";
                Console.WriteLine($"Min: {min} Max: {max}");
                Console.WriteLine($"{fileOut}");
                Directory.CreateDirectory(pathOutput);
                surface.Save(fileOut);
            }

            CreateHeightMap("G", -100, 1000);
            CreateHeightMap("U", -3000, -100);
            CreateHeightMap("S", 400, 3200);
        }
    }
}
