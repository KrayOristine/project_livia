using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.LuaParser.AST_Component
{
    public enum NodeKind
    {
        /// <summary>
        /// A singular node
        /// </summary>
        Node,
        /// <summary>
        /// A block of lua code
        /// </summary>
        Block,
        /// <summary>
        /// A singular variable assignment
        /// </summary>
        VariableAssignment,
        /// <summary>
        /// Any variable declaration, can be singular or multiple
        /// </summary>
        VariableDeclaration,
    }
}
