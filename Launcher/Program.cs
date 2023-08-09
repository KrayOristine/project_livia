using CSharpLua;
using Microsoft.CodeAnalysis;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using War3Net.Build;
using War3Net.Build.Extensions;
using War3Net.IO.Mpq;
using WCSharp.ConstantGenerator;
using Jering.Javascript.NodeJS;
using System.Reflection.Metadata;
using System.Collections.Generic;

namespace Launcher
{
    internal static class Program
    {
        // Input
        private const string SOURCE_CODE_PROJECT_FOLDER_PATH = @"..\..\..\..\Source";
        private const string ASSETS_FOLDER_PATH = @"..\..\..\..\Assets\";
        private const string BASE_MAP_NAME = "source.w3x";
        private const string BASE_MAP_PATH = @"..\..\..\..\Resources\BaseMap\" + BASE_MAP_NAME;

        // Output
        private const string OUTPUT_FOLDER_PATH = @"..\..\..\..\Artifacts";
        private const string OUTPUT_SCRIPT_NAME = @"war3map.lua";
        private const string OUTPUT_MAP_NAME = @"target.w3x";

        // Warcraft III
        private const string GRAPHICS_API = "Direct3D9";
        private const bool PAUSE_GAME_ON_LOSE_FOCUS = false;
#if DEBUG
        private const bool DEBUG = true;
#else
		private const bool DEBUG = false;
#endif

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
                    ConstantGenerator.Run(BASE_MAP_PATH, SOURCE_CODE_PROJECT_FOLDER_PATH, new ConstantGeneratorOptions
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
                    Console.Write($"{Environment.NewLine}Unimplemented input.");
                    Console.WriteLine("");
                    MakeDecision();
                    break;

                case ConsoleKey.NumPad5:
                case ConsoleKey.D5:
                    Console.WriteLine("");
                    Build(false);
                    break;
                case ConsoleKey.NumPad6:
                case ConsoleKey.D6:
                    Console.WriteLine("");
                    Build(true);
                    break;
                default:
					Console.Clear();
                    Console.WriteLine($"{Environment.NewLine}Invalid input. Please choose again.{Environment.NewLine}");
                    MakeDecision();
                    break;
            }
        }

        public static string MinifyScript(string script)
        {
            var result = StaticNodeJSService.InvokeFromFileAsync<string>(@"..\..\..\nodejs\minify.js", null, new object[] { script }).GetAwaiter().GetResult();

            return result;
        }

