using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.LuaParser
{
    /// <summary>
    /// Syntax Token
    /// </summary>
    public enum SyntaxKind
    {
        Unknown = -1,
        EndOfFileToken = 1,
        StringLiteral = 1 << 1,
        Keyword = 1 << 2,
        Identifier = 1 << 3,
        NumericLiteral = 1 << 4,
        Punctuator = 1 << 5,
        BooleanLiteral = 1 << 6,
        NilLiteral = 1 << 7,
        VarargLiteral = 1 << 8,
    }
}
