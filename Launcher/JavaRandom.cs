﻿using System;

namespace Launcher
{
    public sealed class JavaRandom
    {
        public JavaRandom(long seed)
        {
            _seed = (seed ^ LARGE_PRIME) & ((1L << 48) - 1);
        }

        public int NextInt(int n)
        {
            if (n <= 0)
                throw new ArgumentOutOfRangeException("n", n, "n must be positive");

            if ((n & -n) == n)  // i.e., n is a power of 2
                return (int)((n * (long)next(31)) >> 31);

            int bits, val;

            do
            {
                bits = next(31);
                val = bits % n;
            } while (bits - val + (n - 1) < 0);
            return val;
        }

        private int next(int bits)
        {
            _seed = (_seed * LARGE_PRIME + SMALL_PRIME) & ((1L << 48) - 1);
            return (int)(((uint)_seed) >> (48 - bits));
        }

        private long _seed;

        private const long LARGE_PRIME = 0x5DEECE66DL;
        private const long SMALL_PRIME = 0xBL;
    }
}
