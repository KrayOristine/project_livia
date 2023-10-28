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

        private const string debugPem = @"
-----BEGIN PRIVATE KEY-----
MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCkySXqpbrZ+381
Fs7TRRBpuyDO0dbpK0HZT+4mzzkSAVuyo1A7i3fIEPW7v7Ay7QT0z/G6OEQW3J2R
k2MhtxQN6XUFGWOGNWmxbSTGAo9J+lT6cYsB14K92bekosxPV/Et3RNC4F4dANVh
C909ZCwir4pMU5OEOsVaNfZQ4e9fTnWXSRsgDXevObEu44Sv7rjvSC6Tk/zBizCv
OZ49dp+9+9T23foft7vK/r4wgwvOL9ztgorDdk7phfb06+p01ETMZsiz/XEsdfbD
CAhsmZTdxqS8Tye7FuCzSSap0d5yapPjRFVJn2x289pTiMhRPH3gpNNTtqK3hfyL
Y4VnpTrzAgMBAAECggEAQE463Ln2E/DgMHCJH8s433nmEK+0xWNbHBnkHwMJC2y6
QxkWS1Ze1RVBQBZLBWSXZY3skDY65A0N9Gxm6BZQ3ij3wJiqzQn1CAdGrXr8PetR
1DYRABQA8mOPFdFuF5yDAtneFWAHehwEKQ3/NWDtFFAakTSgQZ7Juy2JxQCWcENG
QmRz932bYlI0eaf+Pw1dGY2QOOHMQ4BF7sP7aUFhsLbFIN4cKvfxX6gAvG/6xfnz
9ndQOjs63z+fYVFNFotTFtAtckXk7pFU6SnJoKDWRFFNNdi7vjbik58gPkkNRsxW
pgOT2gi8mX1DrGtUsYcgd4TI2yjty1AYWn2Kj4ZZmQKBgQDdSB4mz9fcDm0G/3zl
b4MawXFPI4joZL+8As459qGZdj0NFAE9rV/owRwqhY1Y93N7uYSssDIIUbL9m63c
X3cSCokywULyx86//1toS3mTf5HOjkD/n2p5HivNo2f+NRpJTnxgyblwc7dzQO3S
31GT6k5LNzEN2fNArFIj9kH9XwKBgQC+o9fmT2mzNj1RHAwayrWWrr0Uceo3SymD
ZcLnyGF2VCcasM+c6kJR6BF6jINLdVLOpVHQAY4JaJVB6PnYJv9FLIRdst9q8Iby
l8OzO+BiXwWBUMefNHYdGsxUefvF1dEAp8whaEeIiaDovhKdT3IdskJO5WZuecG+
POFsXOiW7QKBgAyPc2awlYggKYAkMGq1kbOLF2tukO7gCSIa5qnTngif9Ycwg4GE
vdTwYTDos8VBBoUXWriSp8RhHs/Wbl/76z5S6sa+wZYWUV4EmKOq5RPCQfx/hqD7
44dMdyANDnIXgY53rgewa54+c0NPhDBcDL1gp2ENmPM4U95v6Zmw+L3xAoGBAKbK
Pkg3zmiG2dquyUBffcQZmT/b8mjyD53wWDflrq+SLKxDaUEGC4QRn8kySmSofwxg
w1VTJuvAXmXN6VldtRTWP/CiYKn1SnvQn92eU1B9hhl/jUcS5QvNuFtoZgjOaw9B
v2yzCXLBxtAaC7sqGNM2dKEGPzoEuKcyKJ9gIfZ5AoGBAMLu0e21ES3opmGuI68n
k0RI4+xSO2TuY/cfVRtq9yZxOFU+0pfcqOS7i0PZjqn6f17Edlc5DxJU/VcCNadF
Ge2iEJ54vWFGoA/5esGvXt57CgyW0YhtB+HFN8ezM3GeQvlEsLeKatNK1aeJGxis
xfYC4MNWKH8K2EkUZ6zt5Zld
-----END PRIVATE KEY-----";

        /// <summary>
        /// Get a random signature to build
        /// </summary>
        /// <param name="isDebugBuild">If is true then return the same signature for testing</param>
        /// <returns></returns>
        public static string GetSignature(bool isDebugBuild)
        {
            if (isDebugBuild) return debugPem;

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
