using System;
using System.Collections.Generic;
using System.Linq;

namespace Source.Utils
{
    public static class Hash
    {
        internal static readonly Dictionary<string, long> cacheTable2 = new();
        internal static readonly Dictionary<string, long> cacheTable3 = new();

        public static long MurMur2(string data, uint seed)
        {
            if (cacheTable2.ContainsKey(data + seed.ToString())) return cacheTable2.GetValueOrDefault(data + seed.ToString());

            uint[] bytes = Encoder.Encode(data.ToArray());
            uint l = (uint)bytes.Length;
            long h = seed ^ l;
            int i = 0;
            long k;

            while (l >= 4)
            {
                k = (bytes[i] & 0xff) | ((bytes[++i] & 0xff) << 8) | ((bytes[++i] & 0xff) << 16) | ((bytes[++i] & 0xff) << 24);

                k = (k & 0xffff) * 0x5bd1e995 + ((((k >> 16) * 0x5bd1e995) & 0xffff) << 16);
                k ^= k >> 24;
                k = (k & 0xffff) * 0x5bd1e995 + ((((k >> 16) * 0x5bd1e995) & 0xffff) << 16);

                h = ((h & 0xffff) * 0x5bd1e995 + ((((h >> 16) * 0x5bd1e995) & 0xffff) << 16)) ^ k;

                l -= 4;
                ++i;
            }

            switch (l)
            {
                case 3:
                    h ^= (bytes[i + 2] & 0xff) << 16;
                    break;
                case 2:
                    h ^= (bytes[i + 1] & 0xff) << 8;
                    break;
                case 1:
                    h ^= bytes[i] & 0xff;
                    h = (h & 0xffff) * 0x5bd1e995 + ((((h >> 16) * 0x5bd1e995) & 0xffff) << 16);
                    break;
            }

            h ^= h >> 13;
            h = (h & 0xffff) * 0x5bd1e995 + ((((h >> 16) * 0x5bd1e995) & 0xffff) << 16);
            h ^= h >> 15;
            h >>= 0;

            cacheTable2.Add(data + seed.ToString(), h);

            return h;
        }

        public static long MurMur3(string data, uint seed)
        {
            if (cacheTable3.ContainsKey(data + seed.ToString())) return cacheTable3.GetValueOrDefault(data + seed.ToString());
            uint[] key = Encoder.Encode(data.ToArray());

            int remainder = key.Length & 3; // == % 4
            int bytes = key.Length - remainder;
            long h1 = seed;
            long c1 = 0xcc9e2d51;
            long c2 = 0x1b873593;
            int i = 0;
            long k1;
            long h1b;

            while (i < bytes)
            {
                k1 = (key[i] & 0xff) | ((key[++i] & 0xff) << 8) | ((key[++i] & 0xff) << 16) | ((key[++i] & 0xff) << 24);
                ++i;

                k1 = ((k1 & 0xffff) * c1 + ((((k1 >> 16) * c1) & 0xffff) << 16)) & 0xffffffff;
                k1 = (k1 << 15) | (k1 >> 17);
                k1 = ((k1 & 0xffff) * c2 + ((((k1 >> 16) * c2) & 0xffff) << 16)) & 0xffffffff;

                h1 ^= k1;
                h1 = (h1 << 13) | (h1 >> 19);
                h1b = ((h1 & 0xffff) * 5 + ((((h1 >> 16) * 5) & 0xffff) << 16)) & 0xffffffff;
                h1 = (h1b & 0xffff) + 0x6b64 + ((((h1b >> 16) + 0xe654) & 0xffff) << 16);
            }

            k1 = 0;
            if (remainder > 0)
            {
                switch (remainder)
                {
                    case 3:
                        k1 ^= (key[i + 2] & 0xff) << 16;
                        break;
                    case 2:
                        k1 ^= (key[i + 1] & 0xff) << 8;
                        break;
                    case 1:
                        k1 ^= key[i] & 0xff;
                        break;
                }
                k1 = ((k1 & 0xffff) * c1 + ((((k1 >> 16) * c1) & 0xffff) << 16)) & 0xffffffff;
                k1 = (k1 << 15) | (k1 >> 17);
                k1 = ((k1 & 0xffff) * c2 + ((((k1 >> 16) * c2) & 0xffff) << 16)) & 0xffffffff;
                h1 ^= k1;
            }

            h1 ^= key.Length;

            h1 ^= h1 >> 16;
            h1 = ((h1 & 0xffff) * 0x85ebca6b + ((((h1 >> 16) * 0x85ebca6b) & 0xffff) << 16)) & 0xffffffff;
            h1 ^= h1 >> 13;
            h1 = ((h1 & 0xffff) * 0xc2b2ae35 + ((((h1 >> 16) * 0xc2b2ae35) & 0xffff) << 16)) & 0xffffffff;
            h1 ^= h1 >> 16;
            h1 >>= 0;
            cacheTable2.Add(data + seed.ToString(), h1);

            return h1;
        }
    }
}
