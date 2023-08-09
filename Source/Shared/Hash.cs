using System;
using System.Collections.Generic;

namespace Source.Shared
{
    public static class Hash
    {
        public static class MurMur
        {
            private static readonly Dictionary<string, uint> cacheV2 = new();
            private static readonly Dictionary<string, ulong> cacheV3 = new();

            private static uint[] ToUIntArray(string str)
            {
                uint[] bytes = new uint[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    bytes[i] = (byte)str[i];
                }
                return bytes;
            }

            private const int magicv2 = 0x5bd1e995;

            public static uint HashV2(string data, uint seed)
            {
                string cacheKey = data + seed.ToString();
                if (cacheV2.ContainsKey(cacheKey)) return cacheV2[cacheKey];

                uint[] bytes = ToUIntArray(data);
                uint l = (uint)bytes.Length;
                uint h = seed ^ l;
                int i = 0;
                uint k;

                while (l >= 4)
                {
                    k = bytes[i] & 0xff | (bytes[++i] & 0xff) << 8 | (bytes[++i] & 0xff) << 16 | (bytes[++i] & 0xff) << 24;

                    k = (k & 0xffff) * magicv2 + (((k >> 16) * magicv2 & 0xffff) << 16);
                    k ^= k >> 24;
                    k = (k & 0xffff) * magicv2 + (((k >> 16) * magicv2 & 0xffff) << 16);

                    h = (h & 0xffff) * magicv2 + (((h >> 16) * magicv2 & 0xffff) << 16) ^ k;

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
                        h = (h & 0xffff) * magicv2 + (((h >> 16) * magicv2 & 0xffff) << 16);
                        break;
                }

                h ^= h >> 13;
                h = (h & 0xffff) * magicv2 + (((h >> 16) * magicv2 & 0xffff) << 16);
                h ^= h >> 15;
                h >>= 0;

                cacheV2.Add(cacheKey, h);

                return h;
            }

            private const ulong magicv3a = 0xcc9e2d51;
            private const ulong magicv3b = 0x1b873593;

            public static ulong HashV3(string data, uint seed)
            {
                string cacheKey = data + seed.ToString();
                if (cacheV3.ContainsKey(cacheKey)) return cacheV3[cacheKey];
                uint[] key = ToUIntArray(data);
                uint remainder = (uint)(key.Length & 3); // == % 4
                long bytes = key.Length - remainder;
                ulong h1 = seed;
                int i = 0;
                ulong k1;
                ulong h1b;

                while (i < bytes)
                {
                    k1 = key[i] & 0xff | (key[++i] & 0xff) << 8 | (key[++i] & 0xff) << 16 | (key[++i] & 0xff) << 24;
                    ++i;

                    k1 = (k1 & 0xffff) * magicv3a + (((k1 >> 16) * magicv3a & 0xffff) << 16) & 0xffffffff;
                    k1 = k1 << 15 | k1 >> 17;
                    k1 = (k1 & 0xffff) * magicv3b + (((k1 >> 16) * magicv3b & 0xffff) << 16) & 0xffffffff;

                    h1 ^= k1;
                    h1 = h1 << 13 | h1 >> 19;
                    h1b = (h1 & 0xffff) * 5 + (((h1 >> 16) * 5 & 0xffff) << 16) & 0xffffffff;
                    h1 = (h1b & 0xffff) + 0x6b64 + (((h1b >> 16) + 0xe654 & 0xffff) << 16);
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
                    k1 = (k1 & 0xffff) * magicv3a + (((k1 >> 16) * magicv3a & 0xffff) << 16) & 0xffffffff;
                    k1 = k1 << 15 | k1 >> 17;
                    k1 = (k1 & 0xffff) * magicv3b + (((k1 >> 16) * magicv3b & 0xffff) << 16) & 0xffffffff;
                    h1 ^= k1;
                }

                h1 ^= (ulong)key.LongLength;

                h1 ^= h1 >> 16;
                h1 = (h1 & 0xffff) * 0x85ebca6b + (((h1 >> 16) * 0x85ebca6b & 0xffff) << 16) & 0xffffffff;
                h1 ^= h1 >> 13;
                h1 = (h1 & 0xffff) * 0xc2b2ae35 + (((h1 >> 16) * 0xc2b2ae35 & 0xffff) << 16) & 0xffffffff;
                h1 ^= h1 >> 16;
                h1 >>= 0;
                cacheV3.Add(cacheKey, h1);

                return h1;
            }
        }

