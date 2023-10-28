using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibZopfliSharp;


namespace Launcher
{
    public static class MapCompression
    {
        // compression tweaks
        public const long SMALL_SIZE = 30000000; // 30mb

        public const long MEDIUM_SIZE = 70000000; // 70mb
        // big size is everything else

        private static readonly ZopfliOptions compress_small = new()
        {
            blocksplitting = 1,
            blocksplittingmax = 15,
            numiterations = 14,
            verbose = 0
        };

        private static readonly ZopfliOptions compress_medium = new()
        {
            blocksplitting = 1,
            blocksplittingmax = 15,
            numiterations = 8,
            verbose = 0
        };

        private static readonly ZopfliOptions compress_big = new()
        {
            blocksplitting = 1,
            blocksplittingmax = 15,
            numiterations = 5,
            verbose = 0
        };

        /// <summary>
        /// Compress a file memory stream using Zopfli. Auto tweaking based on file size is included
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] CompressStream(ref MemoryStream stream)
        {
            ZopfliOptions compressOption;
            if (stream.Length <= SMALL_SIZE) compressOption = compress_small;
            else if (stream.Length <= MEDIUM_SIZE) compressOption = compress_medium;
            else compressOption = compress_big;

            return Zopfli.compress(stream.ToArray(), ZopfliFormat.ZOPFLI_FORMAT_ZLIB, compressOption);
        }

        /// <summary>
        /// Compress a file memory stream using Zopfli. Auto tweaking based on file size is included<br/>
        /// This one return a memory stream instead of a byte array
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static MemoryStream CompressStream2(ref MemoryStream stream)
        {
            ZopfliOptions compressOption;
            if (stream.Length <= SMALL_SIZE) compressOption = compress_small;
            else if (stream.Length <= MEDIUM_SIZE) compressOption = compress_medium;
            else compressOption = compress_big;

            var compressed = Zopfli.compress(stream.ToArray(), ZopfliFormat.ZOPFLI_FORMAT_ZLIB, compressOption);

            return new(compressed);
        }
    }
}
