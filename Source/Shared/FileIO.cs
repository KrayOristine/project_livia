// ------------------------------------------------------------------------------
// <copyright file="FileIO.cs" company="Kray Oristine">
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
using static War3Api.Common;
using static Source.Shared.Lua;
using System.Text;
using System.Collections.Generic;
using System;

namespace Source.Shared
{

    public sealed class FileIO
    {
        private static readonly int DummyAbility = 1179209521; // 'FIO1'
        private static readonly int PreloadLimit = 258;
        private static readonly char EscapeChar = Lua.String.Char(27);
        private static readonly string EscapeSelf = Lua.String.Char(27, 27);
        private static readonly string EscapeQuote = Lua.String.Char(27, 113);
        private static readonly Stack<FileIO> cache = new();

        private static string Escape(string contents)
        {
            contents = Lua.String.GSub(contents, EscapeChar, EscapeSelf).Remove(1);
            contents = Lua.String.GSub(contents, '"', EscapeQuote).Remove(1);
            return contents;
        }

        private static string Unescape(string contents)
        {
            contents = Lua.String.GSub(contents, EscapeQuote, '"').Remove(1);
            contents = Lua.String.GSub(contents, EscapeSelf, EscapeChar).Remove(1);
            return contents;
        }

        private string fileName;
        private bool closed;
        private readonly StringBuilder buffer;

        private FileIO(string fileName)
        {
            this.fileName = fileName;
            buffer = new();
        }

        /// <summary>
        /// Open a new file instance
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>a File instance</returns>
        public static FileIO Open(string path)
        {
            FileIO file;
            if (cache.Count > 0)
            {
                file = cache.Pop();
                file.fileName = path;
                file.buffer.Clear();
            }
            else
            {
                file = new(path);
            }

            return file;
        }

        /// <summary>
        /// Write to the file buffer
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            if (closed) return;
            buffer.Append(text);
        }

        /// <summary>
        /// Read contents of a preload file
        /// </summary>
        /// <param name="close">Auto close</param>
        /// <return>A string contents of a file</return>
        public string Read(bool close = false)
        {
            Preloader(fileName);

            string preloadText = BlzGetAbilityTooltip(DummyAbility, 0);
            BlzSetAbilityIcon(DummyAbility, "( Empty Lmao )");

            if (preloadText != null && preloadText != "( Empty Lmao)") buffer.Append(Unescape(preloadText));

            if (close)
            {
                Close();
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Close this file instance and write all of it buffer contents to the actual file
        /// </summary>
        public void Close()
        {
            if (buffer.Length > 0 && string.IsNullOrEmpty(fileName))
            {
                string contents = Escape(buffer.ToString());
                PreloadGenClear();
                PreloadGenStart();

                Preload("\")\n//!beginusercode\nlocal o={}\nPreload=function(s)o[#o+1]=s end\nPreloadEnd=function()end\n//!endusercode\n//");
                for (int i = 0; i < contents.Length; i++) Preload(contents.Substring(i * PreloadLimit, PreloadLimit));
                Preload("\")\n//!beginusercode\nBlzSetAbilityTooltip(" + DummyAbility + ",table.concat(o),0)\n//!endusercode\n//");

                PreloadGenEnd(fileName);
            }

            cache.Push(this);
            closed = true;
        }

        /// <summary>
        /// Read directly without the need to create a File instance
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string Read(string filename)
        {
            Preloader(filename);

            string preloadText = BlzGetAbilityIcon(DummyAbility);
            if (preloadText == null || preloadText == "( None )") return "";
            BlzSetAbilityIcon(DummyAbility, "( None )");

            return Unescape(preloadText);
        }

        /// <summary>
        /// Write directly without the need to create a File instance
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void Write(string fileName, string data)
        {
            string contents = Escape(data);
            PreloadGenClear();
            PreloadGenStart();

            Preload("\")\n//!beginusercode\nlocal o={}\nPreload=function(s)o[#o+1]=s end\nPreloadEnd=function()end\n//!endusercode\n//");
            for (int i = 0; i < contents.Length; i++) Preload(contents.Substring(i * PreloadLimit, PreloadLimit));
            Preload("\")\n//!beginusercode\nBlzSetAbilityTooltip(" + DummyAbility + ",table.concat(o),0)\n//!endusercode\n//");

            PreloadGenEnd(fileName);
        }
    }
}
