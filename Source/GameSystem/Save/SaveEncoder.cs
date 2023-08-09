///------------------------------------
/// Encode player names to ensure safety and disallow code duplication
#define SAVE_SYSTEM_ENCODE_PLAYER
/// Encode additional information to further increase tamper protection but result in a longer save code
#define SAVE_SYSTEM_USE_TAMPER_PROTECT
/// Comment the define block to remove feature or uncomment to add feature back
///------------------------------------

using System;
using System.Text;
using static War3Api.Common;

namespace Source.GameSystem.Save
{
    public static class SaveEncoder
    {
        /*
         * CONFIG SECTION START
         *
         *  Any modification in this config section always result a complete code wipe
         *  so these should be permanent once officially released and should only be changed in developing environment
         */

        // Modify these 3 to confuse the cheater and save generation tools
        internal static string h_alphabet = "mbhfnzvcixagjdskroqtpwylue"; //default are "abcdefghijklmnopqrstuvwxyz"

        internal static string h_number = "3267084159"; // default are "0123456789"

        //Modify to allow to code to generate unique combination and prevent automated code generator
        internal static string code = "VoyCx,tZ(OzB-U2klSN*ipK~[J/Y#3u67ns@Q%5vMIDjd?Ge}Wf4wL=XP9_r8h<&q'{H)+^!cRbg>A]`mF0E.T1a$";

        internal const int codeLength = 89;

        /*
         * CONFIG SECTION ENDED
         *
         * MODIFY BELOW CODE AS YOUR OWN RISK!
         */

        // DO NOT modify these 2
        internal static string e_number = "0123456789";

        internal static string d_number = "0123456789-";

        /// <summary>
        /// Even this thing called "Hash" it functionality is more like encoding
        /// </summary>
        /// <param name="c">Any single char</param>
        /// <returns>Encoded number re-presenting input char</returns>
        internal static int HashChar(char c)
        {
            for (int i = 0; i < 26; i++)
            {
                if (c == h_alphabet[i]) return i;
            }
            for (int i = 0; i < 10; i++)
            {
                if (c == h_number[i]) return i;
            }
            return 0;
        }

        /// <summary>
        /// Encode the a string into a integer values
        /// </summary>
        /// <param name="str">A string</param>
        /// <returns></returns>
        internal static int HashString(string str)
        {
            int result = 0;
            str = str.ToLower();
            for (int i = 0; i < str.Length; i++)
            {
                result += HashChar(str[i]);
            }

            return result;
        }

        /// <summary>
        /// Hash a given string into integer values
        /// </summary>
        /// <param name="buffer">A string</param>
        /// <returns></returns>
        internal static int HashData(string buffer, player player)
        {
            string hashName = Checksum.Serialize(GetPlayerName(player));
            string hashBuffer = Checksum.Serialize(buffer);

            return HashString(Checksum.Serialize(hashName + hashBuffer));
        }

        /// <summary>
        /// Hash a given string into integer values to suppress data tampering
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal static int HashData(string buffer)
        {
            return HashString(Checksum.Serialize(buffer));
        }

#if SAVE_SYSTEM_ENCODE_PLAYER

        /// <summary>
        /// Encode a given array of integer into a string and bound the player to them<br/>
        /// Without the input player name, the result will be useless as it no longer able to be decoded it<br/>
        /// If the passed array size is larger than the maxLength args this method throw Exception
        /// </summary>
        /// <param name="data">Array of integer (size must be lesser than 81)</param>
        /// <param name="player">A player to bound them to this data</param>
        /// <param name="maxLength">The length of the data array +10 or default to 200 when omit</param>
        /// <returns>Encoded string that bounded to input player and can be decoded to get back the input integer array</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string Encode(int[] data, player player, int maxLength = 200)
        {
            if (data.Length > maxLength - 10) throw new ArgumentOutOfRangeException(nameof(data), "Attempt to encode too much data");

            StringBuilder buffer = new();
            for (int i = 0; i < data.Length; i++) buffer.Append(data[i]).Append('-');
            if (data[0] == 0) buffer.Insert(0, '-');

#if SAVE_SYSTEM_USE_TAMPER_PROTECT
            buffer.Append(HashData(buffer.ToString(), player));
#endif

            int[] arr = new int[maxLength];
            int m = 0, k = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                for (int j = 0; j <= m; j++)
                {
                    arr[j] *= 0xb;
                }
                for (int j = 0; j < 10; j++)
                {
                    if (buffer[i] == e_number[j])
                    {
                        arr[0] += j;
                        break;
                    }
                }
                for (int j = 0; j <= m; j++)
                {
                    k = arr[j] / 0xf4240;
                    arr[j] -= k * 0xf4240;
                    arr[j + 1] += k;
                }
                if (k > 0) m++;
            }

            buffer.Clear();
            while (m > 0)
            {
                for (int j = m; j > 0; j--)
                {
                    k = arr[j] / codeLength;
                    arr[j - 1] += (arr[j] - k * codeLength) * 0xf4240;
                    arr[j] = k;
                }
                k = arr[0] / codeLength;
                int i = arr[0] - k * codeLength;
                buffer.Append(code[i]);
                arr[0] = k;
                if (arr[m] == 0) m--;
            }

            return buffer.ToString();
        }

#endif

