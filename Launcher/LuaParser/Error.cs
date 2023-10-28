using System;

namespace Launcher.LuaParser
{
    /// <summary>
    /// General class for throwing error at user face
    /// </summary>
    public static class Error
    {
        public static void Unexpected(string s1, string s2, string s3)
        {
            throw new Exception($"Unexpected {s1} '{s2}' near {s3}");
        }

        public static void UnexpectedEOF()
        {
            throw new Exception("Unexpected symbol near '<eof>'");
        }

        public static void Expected(string s1, string s2)
        {
            throw new Exception($"'{s1}' expected near '{s2}'");
        }

        public static void ExpectedToken(string s1, string s2)
        {
            throw new Exception($"{s1} expected near {s2}");
        }

        public static void UnfinishedString(string s1)
        {
            throw new Exception($"Unfinished string near '{s1}'");
        }

        public static void MalformedNumber(string s1)
        {
            throw new Exception($"Malformed number near '{s1}'");
        }

        public static void DecimalEscapeTooLarge(string s1)
        {
            throw new Exception($"Decimal escape too large near '{s1}'");
        }

        public static void InvalidEscape(string s1)
        {
            throw new Exception($"Invalid escape sequence near '{s1}'");
        }

        public static void HexadecimalDigitExpected(string s1)
        {
            throw new Exception($"Hexadecimal digit expected near '{s1}'");
        }

        public static void BraceExpected(string s1, string s2)
        {
            throw new Exception($"Missing '{s1}' near '{s2}'");
        }

        public static void UnfinishedLongString(string s1, string s2)
        {
            throw new Exception($"Unfinished long string (starting at line '{s1}') near '{s2}'");
        }

        public static void UnfinishedLongComment(string s1, string s2)
        {
            throw new Exception($"Unfinished long comment (starting at line '{s1}') near '{s2}'");
        }

        public static void AmbiguousSyntax(string s1)
        {
            throw new Exception($"Ambiguous syntax (function call x new statement) near '{s1}'");
        }

        public static void NoLoopToBreak(string s1)
        {
            throw new Exception($"No loop to break near '{s1}'");
        }

        public static void LabelAlreadyDefined(string s1, string line)
        {
            throw new Exception($"Label '{s1}' already defined on line {line}");
        }

        public static void LabelNotVisible(string s1)
        {
            throw new Exception($"No visible '{s1}' label for <goto>");
        }

        public static void GotoJumpToLocalScope(string s1, string s2)
        {
            throw new Exception($"<goto {s1}> jump into scope of local '{s2}'");
        }

        public static void CanNotUseVararg(string s1)
        {
            throw new Exception($"Can not use '...' outside a vararg function near '{s1}'");
        }
    }
}
