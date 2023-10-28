namespace Launcher.LuaParser.AST_Component
{
    /// <summary>
    /// Like <see cref="NodeKind.VariableDeclaration"/> but for a single variable
    /// </summary>
    public class VariableAssignment<T> : INode
    {
        public SyntaxKind Kind { get; set; }
        public NodeKind Type { get; set; }
        public string Text { get; set; }
        public string FullText { get; set; }
        /// <summary>
        /// The declaration is local
        /// </summary>
        public bool LocalDeclaration { get; set; }
        /// <summary>
        /// The name of the variable being assigned
        /// </summary>
        public string VariableName { get; set; }
        /// <summary>
        /// The value that being assigned
        /// </summary>
        public T AssignValue { get; set; }

        public VariableAssignment(SyntaxKind kind, string text, string fullText, string name, T value, bool isLocal = true)
        {
            Kind = kind;
            Text = text;
            FullText = fullText;
            Type = NodeKind.VariableAssignment;
            VariableName = name;
            AssignValue = value;
            LocalDeclaration = isLocal;
        }

        /// <summary>
        /// Transform a node into VariableAssignment if the node is valid
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>VariableAssignment object when the given node is valid otherwise null</returns>
        public static VariableAssignment<TOut> Transform<TOut>(INode obj)
        {
            return obj.Type == NodeKind.VariableAssignment ? obj as VariableAssignment<TOut> : null;
        }
    }
}
