using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using War3Net.Common.Extensions;
using WCSharp.ConstantGenerator;

namespace Launcher
{
    internal static class Program
    {
        // Warcraft III
        public const string GRAPHICS_API = "Direct3D9";

        private static void Main()
        {
            MakeDecision();
        }

        private static void MakeDecision()
        {
            Console.WriteLine("The following actions are available:");
            Console.WriteLine("1. Generate constants");
            Console.WriteLine("2. Run ObjectMerger");
            Console.WriteLine("3. Compile map");
            Console.WriteLine("4. Compile & Run map");
            Console.WriteLine("5. Compile without ObjectMerger");
            Console.WriteLine("6. Compile & Run without ObjectMerger");
            Console.Write("Please type the number of your desired action: ");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    ConstantGenerator.Run(BuilderConfig.MAP_PATH, BuilderConfig.SOURCE_CODE_PATH, new ConstantGeneratorOptions
                    {
                        IncludeCode = true
                    });
                    break;

                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                case ConsoleKey.NumPad4:
                case ConsoleKey.D4:
                    Console.Clear();
                    Console.WriteLine($"{Environment.NewLine}Unimplemented input.");
                    Console.WriteLine("");
                    MakeDecision();
                    break;

                case ConsoleKey.NumPad5:
                case ConsoleKey.D5:
                    Console.WriteLine("");
                    MapBuilder.Build();
                    Console.WriteLine("Build finishes, you can close this console for now");
                    break;

                case ConsoleKey.NumPad6:
                case ConsoleKey.D6:
                    Console.WriteLine("");
                    var mapPath = MapBuilder.Build();
                    LaunchMap(mapPath);
                    Console.WriteLine("Build finishes, you can close this console for now");
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine($"{Environment.NewLine}Invalid input. Please choose again.{Environment.NewLine}");
                    MakeDecision();
                    break;
            }
        }

        public static void LaunchMap(string path)
        {
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(BuilderConfig.BASE_PATH + @"Launcher\app.config.json"));
            string wc3exe = config["warcraftExecutable"];

            if (File.Exists(wc3exe))
            {
                var commandLineArgs = new StringBuilder();
                var isReforged = Version.Parse(FileVersionInfo.GetVersionInfo(wc3exe).FileVersion) >= new Version(1, 32);
                if (isReforged) commandLineArgs.Append(" -launch");

                commandLineArgs.Append($" -graphicsapi ").Append(Program.GRAPHICS_API);
                commandLineArgs.Append(" -nowfpause"); // pause on loose focus

                var absoluteMapPath = new FileInfo(path).FullName;
                commandLineArgs.Append($" -loadfile \"{absoluteMapPath}\"");

                Process.Start(wc3exe, commandLineArgs.ToString());
                Console.WriteLine("The map should be loaded on reforged now...");
            }
            else
            {
                throw new AggregateException("Please set wc3exe in Launcher/app.config to the path of your Warcraft III executable.");
            }
        }
    }
}
