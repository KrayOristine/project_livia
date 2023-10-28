// ------------------------------------------------------------------------------
// <copyright file="Utility.cs" company="Kray Oristine">
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
    public static class Utility
    {
        /// <summary>
        /// Preset the value in the list up to <paramref name="length"/>.<br/>
        /// If <paramref name="length"/> is -1, the loop will stop at <paramref name="list"/>.Capacity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="value">The value to preset</param>
        /// <param name="length">The length at which this loop will stop (default -1)</param>
        public static void FillList<T>(List<T> list, T value, int length = -1)
        {
            if (length > list.Capacity || length < -1) return;
            if (length == -1) length = list.Capacity;
            for (int i = 0; i < length; i++)
            {
                list.Add(value);
            }
        }

        /// <summary>
        /// Preset the value in the dictionary up to <paramref name="length"/>.
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="value">The value to preset</param>
        /// <param name="length">The length at which this loop will stop</param>
        public static void FillDictionary<TVal>(Dictionary<int, TVal> dictionary, TVal value, int length)
        {
            for (int i = 0; i < length; i++)
            {
                dictionary[i] = value;
            }
        }

        /// <summary>
        /// As it name suggest
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str), "Input can not be null!");
            var length = str.Length;
            if (length == 1) return new byte[1] { (byte)str[0] };
            var result = new byte[length];
            for (int i = 0; i < length; i++)
                result[i] = (byte)str[i];

            return result;
        }

        /// <summary>
        /// As it name suggest, convert an array of bytes into character
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FromBytes(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            var length = bytes.Length;
            var result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(bytes[i]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Add <paramref name="flagValue"/> flag to <paramref name="baseValue"/>.
        /// </summary>
        /// <param name="baseValue"></param>
        /// <param name="flagValue"></param>
        /// <returns></returns>
        public static int FlagAdd(int baseValue, int flagValue)
        {
            return baseValue | flagValue;
        }

        /// <summary>
        /// Remove <paramref name="flagValue"/> flag from <paramref name="baseValue"/>.
        /// </summary>
        /// <param name="baseValue"></param>
        /// <param name="flagValue"></param>
        /// <returns></returns>
        public static int FlagRemove(int baseValue, int flagValue)
        {
            return baseValue & (~flagValue);
        }
    }
}