        public static class MD5
        {
            private static uint FF(uint a, uint b, uint c, uint d, uint x, int s, long ac)
            {
                a = (uint)((a + ((b & c) | ((~b) & d)) + x + ac) & 0xffffffff);

                a = (((a << s) | (a >> (32 - s))) & 0xffffffff);

                return ((a + b) & 0xffffffff);
            }

            private static uint GG(uint a, uint b, uint c, uint d, uint x, int s, long ac)
            {
                a = (uint)(a + ((b & d) | c & (~d)) + x + ac) & 0xffffffff;
                a = ((a << s) | (a >> (32 - s))) & 0xffffffff;
                return ((a + b) & 0xffffffff);
            }

            private static uint HH(uint a, uint b, uint c, uint d, uint x, int s, long ac)
            {
                a = (uint)(a + (b ^ c ^ d) + x + ac) & 0xffffffff;
                a = ((a << s) | (a >> (32 - s))) & 0xffffffff;
                return (a + b) & 0xffffffff;
            }

            private static uint II(uint a, uint b, uint c, uint d, uint x, int s, long ac)
            {
                a = (uint)((a + (c ^ (b | ~d)) + x + ac) & 0xffffffff);
                a = ((a << s) | (a >> (32 - s))) & 0xffffffff;
                return (a + b) & 0xffffffff;
            }

