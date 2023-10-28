// ------------------------------------------------------------------------------
// <copyright file="Skip32.cs" company="Kray Oristine">
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Shared
{
    public static class Skip32
    {
        private static readonly byte[] ftable = new byte[256]
        {
             0xa3,0xd7,0x09,0x83,0xf8,0x48,0xf6,0xf4,0xb3,0x21,0x15,0x78,0x99,0xb1,0xaf,0xf9,
             0xe7,0x2d,0x4d,0x8a,0xce,0x4c,0xca,0x2e,0x52,0x95,0xd9,0x1e,0x4e,0x38,0x44,0x28,
             0x0a,0xdf,0x02,0xa0,0x17,0xf1,0x60,0x68,0x12,0xb7,0x7a,0xc3,0xe9,0xfa,0x3d,0x53,
             0x96,0x84,0x6b,0xba,0xf2,0x63,0x9a,0x19,0x7c,0xae,0xe5,0xf5,0xf7,0x16,0x6a,0xa2,
             0x39,0xb6,0x7b,0x0f,0xc1,0x93,0x81,0x1b,0xee,0xb4,0x1a,0xea,0xd0,0x91,0x2f,0xb8,
             0x55,0xb9,0xda,0x85,0x3f,0x41,0xbf,0xe0,0x5a,0x58,0x80,0x5f,0x66,0x0b,0xd8,0x90,
             0x35,0xd5,0xc0,0xa7,0x33,0x06,0x65,0x69,0x45,0x00,0x94,0x56,0x6d,0x98,0x9b,0x76,
             0x97,0xfc,0xb2,0xc2,0xb0,0xfe,0xdb,0x20,0xe1,0xeb,0xd6,0xe4,0xdd,0x47,0x4a,0x1d,
             0x42,0xed,0x9e,0x6e,0x49,0x3c,0xcd,0x43,0x27,0xd2,0x07,0xd4,0xde,0xc7,0x67,0x18,
             0x89,0xcb,0x30,0x1f,0x8d,0xc6,0x8f,0xaa,0xc8,0x74,0xdc,0xc9,0x5d,0x5c,0x31,0xa4,
             0x70,0x88,0x61,0x2c,0x9f,0x0d,0x2b,0x87,0x50,0x82,0x54,0x64,0x26,0x7d,0x03,0x40,
             0x34,0x4b,0x1c,0x73,0xd1,0xc4,0xfd,0x3b,0xcc,0xfb,0x7f,0xab,0xe6,0x3e,0x5b,0xa5,
             0xad,0x04,0x23,0x9c,0x14,0x51,0x22,0xf0,0x29,0x79,0x71,0x7e,0xff,0x8c,0x0e,0xe2,
             0x0c,0xef,0xbc,0x72,0x75,0x6f,0x37,0xa1,0xec,0xd3,0x8e,0x62,0x8b,0x86,0x10,0xe8,
             0x08,0x77,0x11,0xbe,0x92,0x4f,0x24,0xc5,0x32,0x36,0x9d,0xcf,0xf3,0xa6,0xbb,0xac,
             0x5e,0x6c,0xa9,0x13,0x57,0x25,0xb5,0xe3,0xbd,0xa8,0x3a,0x01,0x05,0x59,0x2a,0x46
        };

        private static short g(byte[] key, int k, short w)
        {
            byte g1, g2, g3, g4, g5, g6;
            g1 = (byte)((w >> 8) & 0xff);
            g2 = (byte)(w & 0xff);

            g3 = (byte)(ftable[g2 ^ key[(4 * k) % 10]] ^ g1);
            g4 = (byte)(ftable[g3 ^ key[(4 * k + 1) % 10]] ^ g2);
            g5 = (byte)(ftable[g4 ^ key[(4 * k + 2) % 10]] ^ g3);
            g6 = (byte)(ftable[g5 ^ key[(4 * k + 3) % 10]] ^ g4);

            return (short)((g5 << 8) + g6);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key">10 index array</param>
        /// <param name="buffer">4 index array</param>
        private static void PrivateEncrypt(byte[] key, byte[] buffer)
        {
            int k = 0; // round number
            int i; // round counter
            int kstep = 1;
            short wl, wr;

            wl = (short)((buffer[0] << 8) + buffer[1]);
            wr = (short)((buffer[2] << 8) + buffer[3]);

            for (i = 0; i < 12; i++)
            {
                wr = (short)(wr ^ g(key, k, wl) ^ k);
                k += kstep;
                wl = (short)(wl ^ g(key, k, wr) ^ k);
                k += kstep;
            }

            buffer[0] = (byte)(wr >> 8); buffer[1] = (byte)(wr & 0xff);
            buffer[2] = (byte)(wl >> 8); buffer[3] = (byte)(wl & 0xff);
        }

        private static void PrivateDecrypt(byte[] key, byte[] buffer)
        {
            int k = 23; // round number
            int i; // round counter
            int kstep = -1;
            short wl, wr;

            wl = (short)((buffer[0] << 8) + buffer[1]);
            wr = (short)((buffer[2] << 8) + buffer[3]);

            for (i = 0; i < 12; i++)
            {
                wr = (short)(wr ^ g(key, k, wl) ^ k);
                k += kstep;
                wl = (short)(wl ^ g(key, k, wr) ^ k);
                k += kstep;
            }

            buffer[0] = (byte)(wr >> 8); buffer[1] = (byte)(wr & 0xff);
            buffer[2] = (byte)(wl >> 8); buffer[3] = (byte)(wl & 0xff);
        }

        /// <summary>
        /// Encrypt a <paramref name="buffer"/> (assumed the length is divisible by 4) with the given <paramref name="key"/> (assume the length is divisible by 10)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If <paramref name="key"/> or <paramref name="buffer"/> is not divisible by 10/4 respectively</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="buffer"/> is longer than the key used to encrypt</exception>
        public static byte[] Encrypt(string key, string buffer)
        {
            byte[] keyBytes = Utility.GetBytes(key);
            byte[] bufferBytes = Utility.GetBytes(buffer);

            var keyLength = keyBytes.Length;
            var bufferLength = bufferBytes.Length;

            if (keyLength % 10 > 0)
                throw new ArgumentException("Key is not divisible by 10", nameof(key));
            if (bufferLength % 4 > 0)
                throw new ArgumentException("Buffer is not divisible by 4", nameof(buffer));
            if (bufferLength / 4 > keyLength / 10)
                throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer is longer way longer than key");

            var encryptKey = new byte[10];
            var encryptBuffer = new byte[4];
            var resultBuffer = new List<byte>();
            int count = 0;
            int max = bufferLength / 4;
            while (true)
            {
                Array.Copy(keyBytes, count * 10, encryptKey, 0, 10);
                Array.Copy(bufferBytes, count * 4, encryptBuffer, 0, 4);
                PrivateEncrypt(encryptKey, encryptBuffer);
                resultBuffer.Add(encryptBuffer[0]);
                resultBuffer.Add(encryptBuffer[1]);
                resultBuffer.Add(encryptBuffer[2]);
                resultBuffer.Add(encryptBuffer[3]);
                count++;
                if (count >= max) break;
            }

            return resultBuffer.ToArray();
        }

        /// <summary>
        /// As same as <see cref="Encrypt(string, string)"/>. But this one will pad the value if they are not enough<br/>
        /// </summary>
        /// <remarks>This will pad <paramref name="buffer"/> with null byte! (or 0x00)</remarks>
        /// <param name="key"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="buffer"/> is longer than the key used to encrypt</exception>
        public static byte[] EncryptPad(string key, string buffer)
        {
            byte[] keyBytes = Utility.GetBytes(key);
            byte[] bufferBytes = Utility.GetBytes(buffer);

            var keyLength = keyBytes.Length;
            var bufferLength = bufferBytes.Length;

            if (keyLength % 10 > 0)
            {
                var newKey = new byte[keyLength + (keyLength % 10)];
                Array.Copy(keyBytes, newKey, keyLength);
                for (int i = keyLength; i < newKey.Length; i++)
                    newKey[i] = 0;
                keyBytes = newKey;
            }

            if (bufferLength % 4 > 0)
            {
                var newBuffer = new byte[bufferLength + (bufferLength % 4)];
                Array.Copy(bufferBytes, newBuffer, bufferLength);
                for (int i = bufferLength; i < newBuffer.Length; i++)
                    newBuffer[i] = 0;
                bufferBytes = newBuffer;
            }

            if (bufferLength / 4 > keyLength / 10)
                throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer is longer way longer than key");

            var encryptKey = new byte[10];
            var encryptBuffer = new byte[4];
            var resultBuffer = new List<byte>();
            int count = 0;
            int max = bufferLength / 4;
            while (true)
            {
                Array.Copy(keyBytes, count * 10, encryptKey, 0, 10);
                Array.Copy(bufferBytes, count * 4, encryptBuffer, 0, 4);
                PrivateEncrypt(encryptKey, encryptBuffer);
                resultBuffer.Add(encryptBuffer[0]);
                resultBuffer.Add(encryptBuffer[1]);
                resultBuffer.Add(encryptBuffer[2]);
                resultBuffer.Add(encryptBuffer[3]);
                count++;
                if (count >= max) break;
            }

            return resultBuffer.ToArray();
        }

        public static string Decrypt(string key, byte[] buffer)
        {
            byte[] keyBytes = Utility.GetBytes(key);

            var keyLength = keyBytes.Length;
            var bufferLength = buffer.Length;

            if (keyLength % 10 > 0)
                throw new ArgumentException("Key is not divisible by 10", nameof(key));
            if (bufferLength % 4 > 0)
                throw new ArgumentException("Buffer is not divisible by 4", nameof(buffer));
            if (bufferLength / 4 > keyLength / 10)
                throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer is longer way longer than key");

            var encryptKey = new byte[10];
            var encryptBuffer = new byte[4];
            var resultBuffer = new List<byte>();
            int count = 0;
            int max = bufferLength / 4;
            while (true)
            {
                Array.Copy(keyBytes, count * 10, encryptKey, 0, 10);
                Array.Copy(buffer, count * 4, encryptBuffer, 0, 4);
                PrivateDecrypt(encryptKey, encryptBuffer);
                resultBuffer.Add(encryptBuffer[0]);
                resultBuffer.Add(encryptBuffer[1]);
                resultBuffer.Add(encryptBuffer[2]);
                resultBuffer.Add(encryptBuffer[3]);
                count++;
                if (count >= max) break;
            }

            return Utility.FromBytes(resultBuffer.ToArray());
        }

    }
}
