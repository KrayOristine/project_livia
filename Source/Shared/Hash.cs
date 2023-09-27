// ------------------------------------------------------------------------------
// <copyright file="Hash.cs" company="Kray Oristine">
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
// </copyright>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Source.Shared
{
    public static class Hash
    {
        public static class MurMur
        {
            private static readonly Dictionary<string, uint> hashCache = new();

            private static uint[] ToUIntArray(string str)
            {
                uint[] bytes = new uint[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    bytes[i] = (byte)str[i];
                }
                return bytes;
            }

            private const int magicNumber = 0x5bd1e995;

            public static uint Hash(string data, uint seed)
            {
                string cacheKey = data + seed.ToString();
                if (hashCache.ContainsKey(cacheKey)) return hashCache[cacheKey];

                uint[] bytes = ToUIntArray(data);
                uint l = (uint)bytes.Length;
                uint h = seed ^ l;
                int i = 0;
                uint k;

                while (l >= 4)
                {
                    k = bytes[i] & 0xff | (bytes[++i] & 0xff) << 8 | (bytes[++i] & 0xff) << 16 | (bytes[++i] & 0xff) << 24;

                    k = (k & 0xffff) * magicNumber + (((k >> 16) * magicNumber & 0xffff) << 16);
                    k ^= k >> 24;
                    k = (k & 0xffff) * magicNumber + (((k >> 16) * magicNumber & 0xffff) << 16);

                    h = (h & 0xffff) * magicNumber + (((h >> 16) * magicNumber & 0xffff) << 16) ^ k;

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
                        h = (h & 0xffff) * magicNumber + (((h >> 16) * magicNumber & 0xffff) << 16);
                        break;
                }

                h ^= h >> 13;
                h = (h & 0xffff) * magicNumber + (((h >> 16) * magicNumber & 0xffff) << 16);
                h ^= h >> 15;
                h >>= 0;

                hashCache.Add(cacheKey, h);

                return h;
            }
        }

        public static class Jenkin
        {

            /// <summary>
            /// Compute Jenkin hash for the data byte array
            /// </summary>
            /// <param name="data">The source of data</param>
            /// <returns>hash</returns>
            public static uint ComputeHash(byte[] data)
            {
                return PrivateCompute(data);
            }

            /// <summary>
            /// Compute Jenkin hash for the data byte array
            /// </summary>
            /// <param name="data">The source of data</param>
            /// <param name="offset">The offset of the data for hashing</param>
            /// <returns>hash</returns>
            public static uint ComputeHash(byte[] data, int offset)
            {
                var newLen = data.Length - offset;
                byte[] newArray = new byte[newLen];
                Array.Copy(data, offset, newArray, 0, newLen);

                return PrivateCompute(newArray);
            }

            /// <summary>
            /// Compute Jenkin hash for the string
            /// </summary>
            /// <param name="str">The source of data</param>
            /// <returns>hash</returns>
            public static uint ComputeHash(string str)
            {
                List<byte> dataArray = new();
                foreach (char c in str) dataArray.Add((byte)c);

                return PrivateCompute(dataArray.ToArray());
            }

            private static uint PrivateCompute(byte[] data)
            {
                uint hash = 0;
                foreach (byte b in data)
                {
                    hash += b;
                    hash += hash << 10;
                    hash ^= hash >> 6;
                }
                hash += hash << 3;
                hash ^= hash >> 11;
                hash += hash << 15;
                return hash;
            }
        }
    }
}
