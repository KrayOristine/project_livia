// ------------------------------------------------------------------------------
// <copyright file="SaveCompression.cs" company="Kray Oristine">
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
using System.Collections.Generic;

/*
 * Ozzzzymaniac Custom Save/Load
 *
 * This is the core function of the maps
 */

namespace Source.GameSystem.Save
{
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

            id -= i * 64 * 64 * 64;
            i = id / (64 * 64);
            id -= i * 64 * 64;
            r = r * 256 + decompress[i];
            i = id / 64;
            r = r * 256 + decompress[i];

            return r * 256 + decompress[id - i * 64];
        }
    }
}
