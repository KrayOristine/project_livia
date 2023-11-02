using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    public static class Signature
    {
        private const string SIGNATURE_PATH = BuilderConfig.RESOURCES_PATH + @"Signature\";

        /// <summary>
        /// Get a random signature to build
        /// </summary>
        /// <param name="isDebugBuild">If is true then return the same signature for testing</param>
        /// <returns></returns>
        public static string GetSignature(bool isDebugBuild)
        {
            if (isDebugBuild) return File.ReadAllText(Path.Join(SIGNATURE_PATH, "priv", "debug.pem"));


            var rnd = new JavaRandom(DateTime.Now.Ticks);
            int r = rnd.NextInt(11);
            var result = new StringBuilder();
            using (var f = File.OpenRead(Path.Join(SIGNATURE_PATH, "priv", $"key{r}.pem")))
            {
                result.Append(f.ReadByte());
            }

            return result.ToString();
        }
    }
}