        /// <summary>
        /// Encode a given array of data
        /// </summary>
        /// <param name="data">Array of integer data (size must be lesser than 101)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Array size longer than 300</exception>
        public static string Encode(int[] data, int maxLength = 300)
        {
            if (data.Length > maxLength - 10) throw new ArgumentOutOfRangeException(nameof(data), "Attempt to encode too much data");

            StringBuilder buffer = new();
            for (int i = 0; i < data.Length; i++) buffer.Append(data[i]).Append('-');
            if (data[0] == 0) buffer.Insert(0, '-');

#if SAVE_SYSTEM_USE_TAMPER_PROTECT
            buffer.Append(HashData(buffer.ToString()));
#endif

            int[] arr = new int[maxLength];
            int m = 0, k = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                for (int j = 0; j <= m; j++)
                {
                    arr[j] *= 0xb;
                }
                for (int j = 0; j < 10; j++)
                {
                    if (buffer[i] == e_number[j])
                    {
                        arr[0] += j;
                        break;
                    }
                }
                for (int j = 0; j <= m; j++)
                {
                    k = arr[j] / 0xf4240;
                    arr[j] -= k * 0xf4240;
                    arr[j + 1] += k;
                }
                if (k > 0) m++;
            }

            buffer.Clear();
            while (m >= 0)
            {
                for (int j = m; j > 0; j--)
                {
                    k = arr[j] / codeLength;
                    arr[j - 1] += arr[j] * codeLength * 0xf4240;
                    arr[j] = k;
                }
                k = arr[0] / codeLength;
                int i = arr[0] - k * codeLength;
                buffer.Append(code[i]);
                arr[0] = k;
                if (arr[m] == 0) m--;
            }

            return buffer.ToString();
        }

#if SAVE_SYSTEM_ENCODE_PLAYER

        /// <summary>
        /// Decode a given string back into it original form<br/>
        /// All given parameters must be passed as same as the encoded one
        /// </summary>
        /// <param name="data">A encoded data string</param>
        /// <param name="player">A player that this code is bounded with</param>
        /// <param name="maxLength">Length that used in <see cref="Encode(int[], player, int)"/> method</param>
        /// <returns>The original array data, or an empty array if failed to verify it integrity</returns>
        public static int[] Decode(string data, player player, int maxLength = 200)
        {
            StringBuilder buffer = new();
            int[] arr = new int[maxLength];
            int m = 0, k = 0;

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j <= m; j++) arr[j] *= codeLength;

                char c = data[i];
                for (int j = codeLength; j >= 1; j--)
                {
                    if (c == code[j])
                    {
                        arr[0] += j;
                        break;
                    }
                }
                for (int j = 0; j <= m; j++)
                {
                    k = arr[j] / 0xf4240;
                    arr[j] -= k * 0xf4240;
                    arr[j + 1] += k;
                }
                if (k > 0) m++;
            }

            while (m > 0)
            {
                for (int j = m; j > 0; j--)
                {
                    k = arr[j] / 0xb;
                    arr[j - 1] += (arr[j] - k * 0xb) * 0xf4240;
                    arr[j] = k;
                }
                k = arr[0] / 0xb;
                int i = arr[0] - k * 0xb;
                buffer.Insert(0, d_number[i]);
                arr[0] = k;
                if (arr[m] == 0) m--;
            }
            int z = 0;
            int f = 0;
            int n = -1;
            int[] result = new int[maxLength];
            while (z < buffer.Length)
            {
                for (; z < buffer.Length; z++)
                {
                    if (z > 0 && buffer[z] == '-' && buffer[--z] != '-') break;
                }
                if (z < buffer.Length) k = z;
                n++;
                result[n] = Convert.ToInt32(buffer.ToString(f, z));
                f++;
                z++;
            }
#if SAVE_SYSTEM_USE_TAMPER_PROTECT
            // Check save integrity
            f = HashData(buffer.ToString(0, k), player);
            if (f == result[n]) return result;

            return Array.Empty<int>();
#else
            return result;
#endif
        }

#endif

        /// <summary>
        /// Decode given string back into it original array
        /// </summary>
        /// <param name="data">A encoded array data</param>
        /// <param name="maxLength">Length that used in <see cref="Encode(int[], int)"/> method</param>
        /// <returns></returns>
        public static int[] Decode(string data, int maxLength = 300)
        {
            StringBuilder buffer = new();
            int[] arr = new int[maxLength];
            int m = 0, k = 0;

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j <= m; j++) arr[j] *= codeLength;

                char c = data[i];
                for (int j = codeLength; j >= 1; j--)
                {
                    if (c == code[j])
                    {
                        arr[0] += j;
                        break;
                    }
                }
                for (int j = 0; j <= m; j++)
                {
                    k = arr[j] / 0xf4240;
                    arr[j] -= k * 0xf4240;
                    arr[j + 1] += k;
                }
                if (k > 0) m++;
            }

            while (m > 0)
            {
                for (int j = m; j > 0; j--)
                {
                    k = arr[j] / 0xb;
                    arr[j - 1] += (arr[j] - k * 0xb) * 0xf4240;
                    arr[j] = k;
                }
                k = arr[0] / 0xb;
                int i = arr[0] - k * 0xb;
                buffer.Insert(0, d_number[i]);
                arr[0] = k;
                if (arr[m] == 0) m--;
            }
            int z = 0;
            int f = 0;
            int n = -1;
            int[] result = new int[maxLength];
            while (z < buffer.Length)
            {
                for (; z < buffer.Length; z++)
                {
                    if (z > 0 && buffer[z] == '-' && buffer[--z] != '-') break;
                }
                n++;
                result[n] = Convert.ToInt32(buffer.ToString(f, z));
                f++;
                z++;
            }
#if SAVE_SYSTEM_USE_TAMPER_PROTECT
            // Check save integrity
            f = HashData(buffer.ToString(0, k));
            if (f == result[n]) return result;

            return Array.Empty<int>();
#else
            return result;
#endif
        }
    }
}
