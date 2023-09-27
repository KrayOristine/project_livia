namespace Launcher
{
    /*
     *  This is wrapper file, that cut out unnecessary code and combine it into single readable code but this file may won't be like that
     */

    public static class Shared
    {
        public static int ConvertFourCC(string code)
        {
            if (string.IsNullOrEmpty(code)) return 0;

            if (code.Length > 4) return 0;

            return (code[3] << 24) | (code[2] << 16) | (code[1] << 8) | code[0];
        }

        public static string ConvertFourCC(int code)
        {
            if (code == 0) return "";

            char[] chr = new char[4];

            chr[0] = (char)(code & 0xFF);
            chr[1] = (char)((code >> 8) & 0xFF);
            chr[2] = (char)((code >> 16) & 0xFF);
            chr[3] = (char)((code >> 24) & 0xFF);
            return new string(chr);
        }
    }
}
