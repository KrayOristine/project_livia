using CSharpLua;
using Launcher.MapObject;
using LibZopfliSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using War3Net.Build;
using War3Net.Build.Extensions;
using War3Net.IO.Mpq;
using War3Net.IO.Mpq.Extensions;

namespace Launcher
{
    public static class MapBuilder
    {
        // The base path to the solution
        public const string BASE_PATH = @"D:\dev\project_livia\";

        public const string SOURCE_CODE_PATH = BASE_PATH + @"Source\";
        public const string ASSETS_PATH = BASE_PATH + @"Assets\";
        public const string MAP_PATH = BASE_PATH + @"BaseMap\" + BASE_MAP_NAME;
        public const string BASE_MAP_NAME = "source.w3x";

        // Output
        public const string OUT_ARTIFACTS = BASE_PATH + @"Artifacts\";

        public const string OUT_SCRIPT_NAME = @"war3map.lua";
        public const string OUT_MAP_NAME = @"target.w3x";

        public static MemoryStream ReadFile(string path)
        {
            var memoryStream = new MemoryStream();
            using (FileStream fileStream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan))
            {
                fileStream.CopyTo(memoryStream);
            }
            return memoryStream;
        }

        public static void AddFiles(ref List<MpqFile> fileList, string path, string searchPattern, SearchOption searchOption)
        {
            if (fileList == null) throw new ArgumentNullException(nameof(fileList), "fileList param can't be null");
            var directoryLength = path.Length;
            if (!path.EndsWith('/') && !path.EndsWith('\\')) directoryLength++;
            foreach (var file in Directory.EnumerateFiles(path, searchPattern, searchOption))
            {
                var warPath = file[directoryLength..];
                var fileName = Path.GetFileName(warPath);
                var directory = Path.GetDirectoryName(warPath);
                if (!MapProtector.IsWhiteListed(fileName) || MapProtector.IsWorldEditOnly(fileName.ToLowerInvariant())) continue;

                var stream = ReadFile(file);

                // add shit to it
                var newName = MapProtector.ConfuseAndNormalize(fileName);
                var compressed = new MemoryStream(MapCompression.CompressStream(ref stream));
                stream.Dispose();

                var mpq = MpqFile.New(compressed, Path.Combine(directory, newName), false);
                fileList.Add(mpq);
            }
        }

        public static string Build()
        {
            // Ensure these folders exist

            // Load existing map data
            var map = Map.Open(BuilderConfig.MAP_PATH);

            // create a list of file that will be add to the result map
            var fileList = new List<MpqFile>();
            MapAssetManager.

            // enumerate the entire assets path and add every thing it found to the list of file
            AddFiles(ref fileList, BuilderConfig.ASSETS_PATH, "*", SearchOption.AllDirectories);

            // clear out memory in-case there is some leak happen
            GC.Collect(-1, GCCollectionMode.Optimized, false, true);

            // begin object generation
            ObjectEditing.Run(ref map);

            // clear out memory in-case there is some leak happen
            GC.Collect(-1, GCCollectionMode.Optimized, false, true);

            MapScriptCompiler.Run(ref map);

            // clear out memory in-case there is some leak happen
            GC.Collect(-1, GCCollectionMode.Optimized, false, true);

            // Build w3x file
            var archiveCreateOptions = new MpqArchiveCreateOptions
            {
                ListFileCreateMode = MpqFileCreateMode.Prune,
                AttributesCreateMode = MpqFileCreateMode.Prune,
                SignatureCreateMode = MpqFileCreateMode.Overwrite,
                WriteArchiveFirst = false,
                AttributesFlags = AttributesFlags.Crc32 | AttributesFlags.DateTime,
                BlockSize = 3,
#if DEBUG
                SignaturePrivateKey = Signature.GetSignature(true)
#else
                SignaturePrivateKey = Signature.GetSignature(false)
#endif
            };

            //mapArchive.Build(Path.Combine(OUT_ARTIFACTS, OUT_MAP_NAME), archiveCreateOptions);

            return Path.Combine(BuilderConfig.OUT_ARTIFACTS, BuilderConfig.OUT_MAP_NAME);
        }
    }
}
