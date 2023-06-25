using System.CommandLine;
using Newtonsoft.Json;

namespace SaveEdit
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var saveFile = "";
            string? outFile = null;
            string? wordFile = null;
            var showStats = false;

            var rootCommand = new RootCommand("TotK Save Parser & Modifier");

            var saveFileArgument = new Argument<string>("saveFile");

            var wordFileOption = new Option<string>("--wordFile", "Path to WordFile containing property names");
            wordFileOption.AddAlias("-wf");

            var outFileOption = new Option<string>("--outFile", "Path to save output");
            outFileOption.AddAlias("-o");

            var statsOption = new Option<bool>("--verbose", "Show file details");
            statsOption.AddAlias("-v");

            rootCommand.AddArgument(saveFileArgument);
            rootCommand.AddOption(wordFileOption);
            rootCommand.AddOption(outFileOption);
            rootCommand.AddOption(statsOption);

            rootCommand.SetHandler((saveFileArgumentValue, wordFileValue, outFileValue, statsOptionValue) =>
            {
                if (!File.Exists(saveFileArgumentValue))
                {
                    Console.WriteLine($"Supplied saveFile path does not exist '{saveFileArgumentValue}'");
                    Environment.Exit(0);
                }

                if (!string.IsNullOrEmpty(wordFileValue))
                {
                    if (!File.Exists(wordFileValue))
                    {
                        Console.WriteLine($"Supplied wordFile path does not exist '{wordFileValue}'");
                        Environment.Exit(0);
                    }

                    wordFile = wordFileValue;
                }

                if (!string.IsNullOrEmpty(outFileValue))
                {
                    if (Directory.Exists(outFileValue))
                    {
                        Console.WriteLine($"Supplied outFile is a directory '{outFileValue}'");
                        Environment.Exit(0);
                    }

                    outFile = outFileValue;
                }

                saveFile = saveFileArgumentValue;
                showStats = statsOptionValue;

            }, saveFileArgument, wordFileOption, outFileOption, statsOption);
            
            await rootCommand.InvokeAsync(args);

            var fileData = await File.ReadAllBytesAsync(saveFile);

            var saveOptions = new SaveFileOptions
            {
                WordFile = wordFile,
                ShowDetails = showStats
            };

            var saveFileObject = SaveFile.Open(fileData, saveOptions);

            if (!string.IsNullOrEmpty(outFile))
            {
                await File.WriteAllTextAsync(outFile, JsonConvert.SerializeObject(saveFileObject, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            }
        }
    }
}
