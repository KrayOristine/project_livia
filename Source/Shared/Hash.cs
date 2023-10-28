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
using System.Text;

namespace Source.Shared
{
    /// <summary>
    /// A collection of super in-efficient hashing algorithm that is created to run in warcraft lua environment
    /// </summary>
    public static class Hash
    {
        /*
        /// <summary>
        /// Extreme performance hogging bit mask with built-in binary expression to simulate uint in lua environment which only 'int' exists
        /// </summary>
        private sealed class BitMask32 : ICloneable, IEquatable<BitMask32>, IEquatable<int>
        {
            private readonly string _valueBit;
            private readonly int _signBit;

            public BitMask32(int int32Value)
            {
                if (int32Value < 0)
                {
                    _signBit = 1;
                    _valueBit = Convert.ToString(int32Value, 2)[1..];
                }
                else
                {
                    _valueBit = Convert.ToString(int32Value, 2);
                    _signBit = 0;
                }
            }

            /// <summary>
            /// Use this to convert, just in-case the number is too large to uses
            /// </summary>
            /// <param name="valueBit"></param>
            /// <param name="signBit"></param>
            public BitMask32(string valueBit, int signBit)
            {
                _valueBit = valueBit;
                _signBit = signBit >= 1 ? 1 : 0;
            }

            public object Clone()
            {
                return new BitMask32(_valueBit, _signBit);

            }

            public static BitMask32 operator +(BitMask32 lhs) => lhs;

            public static BitMask32 operator -(BitMask32 lhs)
            {
                return new(lhs._valueBit, lhs._signBit == 0 ? 1 : 0);
            }

            public static BitMask32 operator +(BitMask32 lhs, BitMask32 rhs)
            {
                var l = Convert.ToInt32(lhs._valueBit, 2);
                var r = Convert.ToInt32(rhs._valueBit, 2);
                var e = l + r;
                return new(Convert.ToString(e, 2), e >= 0 ? 0 : 1);
            }

            public static BitMask32 operator -(BitMask32 lhs, BitMask32 rhs)
            {
                var l = Convert.ToInt32(lhs._valueBit, 2);
                var r = Convert.ToInt32(rhs._valueBit, 2);
                var e = l - r;
                return new(Convert.ToString(e, 2), e >= 0 ? 0 : 1);
            }

            public static BitMask32 operator *(BitMask32 lhs, BitMask32 rhs)
            {
                var l = Convert.ToInt32(lhs._valueBit, 2);
                var r = Convert.ToInt32(rhs._valueBit, 2);
                var e = l * r;
                return new(Convert.ToString(e, 2), e >= 0 ? 0 : 1);
            }

            public static BitMask32 operator /(BitMask32 lhs, BitMask32 rhs)
            {
                var l = Convert.ToInt32(lhs._valueBit, 2);
                var r = Convert.ToInt32(rhs._valueBit, 2);
                var e = l / r;
                return new(Convert.ToString(e, 2), e >= 0 ? 0 : 1);
            }

            public static bool operator ==(BitMask32? lhs, BitMask32? rhs)
            {
                return lhs switch
                {
                    null => false,
                    _ => lhs.Equals(rhs),
                };
            }

            public static bool operator !=(BitMask32? lhs, BitMask32? rhs)
            {
                return lhs switch
                {
                    null => true,
                    _ => !lhs.Equals(rhs),
                };
            }

            public bool Equals(BitMask32? other)
            {
                return other switch
                {
                    null => false,
                    _ => (_valueBit == other._valueBit && _signBit == other._signBit)
                };
            }

            public bool Equals(int other)
            {
                if (other > 0)
                    return _signBit == 0 && Convert.ToInt32(_valueBit, 2) == other;
                else
                    return _signBit == 1 && -Convert.ToInt32(_valueBit, 2) == other;
            }

            public override bool Equals(object? obj)
            {
                return Equals(obj as BitMask32);
            }

            public override int GetHashCode()
            {
                var h = 0;
                var v = Convert.ToInt32(_valueBit, 2);
                h ^= v;
                h <<= (v ^ 414001 & v) / 256;
                h >>= 3 - _signBit;
                h ^= _signBit + 37;

                return h;
            }
        }
        */
        public static class MurMur
        {
            private static readonly Dictionary<string, int> hashCache = new();

            private static int[] ToUIntArray(string str)
            {
                int[] bytes = new int[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    bytes[i] = (byte)str[i];
                }
                return bytes;
            }

            private const int magicNumber = 0x5bd1e995;

            public static int Hash(string data, int seed)
            {
                string cacheKey = data + seed.ToString();
                if (hashCache.ContainsKey(cacheKey)) return hashCache[cacheKey];

                int[] bytes = ToUIntArray(data);
                int l = bytes.Length;
                int h = seed ^ l;
                int i = 0;
                int k;

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
            public static int ComputeHash(byte[] data)
            {
                return PrivateCompute(data);
            }

            /// <summary>
            /// Compute Jenkin hash for the data byte array
            /// </summary>
            /// <param name="data">The source of data</param>
            /// <param name="offset">The offset of the data for hashing</param>
            /// <returns>hash</returns>
            public static int ComputeHash(byte[] data, int offset)
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
            public static int ComputeHash(string str)
            {
                List<byte> dataArray = new();
                foreach (char c in str) dataArray.Add((byte)c);

                return PrivateCompute(dataArray.ToArray());
            }

            private static int PrivateCompute(byte[] data)
            {
                int hash = 0;
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
        /*
        public static class xxHash
        {
            private const uint PRIME32_1 = 0x9E3779B1U;  // 0b10011110001101110111100110110001
            private const uint PRIME32_2 = 0x85EBCA77U;  // 0b10000101111010111100101001110111
            private const uint PRIME32_3 = 0xC2B2AE3DU;  // 0b11000010101100101010111000111101
            private const uint PRIME32_4 = 0x27D4EB2FU;  // 0b00100111110101001110101100101111
            private const uint PRIME32_5 = 0x165667B1U;  // 0b00010110010101100110011110110001
        }
        */
    }
}
