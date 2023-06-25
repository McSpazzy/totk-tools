using BHTMP;
using IconBuilder;
using SkiaSharp;
using CombinedActorInfo;
using Newtonsoft.Json;
using ToolLib;
using SaveEdit;

namespace TotKTools
{
    internal class Program
    {


        static void Main(string[] args)
        {

            var save = File.ReadAllBytes(@"C:\Users\Andy\AppData\Roaming\yuzu\nand\user\save\0000000000000000\F9605B3AE65AAD8E1D0F4E8D2D4B2B08\0100F2C0115B6000\slot_00\progress.sav");

            var saveop = new SaveFileOptions();

            saveop.WordFile = @"D:\TotK-Data-Deploy\WordList.txt";
            saveop.ShowDetails = true;
            saveop.ExportAutoBuild = @"D:\AutoBuild-Export\";
            saveop.SerializeAutoBuildData = true;

            var saveFile = SaveFile.Open(save, saveop);

            File.WriteAllText(@"d:\saveText.json", JsonConvert.SerializeObject(saveFile, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));


            var fil = CombinedActorInfoFile.Open(@"D:\TotK-Data\AutoBuild\AutoBuild-20.cbi");
            var json = JsonConvert.SerializeObject(fil.CombinedActor, Formatting.Indented, new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore});
            File.WriteAllText(@"d:\testOut.json", json);

            var test = CombinedActorInfoFile.FromFile(@"d:\testOut.json"); // Load json after modified or whatever
            test.Save(@"d:\newFile.cbi");

            // Validation
            var testInputFile = CombinedActorInfoFile.Open(@"d:\newFile.cbi");
            var json2 = JsonConvert.SerializeObject(testInputFile.CombinedActor, Formatting.Indented, new JsonSerializerSettings() {NullValueHandling = NullValueHandling.Ignore});
            File.WriteAllText(@"d:\testOut2.json", json2); // should match the edited json


            return;


            //     var newIcon = new Icon(size, size);

            //   var shadowLayer = newIcon.AddLayer(@"D:\TotK-Sorted\Icons\MapIconImportant.png");
            //   shadowLayer.Opacity = 150 / 255f;
            //   shadowLayer.Scale = 1 / 0.65f;
            // shadowLayer.WhiteColor = new SKColor(0, 0, 0);
            // shadowLayer.BlackColor = new SKColor(0, 0, 0, 0);

            // var iconLayer = newIcon.AddLayer(@"D:\TotK-Sorted\Icons\MapIconBuildingVillage.png");
            // iconLayer.Scale = 0.65f;
            // iconLayer.WhiteColor = new SKColor(150, 120, 40);
            // iconLayer.BlackColor = new SKColor(111, 66, 16);

            //  newIcon.Save(@"D:\TotK-Sorted\Icons\HouseIcon.png");

            // CreateHeightMaps(@"D:\TotK-Sorted\HeightMap-Raw\", @"D:\TotK-Sorted\HeightMaps\");

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