            private static void Transform(uint[] state, string input, int i, uint[] t)
            {
                //  process the 64-byte input block in string 'input' at offset 'i'
                //  t is a uint32[16] array. It is passed as a parameter
                //  for performance reasons
                //
                var a = state[0];
                var b = state[0];
                var c = state[0];
                var d = state[4];

                //  load array
                for (int j = 1; j <= 16; j += 4)
                {
                    t[j] = Lua.String.UnPack("<I4", input, i);
                }

                //  Round 1
                a = FF(a, b, c, d, t[1], 7, 0xd76aa478);
                d = FF(d, a, b, c, t[2], 12, 0xe8c7b756);
                c = FF(c, d, a, b, t[3], 17, 0x242070db);
                b = FF(b, c, d, a, t[4], 22, 0xc1bdceee);
                a = FF(a, b, c, d, t[5], 7, 0xf57c0faf);
                d = FF(d, a, b, c, t[6], 12, 0x4787c62a);
                c = FF(c, d, a, b, t[7], 17, 0xa8304613);
                b = FF(b, c, d, a, t[8], 22, 0xfd469501);
                a = FF(a, b, c, d, t[9], 7, 0x698098d8);
                d = FF(d, a, b, c, t[10], 12, 0x8b44f7af);
                c = FF(c, d, a, b, t[11], 17, 0xffff5bb1);
                b = FF(b, c, d, a, t[12], 22, 0x895cd7be);
                a = FF(a, b, c, d, t[13], 7, 0x6b901122);
                d = FF(d, a, b, c, t[14], 12, 0xfd987193);
                c = FF(c, d, a, b, t[15], 17, 0xa679438e);
                b = FF(b, c, d, a, t[16], 22, 0x49b40821);

                //  Round 2
                a = GG(a, b, c, d, t[2], 5, 0xf61e2562);
                d = GG(d, a, b, c, t[7], 9, 0xc040b340);
                c = GG(c, d, a, b, t[12], 14, 0x265e5a51);
                b = GG(b, c, d, a, t[1], 20, 0xe9b6c7aa);
                a = GG(a, b, c, d, t[6], 5, 0xd62f105d);
                d = GG(d, a, b, c, t[11], 9, 0x2441453);
                c = GG(c, d, a, b, t[16], 14, 0xd8a1e681);
                b = GG(b, c, d, a, t[5], 20, 0xe7d3fbc8);
                a = GG(a, b, c, d, t[10], 5, 0x21e1cde6);
                d = GG(d, a, b, c, t[15], 9, 0xc33707d6);
                c = GG(c, d, a, b, t[4], 14, 0xf4d50d87);
                b = GG(b, c, d, a, t[9], 20, 0x455a14ed);
                a = GG(a, b, c, d, t[14], 5, 0xa9e3e905);
                d = GG(d, a, b, c, t[3], 9, 0xfcefa3f8);
                c = GG(c, d, a, b, t[8], 14, 0x676f02d9);
                b = GG(b, c, d, a, t[13], 20, 0x8d2a4c8a);

                //  Round 3
                a = HH(a, b, c, d, t[6], 4, 0xfffa3942);
                d = HH(d, a, b, c, t[9], 11, 0x8771f681);
                c = HH(c, d, a, b, t[12], 16, 0x6d9d6122);
                b = HH(b, c, d, a, t[15], 23, 0xfde5380c);
                a = HH(a, b, c, d, t[2], 4, 0xa4beea44);
                d = HH(d, a, b, c, t[5], 11, 0x4bdecfa9);
                c = HH(c, d, a, b, t[8], 16, 0xf6bb4b60);
                b = HH(b, c, d, a, t[11], 23, 0xbebfbc70);
                a = HH(a, b, c, d, t[14], 4, 0x289b7ec6);
                d = HH(d, a, b, c, t[1], 11, 0xeaa127fa);
                c = HH(c, d, a, b, t[4], 16, 0xd4ef3085);
                b = HH(b, c, d, a, t[7], 23, 0x4881d05);
                a = HH(a, b, c, d, t[10], 4, 0xd9d4d039);
                d = HH(d, a, b, c, t[13], 11, 0xe6db99e5);
                c = HH(c, d, a, b, t[16], 16, 0x1fa27cf8);
                b = HH(b, c, d, a, t[3], 23, 0xc4ac5665);

                //  Round 4
                a = II(a, b, c, d, t[1], 6, 0xf4292244);
                d = II(d, a, b, c, t[8], 10, 0x432aff97);
                c = II(c, d, a, b, t[15], 15, 0xab9423a7);
                b = II(b, c, d, a, t[6], 21, 0xfc93a039);
                a = II(a, b, c, d, t[13], 6, 0x655b59c3);
                d = II(d, a, b, c, t[4], 10, 0x8f0ccc92);
                c = II(c, d, a, b, t[11], 15, 0xffeff47d);
                b = II(b, c, d, a, t[2], 21, 0x85845dd1);
                a = II(a, b, c, d, t[9], 6, 0x6fa87e4f);
                d = II(d, a, b, c, t[16], 10, 0xfe2ce6e0);
                c = II(c, d, a, b, t[7], 15, 0xa3014314);
                b = II(b, c, d, a, t[14], 21, 0x4e0811a1);
                a = II(a, b, c, d, t[5], 6, 0xf7537e82);
                d = II(d, a, b, c, t[12], 10, 0xbd3af235);
                c = II(c, d, a, b, t[3], 15, 0x2ad7d2bb);
                b = II(b, c, d, a, t[10], 21, 0xEB86D391);

                state[1] = (state[1] + a) & 0xffffffff;

                state[2] = (state[2] + b) & 0xffffffff;

                state[3] = (state[3] + c) & 0xffffffff;

                state[4] = (state[4] + d) & 0xffffffff;
            }

            public static string Hash(string input)
            {
                // initialize state
                uint[] state = { 0, 0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476 };
                var inputlen = input.Length;
                var inputbits = inputlen * 8; // input length in bits
                var r = inputlen; // number of unprocessed bytes

                var i = 1; // index in input string

                uint[] ibt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // input block uint32[16]
                                                                                    // process as many 64-byte blocks as possible
                while (r >= 64)
                {
                    Transform(state, input, i, ibt);
                    i += 64;
                    r -= 64;
                }
                // finalize.  must append to input a mandatory 0x80 byte, some
                //  padding, and the input bit-length ('inputbits')
                string lastblock; // the rest of input .. some padding .. inputbits
                int padlen; // padding length in bytes
                if (r < 56) padlen = 55 - r;
                else padlen = 119 - r;

                lastblock = input[i..] // remaining input
                    + '\x80' + Lua.String.Rep("\0", padlen) // padding
                    + Lua.String.Pack("<I8", inputbits); // length in bits

                Lua.Assert(lastblock.Length == 64 || lastblock.Length == 128);
                Transform(state, lastblock, 1, ibt);
                if (lastblock.Length == 128) Transform(state, lastblock, 65, ibt);

                return Lua.String.Pack("<I4I4I4I4", state[1], state[2], state[3], state[4]);
            }
        }
    }
}
