using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.LuaParser.AST_Component
{
    /// <summary>
    /// A block of code in Lua
    /// </summary>
    public class Block : INode
    {
        /// <summary>
        /// List of the statement that contained in the block
        /// </summary>
        public List<INode> statementList = new();
        public SyntaxKind Kind { get; set; }
        public NodeKind Type { get; set; }
        public string Text { get; set; }
        public string FullText { get; set; }

        public Block(SyntaxKind kind, string text, string fullText)
        {
            Kind = kind;
            Text = text;
            FullText = fullText;
            Type = NodeKind.Block;
        }

        /// <summary>
        /// Transform a node into Block if the node is valid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Block object when the given node is valid otherwise null</returns>
        public static Block Transform(INode obj)
        {
            return obj.Type == NodeKind.Block ? obj as Block : null;
        }
    }
}
