using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loretta.CodeAnalysis.Lua;
using Loretta.CodeAnalysis.Text;
using Loretta.CodeAnalysis;

using SyntaxTree = Loretta.CodeAnalysis.SyntaxTree;
using War3Net.CodeAnalysis.Jass;
using System.IO;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace Launcher
{
    public static class MapScriptObfuscator
    {
        internal static uint counter = 0;
        internal static uint charCount = 1;
        internal static char[] charList = new char[63]
        {
            'c', 'M', 'x', 'C', '8', '0', '6', 'D', 'H', 'F', 'Y', 'J', 'q', '1', 'j', 'w', 'P', 'O', 'U', 'u', 'I', 'V', 'Q', 'm', 't', 'f', 'p', '9', 'z', 'W', 'n', 'g', 'T', 'e', 'X', 'Z', 'G', 'a', 'v', 'l', 's', '7', 'b', 'B', 'o', 'K', 'N', 'd', 'y', 'E', 'r', 'i', 'L', 'k', 'h', 'A', '4', 'S', '5', '3', 'R', '2', '_'
        };
        internal static Dictionary<string, bool> nativeList = new();

        internal static string GetName()
        {
            if (counter > 63 * charCount)
            {
                charCount++;
                counter = 0;
            }
            var sb = new StringBuilder();
            uint c = counter;
            while (true)
            {
                if (c > 63)
                {
                    sb.Append(charList[63]);
                    c -= 63;
                    continue;
                }

                sb.Append(charList[c]);
                break;
            }

            counter++;
            return sb.ToString();
        }

        internal static void ParseCommonAndBlizzard(JassCompilationUnitSyntax jassSyntaxTree)
        {
            foreach (var node in jassSyntaxTree.Declarations)
            {
                if (node is JassNativeFunctionDeclarationSyntax nativeDeclaration)
                {
                    nativeList.Add(nativeDeclaration.FunctionDeclarator.IdentifierName.Name, true);
                }
                if (node is JassFunctionDeclarationSyntax functionDeclaration)
                {
                    nativeList.Add(functionDeclaration.FunctionDeclarator.IdentifierName.Name, true);
                }
            }
        }


        internal static void WalkTree(SyntaxTree tree)
        {
            if (tree == null) return;


        }

        public static string ObfuscateScript(string inputLua)
        {
            var syntaxTree = LuaSyntaxTree.ParseText(SourceText.From(inputLua, Encoding.UTF8,  SourceHashAlgorithm.Sha256), new(LuaSyntaxOptions.Lua53));

            return inputLua;
        }
    }
}
