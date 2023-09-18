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
    }
}
