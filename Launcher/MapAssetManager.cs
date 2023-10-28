using System;
using System.Collections.Generic;
using System.IO;
using War3Net.IO.Mpq;

namespace Launcher
{
    internal static class MapAssetManager
    {

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
        public static MemoryStream ReadFile(string path)
        {
            var memoryStream = new MemoryStream();
            using (FileStream fileStream = new(path, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, FileOptions.SequentialScan))
            {
                fileStream.CopyTo(memoryStream);
            }
            return memoryStream;
        }
    }
}