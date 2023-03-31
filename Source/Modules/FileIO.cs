using Source.Shared;
using static War3Api.Common;

namespace Source.Modules
{
    /*
     * TriggerHappy's W3TS FileIO but in C#
     *
     * Ported by Ozzzzymaniac
     */

    public static class FileIO
    {
        internal static readonly int DummyAbility = 1179209521; // 'FIO1'
        internal static readonly int PreloadLimit = 259;
        internal static readonly string EscapeChar = LuaLib.StringChar(27);
        internal static readonly string EscapeSelf = EscapeChar + EscapeChar;
        internal static readonly string EscapeQuote = EscapeChar + 'q';

        internal static string Escape(string contents)
        {
            contents = LuaLib.StringGsub(contents, EscapeChar, EscapeSelf).Remove(1);
            contents = LuaLib.StringGsub(contents, '"', EscapeQuote).Remove(1);
            return contents;
        }

        internal static string Unescape(string contents)
        {
            contents = LuaLib.StringGsub(contents, EscapeQuote, '"').Remove(1);
            contents = LuaLib.StringGsub(contents, EscapeSelf, EscapeChar).Remove(1);
            return contents;
        }

        public static string? Read(string filename)
        {
            string originalIcon = BlzGetAbilityIcon(DummyAbility);
            if (originalIcon == null) return null;

            Preloader(filename);

            string preloadText = BlzGetAbilityIcon(DummyAbility);
            if (preloadText == null || preloadText == originalIcon) return null;
            BlzSetAbilityIcon(DummyAbility, originalIcon);

            return Unescape(preloadText);
        }

        public static void WriteRaw(string filename, string data, bool allowReading = false)
        {
            PreloadGenClear();
            PreloadGenStart();

            if (allowReading)
            {
                Preload("\")\n//! beginusercode\nlocal o=''\nPreload=function(s)o=o..s end\nPreloadEnd=function()end\n//!endusercode\n//");
                data = Escape(data);
            }

            for (int i = 0; i < data.Length; i++)
            {
                Preload(data.Substring(i * PreloadLimit, PreloadLimit));
            }

            if (allowReading)
            {
                Preload("\")\n//! beginusercode\nBlzSetAbilityIcon(" + DummyAbility + ",o)\n//!endusercode\n//");
            }

            PreloadGenEnd(filename);
        }

        public static void Write(string filename, string data) => WriteRaw(filename, data, true);
    }
}
