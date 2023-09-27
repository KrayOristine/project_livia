using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using War3Net.Build;
using War3Net.Build.Extensions;
using CSharpLua;
using Microsoft.CodeAnalysis;
using System.IO;

namespace Launcher
{
    public static class MapScriptCompiler
    {
        public static void Run(ref Map map)
        {

            // Set debug options if necessary, configure compiler
#if DEBUG
            var csc = "-debug -define:DEBUG";
#else
            var csc = string.Empty;
#endif
            var csproj = Directory.EnumerateFiles(MapBuilder.SOURCE_CODE_PATH, "*.csproj", SearchOption.TopDirectoryOnly).Single();
            // configure 'c sharp lua' compiler
            var compiler = new Compiler(csproj, MapBuilder.OUT_ARTIFACTS, string.Empty, string.Empty, "War3Api.*;WCSharp.*", "", csc, false, null, string.Empty)
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
                throw new AggregateException("Compiler error, see all above message for more details information");
            }

#if DEBUG
            // built-in error log caching to prevent shit happen at the time the game being loaded but no error log is displayed
            bool foundInitCSharp = false;
            bool skipRest = false;
            int scriptIndex = -1;
            var mapScript = map.Script.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new();
            // Add try catch on InitCSharp
            foreach (var lines in mapScript)
            {
                scriptIndex++;
                if (skipRest)
                {
                    sb.AppendLine(lines);
                    if (lines == "    InitCSharp()")
                    {
                        sb.AppendLine("    if _G[\"InitCSharpError\"] and _G[\"InitCSharpError\"][\"isError\"] then");
                        sb.AppendLine("        TimerStart(CreateTimer(), 1, false, function()");
                        sb.AppendLine("            print(table.unpack(_G[\"InitCSharpError\"][\"errorMsg\"]))");
                        sb.AppendLine("            _G[\"InitCSharpError\"] = nil");
                        sb.AppendLine("            DestroyTimer(GetExpiredTimer())");
                        sb.AppendLine("        end)");
                        sb.AppendLine("    end");
                    }
                    continue;
                }

                string mod = string.Empty;
                if (lines == "local InitCSharp = function ()")
                {
                    foundInitCSharp = true;
                    mod = Environment.NewLine + "  local result, ret = xpcall(function()";
                }
                if (lines == "end" && foundInitCSharp)
                {
                    mod = ", function(...) _G[\"InitCSharpError\"] = {};_G[\"InitCSharpError\"][\"isError\"] = true;_G[\"InitCSharpError\"][\"errorMsg\"] = table.pack(...);end)" + Environment.NewLine + "end";
                    foundInitCSharp = false;
                    skipRest = true;
                }
                // if no modify
                sb.Append(lines);
                if (!string.IsNullOrEmpty(mod)) sb.Append(mod);

                sb.Append(Environment.NewLine);
            }
            map.Script = sb.ToString();
#endif

            // minify on release
#if !DEBUG
            map.Script = MinifyScript(map.Script);
#endif

            // Update war3map.lua so you can inspect the generated Lua code easily
            File.WriteAllText(Path.Combine(MapBuilder.OUT_ARTIFACTS, MapBuilder.OUT_SCRIPT_NAME), map.Script);
        }
    }
}
