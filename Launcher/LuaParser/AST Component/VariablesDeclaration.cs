using System.Collections.Generic;

namespace Launcher.LuaParser.AST_Component
{
    /// <summary>
    /// A variables declaration statement<br/>
    /// For example:<br/><br/>
    ///
    /// ```lua
    /// local a = 1 -- local declaration
    /// b = {} -- global declaration
    /// local c,d = 1,2 -- multiple local declaration
    /// e,f = {},{} -- multiple global declaration
    /// ```
    /// <
    /// </summary>
    public class VariablesDeclaration : INode
    {
        /// <summary>
        /// List of variable declaration in one block
        /// </summary>
        public List<INode> statementList = new();
        public SyntaxKind Kind { get; set; }
        public NodeKind Type { get; set; }
        public string Text { get; set; }
        public string FullText { get; set; }
        /// <summary>
        /// The declaration is local
        /// </summary>
        public bool LocalDeclaration { get; set; }

        public VariablesDeclaration(SyntaxKind kind, string text, string fullText, bool isLocal)
        {
            Kind = kind;
            Text = text;
            FullText = fullText;
            Type = NodeKind.VariableDeclaration;
            LocalDeclaration = isLocal;
        }

        /// <summary>
        /// Transform a node into VariablesDeclaration if the node is valid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>VariablesDeclaration object when the given node is valid otherwise null</returns>
        public static VariablesDeclaration Transform(INode obj)
        {
            return obj.Type == NodeKind.VariableDeclaration ? obj as VariablesDeclaration : null;
        }
    }
}
