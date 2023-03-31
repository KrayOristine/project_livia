namespace Source.Shared
{
    public static class Encoder
    {
        public static uint[] Encode(char[] data)
        {
            uint[] result = new uint[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                result[i] = data[i];
            }
            return result;
        }
    }
}
