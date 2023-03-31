using Source.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using War3Api;

/*
 * Ozzzzymaniac Custom Save/Load
 *
 * This is the core function of the maps
 */

namespace Source.Modules
{
    public abstract class Saveable
    {
        protected readonly Common.player boundedPlayer;
        protected readonly int boundedSlot;

        protected readonly string hash;

        protected Saveable(Common.player boundPlayer, int boundSlot)
        {
            boundedPlayer = boundPlayer;
            boundedSlot = boundSlot;
            hash = Checksum.Serialize(Common.GetPlayerName(boundPlayer) + boundSlot.ToString());
        }
    }

    public class HeroBank : Saveable
    {
        internal HeroBank(Common.player player, int slot) : base(player, slot)
        {
        }
    }

    internal class PlayerBank
    {
        public int KEY = 0;
        public int HASH = 0;
        public List<HeroBank> saveSlots = new();
        private readonly Common.player boundedPlayer;

        /// <summary>
        /// Create a new bank for player, this is a core bank that contain shared data between hero like achievement, donation skins, etc.
        /// </summary>
        /// <param name="player">A player to bound this bank</param>
        /// <param name="key">A player key to encrypt/decrypt the hero bank - empty for performance if needed</param>
        /// <param name="hash">A unique hash of the player to encode/decode the hero bank</param>
        public PlayerBank(Common.player player, int key, int hash)
        {
            boundedPlayer = player;
            KEY = key;
            HASH = hash;
        }

        /// <summary>
        /// Create a new slot for the save
        /// just be careful as too many slot will cause the loading of this meta data became unstable
        /// </summary>
        /// <returns>A new bank for you to modify</returns>
        public HeroBank CreateNewSlot()
        {
            var bank = new HeroBank(boundedPlayer, saveSlots.Count);
            saveSlots.Add(bank);

            return bank;
        }

        /// <summary>
        /// Get the save bank from specific slots
        ///
        /// If the slot is exceed current maximum slot, return the last slot
        /// If the slot is less than 0 return the first slot
        /// </summary>
        /// <param name="num">Slot index</param>
        /// <returns>A hero bank on given index</returns>
        public HeroBank GetSaveSlot(int num)
        {
            if (num > saveSlots.Count) return saveSlots[^1];
            if (num < 0) return saveSlots[0];

            return saveSlots[num];
        }
    }

    public static class SaveableObject
    {
        internal static readonly List<int> heroStorage = new();
        internal static readonly List<int> abilityStorage = new();
        internal static readonly List<int> upgradeStorage = new();
        internal static int heroCounter = 0;
        internal static int abilityCounter = 0;
        internal static int upgradeCounter = 0;

        public static void AddSaveableHero(int id)
        {
            if (heroStorage.Contains(id)) return;

            heroStorage.Insert(id, heroCounter);
            heroStorage.Insert(heroCounter, id);
            heroCounter++;
        }

        public static void AddSaveableAbility(int id)
        {
            if (abilityStorage.Contains(id)) return;

            abilityStorage.Insert(id, abilityCounter);
            abilityStorage.Insert(abilityCounter, id);
            abilityCounter++;
        }

        public static void AddSaveableUpgrade(int id)
        {
            if (upgradeStorage.Contains(id)) return;

            upgradeStorage.Insert(id, upgradeCounter);
            upgradeStorage.Insert(upgradeCounter, id);
            upgradeCounter++;
        }

        public static int GetHeroId(int id)
        {
            return heroStorage.Contains(id) ? heroStorage[id] : -1;
        }

        public static int GetAbilityId(int id)
        {
            return abilityStorage.Contains(id) ? abilityStorage[id] : -1;
        }

        public static int GetUpgradeId(int id)
        {
            return upgradeStorage.Contains(id) ? upgradeStorage[id] : -1;
        }
    }

    public static class SaveCompression
    {
        internal static readonly int doInit = Init();
        internal static readonly List<int> compress = new();
        internal static readonly List<int> decompress = new();

        private static int Init()
        {
            for (int i = 0; i < 10; i++)
            {
                compress[i + 48] = i;
                decompress[i] = i + 48;
            }
            for (int i = 0; i < 26; i++)
            {
                compress[i + 97] = i;
                compress[i + 65] = i + 26;
                decompress[i + 10] = i + 97;
                decompress[i + 36] = i + 65;
            }
            return 0;
        }

        public static int CompressId(int id)
        {
            int i = id / 0x1000000;
            int r = compress[i];

            id -= i * 0x1000000;
            i = id / 0x10000;
            id -= i * 0x10000;
            r = r * 64 + compress[i];
            i = id / 256;
            r = r * 64 + compress[i];

            return r * 64 + compress[id - i * 256];
        }

        public static int DecompressId(int id)
        {
            int i = id / (64 * 64 * 64);
            int r = decompress[i];

            id -= i * (64 * 64 * 64);
            i = id / (64 * 64);
            id -= i * (64 * 64);
            r = r * 256 + decompress[i];
            i = id / 64;
            r = r * 256 + decompress[i];

            return r * 256 + decompress[id - i * 64];
        }
    }

    public static class Checksum
    {
        internal const string checkStr = "ZxJW8L9vXQl6qK5R7g3N4b2a0Y1cUdTeSfPhOiMnNmLkHjGpFrEsDtCuBvAwVzXy"; //"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        internal const int checkLength = 62;

