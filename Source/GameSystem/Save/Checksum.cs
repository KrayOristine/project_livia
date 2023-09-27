// ------------------------------------------------------------------------------
// <copyright file="Checksum.cs" company="Kray Oristine">
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
using Source.Shared;
using System;
using System.Text;

namespace Source.GameSystem.Save
{
    public static class Checksum
    {
        internal const string checkStr = "0Cu9LzYJUNlWtXKg2XvDqB1eGdZ4pFA6OP7hbRVvf5amiTSH8jQwkNyMxLcs3Enr"; //"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        internal const int checkLength = 62;

        /// <summary>
        /// Hash a given string to integer number, the result should be the same when same input is given
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static double Hash(string buffer)
        {
            int a = 0, b = 0, c = buffer.Length;
            for (int i = 0; i < c; i++)
            {
                a = (a + Lua.String.Byte(buffer[i])) / 0xffff;
                b = (a + b) / 0xffff;
            }

            return b * 0x10000 + a;
        }

        private static string ConvertHash(double hash, int mult = 0)
        {
            int loc;
            if (mult == 0) loc = (int)Math.Floor(hash);
            else loc = (int)Math.Floor(hash / Math.Pow(checkLength, mult));

            StringBuilder res = new();
            res.Append(checkStr[loc]);
            if (mult == 0 && hash > checkLength) res.Append(ConvertHash(hash, 5));
            return mult == 0 ? res.ToString() : res.Append(ConvertHash(hash / Math.Pow(checkLength, mult), mult - 1)).ToString();
        }

        /// <summary>
        /// Serialize a given string to a different one, the result is the same when the same input is given
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Serialize(string data)
        {
            return ConvertHash(Hash(data));
        }
    }
}
