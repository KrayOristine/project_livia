// ------------------------------------------------------------------------------
// <copyright file="PRNG.cs" company="Kray Oristine">
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


/*
 *  Pseudo Random Number Generation
 *
 *  Args for new():
 *       seed - initial seed of the RNG table
 *       mult - multiplier value
 *       mod - modulo value
 *       inc - increment value
 *  After that you can call Next() to generate a new random values
 *       .Next() -- no arguments is a range between 0 and 1
 *       .Next(100) -- only 1 argument means a range 1 - val (1-100 in this case)
 *       .Next(5.0,25.0) -- two arguments means a range from low - high (int or float)
 */

namespace Source.Shared
{

    public sealed class LinearCongruentialGenerator
    {
        private int seed;
        private readonly int div;
        private readonly int mod;
        private readonly int mult;
        private readonly int inc;

        public LinearCongruentialGenerator(int seed, int mult, int mod, int inc)
        {
#if DEBUG
            if (seed < 0) throw new ArgumentOutOfRangeException(nameof(seed), $"Don't pass negative seed value to {nameof(LinearCongruentialGenerator)}");
            if (mult < 0) throw new ArgumentOutOfRangeException(nameof(mult), $"Don't pass negative mult value to {nameof(LinearCongruentialGenerator)}");
            if (mod < 0) throw new ArgumentOutOfRangeException(nameof(mod), $"Don't pass negative mod value to {nameof(LinearCongruentialGenerator)}");
            if (inc < 0) throw new ArgumentOutOfRangeException(nameof(inc), $"Don't pass negative inc value to {nameof(LinearCongruentialGenerator)}");
#else
            if (seed < 0 || inc < 0 || mod < 0 || mult < 0)
                throw new ArgumentOutOfRangeException();
#endif
            this.seed = seed;
            this.mod = mod;
            this.mult = mult;
            this.inc = inc;
            div = 1 / mod;
        }

        /// <summary>
        /// Generate a new random values in between 0 and 1
        /// </summary>
        /// <returns></returns>
        public float Next()
        {
            var d = seed * mult + inc;
            var m = MathF.Floor(d * div) * mod;
            if (m < 0) m += mod;

            seed = (int)m;
            m *= div;

            return m;
        }

        /// <summary>
        /// Generate a new random values in between 1 and low parameters
        /// </summary>
        /// <param name="max">Maximum value to generate</param>
        /// <returns></returns>
        public float Next(float max)
        {
            var d = seed * mult + inc;
            var m = MathF.Floor(d * div) * mod;
            if (m < 0) m += mod;

            seed = (int)m;
            m *= div;

            return m * (max - 1) + 1;
        }

        /// <summary>
        /// Generate a new random value in between low and high parameters
        /// </summary>
        /// <param name="low">Minimum value</param>
        /// <param name="high">Maximum value</param>
        /// <returns></returns>
        public float Next(float low, float high)
        {
            var d = seed * mult + inc;
            var m = MathF.Floor(d * div) * mod;
            if (m < 0) m += mod;

            seed = (int)m;
            m *= div;

            return m * (high - low) + low;
        }

        /// <summary>
        /// Generate a new random value in between low and high parameters<br/>
        /// This method convert the output to became integer
        /// </summary>
        /// <param name="low">Minimum value</param>
        /// <param name="high">Maximum value</param>
        /// <returns></returns>
        public int Next(int low, int high)
        {
            var d = seed * mult + inc;
            var m = MathF.Floor(d * div) * mod;
            if (m < 0) m += mod;

            seed = (int)m;
            m *= div;
            m *= high - low + low;
            m = m >= 0 ? MathF.Floor(m + 0.5f) : MathF.Ceiling(m - 0.5f);
            return (int)m;
        }
    }
}
