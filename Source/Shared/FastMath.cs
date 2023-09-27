// ------------------------------------------------------------------------------
// <copyright file="FastMath.cs" company="Kray Oristine">
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

namespace Source.Shared
{
    /// <summary>
    /// A Math library that using caching to speed up some calculation that is usually use multiple times in a row<br/>
    /// This library only implement usually performance heavy calculation method
    /// </summary>
    public static class FastMath
    {

        // pow cache
        private static readonly Dictionary<int, List<int>> _powCache = new();
        private static readonly Dictionary<float, Dictionary<float, float>> _powCacheF = new();

        /// <summary>
        /// Return the value of x to the power of n
        /// </summary>
        /// <param name="n"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Pow(int x, int n)
        {
            if (n == 0) return 1;
            if (x == 0) return 0;

            if (_powCache.TryGetValue(x, out var cachedResult) && cachedResult[n] > 1) return cachedResult[n];
            else
            {
                cachedResult = new List<int>(x);
                _powCache[x] = cachedResult;
            }

            var result = Lua.Math.Pow(x, n);
            cachedResult[n] = result;
            return result;
        }

        /// <summary>
        /// Return the value of x to the power of n
        /// </summary>
        /// <param name="n"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Pow(float x, float n)
        {
            if (n == 0) return 1.0f;
            if (x == 0) return 0.0f;

            if (_powCacheF.TryGetValue(x, out var cachedResult) && cachedResult[n] > 1) return cachedResult[n];
            else
            {
                cachedResult = new();
                _powCacheF[x] = cachedResult;
            }

            var result = Lua.Math.Pow(x, n);
            cachedResult[n] = result;
            return result;
        }

        // log cache
        private static readonly Dictionary<int, List<int>> _logCache = new();

        /// <summary>
        /// Return the logarithm of x in the given base, The default for base is e (so that the function returns the natural logarithm of x)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="_base"></param>
        /// <returns></returns>
        public static int Log(int x, int? _base)
        {
            throw new NotImplementedException("Fast Log has not yet been implemented");
        }
    }
}
