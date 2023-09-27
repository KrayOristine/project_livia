using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.LuaParser.AST_Component
{
    /// <summary>
    /// An interface that define a node in the syntax tree<br/>
    /// the node can be anything.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Node kind
        /// </summary>
        public SyntaxKind Kind { get; set; }
        /// <summary>
        /// Node type in Lua
        /// </summary>
        public NodeKind Type { get; set; }
        /// <summary>
        /// Escaped node code in Lua
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Node code from the string user given
        /// </summary>
        public string FullText { get; set; }
    }
}
