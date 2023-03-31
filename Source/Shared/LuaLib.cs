namespace Source.Shared
{
    /*
     *  Enable public API for C# user to interact with Lua
     *
     *  This require CSharp.lua to be used
     */


    public static class LuaLib
    {
#pragma warning disable CS0626
#pragma warning disable S4200

        /// <summary>
        /// Load and call the given script with it parameters
        /// </summary>
        /// <param name="loadCode">Code to execute</param>
        /// <param name="loadArgs">Parameters of the code</param>
        /// @CSharpLua.Template = "(load({0}))({*1})"

        public static extern void LoadCall(string loadCode, params object[] loadArgs);

        /// <summary>
        /// Get any value from lua global scope (_G)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "rawget(_G, {0})"
        public static extern dynamic GlobalGet(string name);

        /// <summary>
        /// Call lua string.pack
        /// </summary>
        /// @CSharpLua.Template = "string.pack({0}, {*1})"
        public static extern string StringPack(string format, params string[] values);

        /// <summary>
        /// Call lua string.unpack
        /// </summary>
        /// @CSharpLua.Template = "string.unpack({0}, {1}, {2})"
        public static extern string StringUnpack(string format, string values, int? pos);

        /// <summary>
        /// Call lua string.gsub
        /// </summary>
        /// @CSharpLua.Template = "string.gsub({0}, {1}, {2})"
        public static extern string StringGsub(string str, string pattern, dynamic replacement);

        /// <summary>
        /// Call lua string.gsub
        /// </summary>
        /// @CSharpLua.Template = "string.gsub({0}, {1}, {2})"
        public static extern string StringGsub(string str, char pattern, dynamic replacement);

        /// <summary>
        /// Get character from char code
        /// </summary>
        /// @CSharpLua.Template = "string.char({0})"
        public static extern string StringChar(int charCode);

        /// <summary>
        /// Get char code from characters
        /// </summary>
        /// @CSharpLua.Template = "string.byte({0})"
        public static extern int StringByte(char chr);

        /// <summary>
        /// Get char code from the first characters in the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "string.byte({0})"
        public static extern int StringByte(string str);

        /// <summary>
        /// Get char code from specific index of the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="atIndex"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "string.byte({0}, {1})"
        public static extern int StringByte(string str, int atIndex);

        /// <summary>
        /// Get char code from index A to index B of the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns>Array of char code</returns>
        /// @CSharpLua.Template = "string.byte({0}, {1}, {2})"
        public static extern int[] StringByte(string str, int startIndex, int endIndex);

        /// <summary>
        /// Create a lua tables
        /// </summary>
        /// <returns>Lua tables</returns>
        /// @CSharpLua.Template = "{}"
        public static extern object NewTable();

        /// <summary>
        /// Call table.unpack
        /// </summary>
        /// @CSharpLua.Template = "table.unpack({0})"
        public static extern dynamic TableUnpack(object table);

        /// <summary>
        /// Set a value of the specified tables
        /// </summary>
        /// <param name="whichTable">A Lua Table</param>
        /// <param name="keys"></param>
        /// <param name="value"></param>
        /// @CSharpLua.Template = "{0}[{1}]={2}"
        public static extern void TableSet(object whichTable, dynamic keys, dynamic value);

        /// <summary>
        /// Like <see cref="TableSet(object, dynamic, dynamic)"/><br></br> but use rawset that disallow metatable function to invoke
        /// </summary>
        /// <param name="whichTable">A Lua Table</param>
        /// <param name="keys"></param>
        /// <param name="value"></param>
        /// @CSharpLua.Template = "rawset({0}, {1}, {2})"
        public static extern void TableRawSet(object whichTable, dynamic keys, dynamic value);

        /// <summary>
        /// Get a value of the specified tables
        /// </summary>
        /// <param name="whichTable">A Lua Table</param>
        /// <param name="keys"></param>
        /// <returns>Value that the table withhold on specific keys, or null if nothing found</returns>
        /// @CSharpLua.Template = "{0}[{1}]"
        public static extern dynamic TableGet(object whichTable, dynamic keys);

        /// <summary>
        /// Like <see cref="TableGet(object, dynamic)"/><br></br> but uses rawget that disallow metatable function to invoke
        /// </summary>
        /// <param name="whichTable">A Lua Table</param>
        /// <param name="keys"></param>
        /// <returns>Value that the table withhold on specific keys, or null if nothing found</returns>
        /// @CSharpLua.Template = "rawget({0}, {1})"
        public static extern dynamic TableRawGet(object whichTable, dynamic keys);

        /// <summary>
        /// Push a value to the tables
        /// </summary>
        /// <param name="whichTable">A Lua Table</param>
        /// <param name="value"></param>
        /// @CSharpLua.Template = "{0}[#{0}+1]={1}"
        public static extern void TablePush(object whichTable, dynamic value);

        /// <summary>
        /// Get a table length
        /// </summary>
        /// <param name="whichTable">A Lua Table</param>
        /// <returns>Number re-presenting the length of the table</returns>
        /// @CSharpLua.Template = "#{0}"
        public static extern int TableLength(object whichTable);

        /// <summary>
        /// Instead of set, this remove the values from specified table keys
        /// </summary>
        /// <param name="whichTable"></param>
        /// <param name="keys"></param>
        /// @CSharpLua.Template = "{0}[{1}] = nil"
        public static extern void TableRemove(object whichTable, dynamic keys);

        /// <summary>
        /// Null the table
        /// </summary>
        /// <param name="whichTable"></param>
        /// <remarks>This will null the table</remarks>
        /// @CSharpLua.Template = "{0}=nil"
        public static extern void DropTable(object whichTable);

        /// <summary>
        /// Giving the variables a new table ref
        /// </summary>
        /// <param name="whichTable"></param>
        /// @CSharpLua.Template = "{0}={}"
        public static extern void RenewTable(object whichTable);
#pragma warning restore S4200
#pragma warning restore CS0626
    }
}
