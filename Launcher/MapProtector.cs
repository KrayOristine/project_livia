using Jering.Javascript.NodeJS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Launcher
{
    public static class MapProtector
    {
        private static readonly SHA512 shaHash = SHA512.Create();
        private static readonly UTF8Encoding utf8Encoder = new(false, false);
        //private static readonly Encoding asciiEncoding = Encoding.ASCII

        // Caching for name confuser
        private static readonly Dictionary<string, byte[]> _confuseMap = new();

        public static Dictionary<string, byte[]> ConfuseMap { get => _confuseMap; }

        /// <summary>
        /// Confuse file name but still leaving it 'readable by machine'
        /// </summary>
        /// <param name="original"></param>
        /// <returns>An unsafe byte array (hash) that could exceed 0x200</returns>
        public static byte[] ConfuseName(string original)
        {
            byte[] bytes = utf8Encoder.GetBytes(original.ToUpperInvariant());
            var hashed = shaHash.ComputeHash(bytes);

            _confuseMap[original] = hashed;

            return hashed;
        }

        /// <summary>
        /// Normalize the given byte array from <see cref="ConfuseName(string)"/>.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static char[] NormalizeName(byte[] hash)
        {
            List<char> result = new();
            foreach (var b in hash)
                result.Add((char)b);

            return result.ToArray();
        }

        public static string ConfuseAndNormalize(string original)
        {
            var res = NormalizeName(ConfuseName(original));

            return res.ToString();
        }

        public static bool IsWhiteListed(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;

            return true;
        }

        public static bool IsWorldEditOnly(string name)
        {
            if (name == "war3mapUnits.doo") return false;
            if (name == "war3map.wtg") return false;
            if (name == "war3map.w3c") return false;
            if (name == "war3map.w3r") return false;
            if (name == "war3map.w3s") return false;
            if (name == "war3map.wct") return false;
            if (name == "war3map.imp") return false;
            if (name == "war3campaign.imp") return false;

            return true;
        }

        public static bool IsValidForProtect(string name)
        {
            if (name.ToLower().StartsWith("war3map")) return false;
            var extension = Path.GetExtension(name);
            if (extension == "j" || extension == "lua" || extension == "txt" || extension == "slk") return false;

            return true;
        }

        public static string MinifyScript(string script)
        {
            var result = StaticNodeJSService.InvokeFromFileAsync<string>(MapBuilder.BASE_PATH + @"Launcher\nodejs\minify.js", null, new object[] { script }).GetAwaiter().GetResult();

            return result;
        }
    }
}
