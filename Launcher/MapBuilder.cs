using Launcher.MapObject;
using System;
using System.Collections.Generic;
using System.IO;
using War3Net.Build;
using War3Net.IO.Mpq;

namespace Launcher
{
    public static class MapBuilder
    {
        public static string Build()
        {
            // Ensure these folders exist
            Directory.CreateDirectory(BuilderConfig.ASSETS_PATH);
            Directory.CreateDirectory(BuilderConfig.OUT_ARTIFACTS);

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
