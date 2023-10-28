using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using War3Net.Build;
using War3Net.Build.Extensions;
using CSharpLua;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Globalization;
using War3Net.CodeAnalysis.Transpilers;
using War3Net.CodeAnalysis.Jass;
using Microsoft.CodeAnalysis.Emit;

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
            var csproj = Directory.EnumerateFiles(BuilderConfig.SOURCE_CODE_PATH, "*.csproj", SearchOption.TopDirectoryOnly).Single();
            // configure 'c sharp lua' compiler
            var compiler = new Compiler(csproj, BuilderConfig.OUT_ARTIFACTS, string.Empty, string.Empty, "War3Api.*;WCSharp.*", "", csc, false, null, string.Empty)
            {
                IsExportMetadata = false,
                IsModule = false,
                IsInlineSimpleProperty = false,
                IsPreventDebugObject = true,
                IsCommentsDisabled = true,
            };

            // Collect required paths and compile
            var coreSystemFiles = CSharpLua.CoreSystem.CoreSystemProvider.GetCoreSystemFiles();
            var blizzardJ = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Warcraft III/JassHelper/Blizzard.j");
            var commonJ = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Warcraft III/JassHelper/common.j");
            var builder = new MapScriptBuilder()
            {
                LobbyMusic = null,
                MaxPlayerSlots = 24,
                ForceGenerateGlobalUnitVariable = true,
                ForceGenerateGlobalDestructableVariable = false,
                ForceGenerateUnitWithSkin = false,
                ForceGenerateDestructableWithSkin = false,
                UseCSharpLua = true,
                UseLifeVariable = false,
                UseWeatherEffectVariable = false
            };
            var compileResult = Compile(ref map, compiler, builder, coreSystemFiles, blizzardJ, commonJ);


            // If something wrong happen print it out
            if (compileResult is not null && !compileResult.Success)
            {
                Console.Clear();
                foreach (Diagnostic d in compileResult.Diagnostics)
                {
                    if (d.Severity == DiagnosticSeverity.Warning && (d.Id == "CS0626" || d.Id == "CS8632")) continue;

                    Console.WriteLine("=============================================");
                    Console.WriteLine(d.GetMessage(CultureInfo.InvariantCulture));
                    var pos = d.Location.GetLineSpan();
                    Console.Write($"In {pos.Path}:line {pos.StartLinePosition.Line}, char {pos.StartLinePosition.Character}\n");
                }
                Console.WriteLine("=============================================");
                throw new AggregateException("Compiler error, see all above message for more details information");
            }

#if DEBUG
            // lua safe-call wrap on InitCSharp
            WrapInitMethod(ref map);
#else
            // obfuscate then minify on release
            map.Script = MapScriptObfuscator.ObfuscateScript(map.Script);
            map.Script = MapProtector.MinifyScript(map.Script);
#endif

            // Update war3map.lua so you can inspect the generated Lua code easily
            File.WriteAllText(Path.Combine(BuilderConfig.OUT_ARTIFACTS, BuilderConfig.OUT_SCRIPT_NAME), map.Script);
        }

        public static EmitResult Compile(ref Map map, Compiler compiler, MapScriptBuilder mapScriptBuilder, IEnumerable<string> luaSystemLibs, string commonJPath, string blizzardJPath)
        {
            using var stream = new MemoryStream();

            try
            {
                compiler.CompileSingleFile(stream, luaSystemLibs);
            }
            catch (CompilationErrorException e) when (e.EmitResult is not null)
            {
                return e.EmitResult;
            }

            var transpiler = new JassToLuaTranspiler
            {
                IgnoreComments = true,
                IgnoreEmptyDeclarations = true,
                IgnoreEmptyStatements = true,
                KeepFunctionsSeparated = true
            };

            var commonUnit = JassSyntaxFactory.ParseCompilationUnit(File.ReadAllText(commonJPath));
            var blizzardUnit = JassSyntaxFactory.ParseCompilationUnit(File.ReadAllText(blizzardJPath));

            transpiler.RegisterJassFile(commonUnit);
            transpiler.RegisterJassFile(blizzardUnit);

#if !DEBUG
            MapScriptObfuscator.ParseCommonAndBlizzard(commonUnit);
            MapScriptObfuscator.ParseCommonAndBlizzard(blizzardUnit);
#endif

            var luaCompilationUnit = transpiler.Transpile(mapScriptBuilder.Build(map));
            using (var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                var luaRenderOptions = new LuaSyntaxGenerator.SettingInfo
                {
                    Indent = 2,
                };

                var luaRenderer = new LuaRenderer(luaRenderOptions, writer);
                luaRenderer.RenderCompilationUnit(luaCompilationUnit);
            }

            stream.Position = 0;
            map.SetScriptFile(stream);
            return null;
        }

        public static void WrapInitMethod(ref Map map){
            // built-in error log caching to prevent shit happen at the time the game being loaded, rendering error log won't be displayed
            bool foundInitCSharp = false;
            bool skipRest = false;
            int scriptIndex = -1;
            var mapScript = map.Script.Split('\n', StringSplitOptions.RemoveEmptyEntries);
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
                        sb.AppendLine("            DestroyTimer(GetExpiredTimer())");
                        sb.AppendLine("            print(table.unpack(_G[\"InitCSharpError\"][\"errorMsg\"]))");
                        sb.AppendLine("            _G[\"InitCSharpError\"] = nil");
                        sb.AppendLine("        end)");
                        sb.AppendLine("    end");
                    }
                    continue;
                }

                string mod = string.Empty;
                if (lines == "local InitCSharp = function ()")
                {
                    foundInitCSharp = true;
                    mod = "\n  local result, ret = xpcall(function()";
                }
                if (lines == "end" && foundInitCSharp)
                {
                    mod = ", function(...) _G[\"InitCSharpError\"] = {};_G[\"InitCSharpError\"][\"isError\"] = true;_G[\"InitCSharpError\"][\"errorMsg\"] = table.pack(...);end)\nend";
                    foundInitCSharp = false;
                    skipRest = true;
                }
                // if no modify
                sb.Append(lines);
                if (!string.IsNullOrEmpty(mod)) sb.Append(mod);

                sb.Append('\n');
            }
            map.Script = sb.ToString();
        }
    }
}
