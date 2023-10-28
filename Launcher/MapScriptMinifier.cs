using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loretta.CodeAnalysis.Lua;
using Loretta.CodeAnalysis.Text;
using Loretta.CodeAnalysis;
using System.Collections.Immutable;
using Loretta.CodeAnalysis.Lua.Syntax;
using System.Runtime.InteropServices;

namespace Launcher
{
    public static class MapScriptMinifier
    {
        internal static char[] charList = new char[63]
        {
            'c', 'M', 'x', 'C', '8', '0', '6', 'D', 'H', 'F', 'Y', 'J', 'q', '1', 'j', 'w', 'P', 'O', 'U', 'u', 'I', 'V', 'Q', 'm', 't', 'f', 'p', '9', 'z', 'W', 'n', 'g', 'T', 'e', 'X', 'Z', 'G', 'a', 'v', 'l', 's', '7', 'b', 'B', 'o', 'K', 'N', 'd', 'y', 'E', 'r', 'i', 'L', 'k', 'h', 'A', '4', 'S', '5', '3', 'R', '2', '_'
        };
        internal const int charMax = 63;
        internal static Dictionary<string, string> identifierMap = new();
        internal static List<string> identifierInUse = new();
        internal static StringBuilder currentidentifier = new();

        internal static int IndexOf(char[] chars, char target)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == target) return i;
            }
            return -1;
        }

        internal static int IndexOf(string[] strs, string target)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i] == target) return i;
            }
            return -1;
        }

        internal static string GenerateZeroes(int length)
        {
            if (length < 1) return string.Empty;
            if (length == 1) return "c";
            var result = new StringBuilder();
            var zeroes = new StringBuilder();
            zeroes.Append('c');
            while (length > 0)
            {
                if ((length & 1) == 1)
                {
                    result.Append(zeroes);
                }
                if ((length >>= 1) == 1)
                {
                    zeroes.Append('c');
                }
            }
            return result.ToString();
        }

        internal static bool IsKeyword(string str)
        {
            return str.Length switch
            {
                2 => "do" == str || "if" == str || "in" == str || "or" == str,
                3 => "and" == str || "end" == str || "for" == str || "nil" == str ||
                                        "not" == str,
                4 => "else" == str || "goto" == str || "then" == str || "true" == str,
                5 => "break" == str || "false" == str || "local" == str ||
                                        "until" == str || "while" == str,
                6 => "elseif" == str || "repeat" == str || "return" == str,
                8 => "function" == str,
                _ => false,
            };
        }

        internal static string GenerateIdentifier(string original)
        {
            if (original == "self") return original;
            if (identifierMap.ContainsKey(original)) return identifierMap[original];

            int len = currentidentifier.Length;
            var oldStr = currentidentifier.ToString();
            int pos = len - 1;
            char c;
            int index;
            while (pos >= 0)
            {
                c = currentidentifier[pos];
                index = IndexOf(charList, c);
                if (index != charMax)
                {
                    currentidentifier.Clear();
                    currentidentifier.Append(oldStr.AsSpan(0, pos));
                    currentidentifier.Append(charList[index+1]);
                    currentidentifier.Append(GenerateZeroes(len - (pos + 1)));
                    if (IsKeyword(currentidentifier.ToString()) || IndexOf(identifierInUse.ToArray(), currentidentifier.ToString()) > -1) return GenerateIdentifier(original);
                    identifierMap[original] = currentidentifier.ToString();
                    return currentidentifier.ToString();
                }
                pos--;
            }
            currentidentifier.Clear();
            currentidentifier.Append('a');
            currentidentifier.Append(GenerateZeroes(len));
            if (IndexOf(identifierInUse.ToArray(), currentidentifier.ToString()) > -1) return GenerateIdentifier(original);
            identifierMap[original] = currentidentifier.ToString();
            return currentidentifier.ToString();
        }

        internal enum GrabTarget
        {
            Global = 1 << 0,
            Local = 1 << 1,
            All = Global | Local,
        }

        // class that grab globals names
        internal sealed class Formatter : LuaSyntaxWalker
        {

            public Formatter() : base(SyntaxWalkerDepth.Node)
            {

            }

            public override void VisitStatementList(StatementListSyntax node)
            {

            }

            public override void VisitFunctionDeclarationStatement(FunctionDeclarationStatementSyntax node)
            {

            }

            public override void VisitAssignmentStatement(AssignmentStatementSyntax node)
            {

            }
        }

        internal static void ExtractNameFromScope(IScope targetScope)
        {
            foreach (var scope in targetScope.ContainedScopes)
            {
                foreach (var variable in scope.DeclaredVariables)
                {
                    if (variable.Kind != VariableKind.Global) continue;

                    identifierMap[variable.Name] = variable.Name;
                    if (!identifierInUse.Contains(variable.Name)) identifierInUse.Add(variable.Name);
                }
            }
        }

        internal static void FormatScript(ref Script script)
        {

        }


        public static string Minify(string inputLua)
        {
            if (inputLua == null) return string.Empty;

            var ast = LuaSyntaxTree.ParseText(SourceText.From(inputLua, Encoding.UTF8, SourceHashAlgorithm.Sha256),
                                              new(LuaSyntaxOptions.Lua53));
            currentidentifier.Append(charList[0]);
            var script = new Script(ImmutableArray.Create(ast));
            ExtractNameFromScope(script.RootScope);
            //var newStatement = FormatScript(ref script);



            return inputLua;
        }
    }
}
