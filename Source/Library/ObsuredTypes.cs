using System;

namespace Source.Library
{

    public class ObscuredInt
    {
        private readonly string obscured;
        private readonly int epsilon;
        private readonly int gamma;
        private readonly int beta;
        private readonly int key;

        public ObscuredInt(int value, int key = 0x7fffffff)
        {
            if (value < key) key = 0x7fffffff;
            epsilon = value;
            beta = value;
            gamma = value;
            this.key = key;
            obscured = Convert.ToString(~value & key, 32);
        }

        public int Truth {
            get
            {
                int t = key ^ Convert.ToInt32(obscured, 32);

                if (epsilon == beta && beta == gamma && epsilon == t)
                {
                    return t;
                }
                // Yep, cheater detected do something with him!

                return t;
            }
        }

        public static implicit operator ObscuredInt(int v) => new(v);
        public static explicit operator int(ObscuredInt v) => v.Truth;

        // Plus operator
        public static ObscuredInt operator +(ObscuredInt a) => a;
        public static ObscuredInt operator +(ObscuredInt a, ObscuredInt b) => new(a.Truth + b.Truth);
        public static ObscuredInt operator +(ObscuredInt a, int b) => new(a.Truth + b);
        public static ObscuredInt operator +(int a, ObscuredInt b) => new(a + b.Truth);

        // Minus operator
        public static ObscuredInt operator -(ObscuredInt a) => new(-a.Truth);
        public static ObscuredInt operator -(ObscuredInt a, ObscuredInt b) => new(a.Truth - b.Truth);
        public static ObscuredInt operator -(ObscuredInt a, int b) => new(a.Truth - b);
        public static ObscuredInt operator -(int a, ObscuredInt b) => new(a - b.Truth);

        // Multiply operator
        public static ObscuredInt operator *(ObscuredInt a, ObscuredInt b) => new(a.Truth * b.Truth);
        public static ObscuredInt operator *(ObscuredInt a, int b) => new(a.Truth * b);
        public static ObscuredInt operator *(int a, ObscuredInt b) => new(a * b.Truth);

        // Divide operator
        public static ObscuredInt operator /(ObscuredInt a, ObscuredInt b) {
            int an = a.Truth;
            int bn = b.Truth;
            if (bn == 0) { throw new DivideByZeroException(); }

            return new(an / bn);
        }
        public static ObscuredInt operator /(ObscuredInt a, int b)
        {
            int an = a.Truth;
            int bn = b;
            if (bn == 0) { throw new DivideByZeroException(); }

            return new(an / bn);
        }
        public static ObscuredInt operator /(int a, ObscuredInt b)
        {
            int an = a;
            int bn = b.Truth;
            if (bn == 0) { throw new DivideByZeroException(); }

            return new(an / bn);
        }

        // Remainder operator
        public static ObscuredInt operator %(ObscuredInt a, ObscuredInt b)
        {
            int an = a.Truth;
            int bn = b.Truth;
            if (bn == 0) { throw new DivideByZeroException(); }

            return new(an % bn);
        }
        public static ObscuredInt operator %(ObscuredInt a, int b)
        {
            int an = a.Truth;
            int bn = b;
            if (bn == 0) { throw new DivideByZeroException(); }

            return new(an % bn);
        }
        public static ObscuredInt operator %(int a, ObscuredInt b)
        {
            int an = a;
            int bn = b.Truth;
            if (bn == 0) { throw new DivideByZeroException(); }

            return new(an % bn);
        }
    }
}
