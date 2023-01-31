using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Utils
{
    public static class Encoder
    {
        public static uint[] Encode(char[] data)
        {
            uint[] result = new uint[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (uint)data[i];
            }
            return result;
        }
    }
}