        public static double Hash(string buffer)
        {
            int a = 0, b = 0, c = buffer.Length;
            for (int i = 0; i < c; i++)
            {
                a = (a + LuaLib.StringByte(buffer[i])) / 0xffff;
                b = (a + b) / 0xffff;
            }

            return b * 0x10000 + a;
        }

        private static string ConvertHash(double hash, int mult = 0)
        {
            int loc;
            if (mult == 0) loc = (int)Math.Floor(hash);
            else loc = (int)Math.Floor(hash / Math.Pow(checkLength, mult));

            StringBuilder res = new();
            res.Append(checkStr[loc]);
            if (mult == 0 && hash > checkLength) res.Append(ConvertHash(hash, 5));
            return mult == 0 ? res.ToString() : res.Append(ConvertHash(hash / Math.Pow(checkLength, mult), mult - 1)).ToString();
        }

        public static string Serialize(string data)
        {
            return ConvertHash(Hash(data));
        }
    }

    /*
     * Save Encoder v1.0
     *
     * TODO: Improve the encoding speed
     */

    public static class SaveEncoder
    {
        /*
         * CONFIG SECTION START
         *
         *  Any modification in this config section always result a complete code wipe
         *  so these should be permanent once officially released and should only be changed in developing environment
         */

        // Modify these 3 to confuse the cheater and save generation tools
        internal const string h_alphabet = "qjzvbxwycfukdsgnmlhtreaoip"; //default are "abcdefghijklmnopqrstuvwxyz"

        internal const string h_alphabetCapitalize = "QJZVBXWYCFUKDSGNMLHTREAOIP"; // default are "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        internal const string h_number = "7390581246"; // default are "0123456789"

        //Modify to allow to code to generate unique combination and prevent automated code generator
        internal const string code = ">0vyRNtD7Fg(WdYKJiEqU^.@5),1{xzHk+oSCfnjXhe_#]*2OGrlwI?acu9$3|p/ML8Z!b6~}A[BQ-smTP4<`&'=V";

        internal const int codeLength = 90;

        /*
         * CONFIG SECTION ENDED
         *
         * MODIFY BELOW CODE AS YOUR OWN RISK!
         */

        // DO NOT modify these 2
        internal const string e_number = "0123456789";

        internal const string d_number = "0123456789-";

        /// <summary>
        /// Even this thing called "Hash" it functionality is more like encoding
        /// </summary>
        /// <param name="c">Any single char</param>
        /// <returns>Encoded number re-presenting input char</returns>
        internal static int HashChar(char c)
        {
            for (int i = 0; i < h_alphabet.Length; i++)
            {
                if (c == h_alphabet[i] || c == h_alphabetCapitalize[i]) return i;
            }
            for (int i = 0; i < h_number.Length; i++)
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
        internal static int HashData(string buffer, Common.player player)
        {
            string hashName = Checksum.Serialize(Common.GetPlayerName(player));
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
        public static string Encode(int[] data, Common.player player, int maxLength = 200)
        {
            if (data.Length > maxLength - 10) throw new ArgumentOutOfRangeException(nameof(data), "Attempt to encode too much data");

            StringBuilder buffer = new();
            for (int i = 0; i < data.Length; i++) buffer.Append(data[i]).Append('-');
            if (data[0] == 0) buffer.Insert(0, '-');

            buffer.Append(HashData(buffer.ToString(), player));

            int[] arr = new int[maxLength];
            int m = 0, k = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                for (int j = 0; j <= m; j++)
                {
                    arr[j] *= 0xb;
                }
                for (int j = 0; j < e_number.Length; j++)
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

        /// <summary>
        /// Encode a given array of data
        /// </summary>
        /// <param name="data">Array of integer data (size must be lesser than 101)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Array size longer than 300</exception>
        public static string Encode(int[] data, int maxLength = 300)
        {
            if (data.Length > maxLength - 5) throw new ArgumentOutOfRangeException(nameof(data), "Attempt to encode too much data");

            StringBuilder buffer = new();
            for (int i = 0; i < data.Length; i++) buffer.Append(data[i]).Append('-');
            if (data[0] == 0) buffer.Insert(0, '-');

            int[] arr = new int[maxLength];
            int m = 0, k = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                for (int j = 0; j <= m; j++)
                {
                    arr[j] *= 0xb;
                }
                for (int j = 0; j < e_number.Length; j++)
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

        /// <summary>
        /// Decode a given string back into it original form<br/>
        /// All given parameters must be passed as same as the encoded one
        /// </summary>
        /// <param name="data">A encoded data string</param>
        /// <param name="player">A player that this code is bounded with</param>
        /// <param name="maxLength">Length that used in <see cref="Encode(int[], player, int)"/> method</param>
        /// <returns>The original array data, or an empty array if failed to verify it integrity</returns>
        public static int[] Decode(string data, Common.player player, int maxLength = 200)
        {
            var buffer = new StringBuilder();
            var arr = new int[maxLength];
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

            // Check save integrity
            f = HashData(buffer.ToString(0, k), player);
            if (f == result[n]) return result;

            return Array.Empty<int>();
        }

        /// <summary>
        /// Decode given string back into it original array
        /// </summary>
        /// <param name="data">A encoded array data</param>
        /// <param name="maxLength">Length that used in <see cref="Encode(int[], int)"/> method</param>
        /// <returns></returns>
        public static int[] Decode(string data, int maxLength = 300)
        {
            var buffer = new StringBuilder();
            var arr = new int[maxLength];
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

            return result;
        }
    }
}