        public static void AddFiles(MapBuilder builder, Map map, string path, string searchPattern, SearchOption searchOption)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder), "builder param can't be null");
            var directoryLength = path.Length;
            if (!path.EndsWith('/') && !path.EndsWith('\\')) directoryLength++;
            List<MpqFile> filesList = new();
            foreach (var file in Directory.EnumerateFiles(path, searchPattern, searchOption))
            {
                var name = file[directoryLength..];
                var fileStream = File.OpenRead(file);
                if (!map.SetFile(name, false, fileStream))
                {
                    var mpq = MpqFile.New(fileStream, name, MpqLocale.Neutral, false);
                    // do stuff with it

                    filesList.Add(mpq);
                }
            }
            builder.AddFiles(filesList.AsEnumerable());
        }

        public static void Build(bool launch)
        {
            // Ensure these folders exist
            Directory.CreateDirectory(ASSETS_FOLDER_PATH);
            Directory.CreateDirectory(OUTPUT_FOLDER_PATH);

            // Load existing map data
            var map = Map.Open(BASE_MAP_PATH);
            var builder = new MapBuilder(map);

            AddFiles(builder, map, BASE_MAP_PATH, "*", SearchOption.AllDirectories);
            AddFiles(builder, map, ASSETS_FOLDER_PATH, "*", SearchOption.AllDirectories);

            // Set debug options if necessary, configure compiler
#if DEBUG
            var csc = "-debug -define:DEBUG";
#else
            var csc = string.Empty;
#endif
            var csproj = Directory.EnumerateFiles(SOURCE_CODE_PROJECT_FOLDER_PATH, "*.csproj", SearchOption.TopDirectoryOnly).Single();
            var meta = string.Join(";", Directory.EnumerateFiles(SOURCE_CODE_PROJECT_FOLDER_PATH, "*.meta.xml", SearchOption.AllDirectories));

            var compiler = new Compiler(csproj, OUTPUT_FOLDER_PATH, string.Empty, meta, "War3Api.*;WCSharp.*", "", csc, false, null, string.Empty)
            {
                IsExportMetadata = true,
                IsModule = false,
                IsInlineSimpleProperty = false,
                IsPreventDebugObject = true,
                IsCommentsDisabled = true,
            };

            // Collect required paths and compile
            var coreSystemFiles = CSharpLua.CoreSystem.CoreSystemProvider.GetCoreSystemFiles();
            var blizzardJ = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Warcraft III/JassHelper/Blizzard.j");
            var commonJ = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Warcraft III/JassHelper/common.j");
            var compileResult = map.CompileScript(compiler, coreSystemFiles, blizzardJ, commonJ);

            // If something wrong happen print it out
            if (!compileResult.Success)
            {
                Console.Clear();
                foreach (Diagnostic d in compileResult.Diagnostics)
                {
                    if (d.Severity == DiagnosticSeverity.Warning && (d.Id == "CS0626" || d.Id == "CS8632")) continue;

                    Console.WriteLine("=============================================");
                    Console.WriteLine(d.GetMessage());
                    var pos = d.Location.GetLineSpan();
                    Console.Write($"In {pos.Path}:line {pos.StartLinePosition.Line}, char {pos.StartLinePosition.Character}\n");
                }
                Console.WriteLine("=============================================");
                throw new Exception("Compiler error, see all above message for more details information");
            }

#if !DEBUG
            map.Script = MinifyScript(map.Script);
#endif

            // Update war3map.lua so you can inspect the generated Lua code easily
            File.WriteAllText(Path.Combine(OUTPUT_FOLDER_PATH, OUTPUT_SCRIPT_NAME), map.Script);


            // Build w3x file
            var archiveCreateOptions = new MpqArchiveCreateOptions
            {
                ListFileCreateMode = MpqFileCreateMode.Prune,
                AttributesCreateMode = MpqFileCreateMode.Prune,
                BlockSize = 3,
            };

            builder.Build(Path.Combine(OUTPUT_FOLDER_PATH, OUTPUT_MAP_NAME), archiveCreateOptions);

            // Launch if that option was selected
            if (launch)
            {
                var wc3exe = ConfigurationManager.AppSettings["wc3exe"];
                if (File.Exists(wc3exe))
                {
                    var commandLineArgs = new StringBuilder();
                    var isReforged = Version.Parse(FileVersionInfo.GetVersionInfo(wc3exe).FileVersion) >= new Version(1, 32);
                    if (isReforged)
                    {
                        commandLineArgs.Append(" -launch");
                    }
                    else if (GRAPHICS_API != null)
                    {
                        commandLineArgs.Append($" -graphicsapi {GRAPHICS_API}");
                    }

                    if (!PAUSE_GAME_ON_LOSE_FOCUS)
                    {
                        commandLineArgs.Append(" -nowfpause");
                    }

                    var mapPath = Path.Combine(OUTPUT_FOLDER_PATH, OUTPUT_MAP_NAME);
                    var absoluteMapPath = new FileInfo(mapPath).FullName;
                    commandLineArgs.Append($" -loadfile \"{absoluteMapPath}\"");

                    Process.Start(wc3exe, commandLineArgs.ToString());
                    Console.WriteLine("The map should be loaded on reforged now...");
                }
                else
                {
                    throw new AggregateException("Please set wc3exe in Launcher/app.config to the path of your Warcraft III executable.");
                }
            }
            else Console.WriteLine("Build finishes, you can close this console for now");
        }
    }
}
