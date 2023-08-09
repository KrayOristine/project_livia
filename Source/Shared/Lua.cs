// Ignore Spelling: Metatable Metatables rhs lhs Yieldable Utf repl tbl

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Source.Shared
{
    /*
     *  This snippet allow user to use lua in the c# code but still retain c# static type
     *
     *  but at a cost, some method will not be able to put in here
     */

    /// <summary>
    /// Static entry class contain all of the usable lua standard method, function, table<br/>
    /// This does not provide all of Lua method, some will be removed because it doesn't stable <br/><br/>
    /// Be careful that every method in this use 1-index not 0-index
    /// </summary>
    /// @CSharpLua.Ignore
    public static class Lua
    {
#pragma warning disable CS0626
#pragma warning disable S4200
#pragma warning disable CS8618

        public const string _VERSION = "Lua 5.3";

        /// <summary>
        /// Lua global variable table
        /// </summary>
        /// @CSharpLua.Template = "_G"
        public static readonly Table _G;

        /// <summary>
        /// Call <see cref="Error"/> if it <paramref name="v"/> argument is false, otherwise return the boolean<br/>
        /// In case of error, message is the error object, which is default to "assertion failed!"
        /// </summary>
        /// <param name="v"></param>
        /// @CSharpLua.Template = "assert({0})"
        public static extern bool Assert(bool v);

        /// <summary>
        /// Call <see cref="Lua.Error"/> if it <paramref name="v"/> argument is false, otherwise return the argument passed in ei<br/>
        /// In case of error, message is the error object, which is default to "assertion failed!"
        /// </summary>
        /// <param name="v"></param>
        /// @CSharpLua.Template = "assert({0})"
        public static extern object? Assert(object? v);

        /// <summary>
        /// Call <see cref="Error"/> if it <paramref name="v"/> argument is false, otherwise return the boolean<br/>
        /// In case of error, message is the error object, which is default to "assertion failed!"
        /// </summary>
        /// <param name="v"></param>
        /// <param name="message">Error object or string message</param>
        /// @CSharpLua.Template = "assert({0}, {1})"
        public static extern bool Assert(bool v, string message);

        /// <summary>
        /// Call <see cref="Lua.Error"/> if it <paramref name="v"/> argument is false, otherwise return the argument passed in ei<br/>
        /// In case of error, message is the error object, which is default to "assertion failed!"
        /// </summary>
        /// <param name="v"></param>
        /// <param name="message">Error object or string message</param>
        /// @CSharpLua.Template = "assert({0}, {1})"
        public static extern object? Assert(object? v, string message);

        /// <summary>
        /// Please see <see href="https://www.lua.org/manual/5.3/manual.html#pdf-collectgarbage">this</see> document for best explanation how this work
        /// </summary>
        /// <param name="option"></param>
        /// <param name="arg"></param>
        /// <remarks>Disabled in Warcraft III 1.32+</remarks>
        /// @CSharpLua.Template = "collectgarbage({0}, {1})"
        public static extern void CollectGarbage(string option, int? arg);

        /// <summary>
        /// Terminates the last protected function called and returns message as the error object.<br/>
        /// The level argument specifies how to get the error position.<br/>
        /// With level 1 (the default), the error position is where the error function was called.<br/>
        /// Level 2 points the error to where the function that called error was called; and so on.<br/>
        /// Passing a level 0 avoids the addition of error position information to the message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// @CSharpLua.Template = "error({0}, {1})"
        public static extern void Error(string message, int level = 1);

        /// <summary>
        /// Load a chunk<br/><br/>
        ///
        /// If chunk is a string, the chunk is this string. If chunk is a function,<br/>
        /// load calls it repeatedly to get the chunk pieces. Each call to chunk<br/>
        /// must return a string that concatenates with previous results. A return<br/>
        /// of an empty string, nil, or no value signals the end of the chunk.<br/><br/>
        ///
        /// If there are no syntactic errors, returns the compiled chunk as a function; <br/>
        /// otherwise, returns nil plus the error message.<br/><br/>
        ///
        /// If the resulting function has upvalues, the first upvalue is set to the value<br/>
        /// of <paramref name="env"/>, if that parameter is given, or to the value of the global<br/>
        /// environment.Other upvalues are initialized with nil. (When you load a main<br/>
        /// chunk, the resulting function will always have exactly one upvalue, the _ENV<br/>
        /// variable (see <see href="https://www.lua.org/manual/5.3/manual.html#2.2">§2.2</see>). However, when you load a binary chunk created from a<br/>
        /// function(see <see href="https://www.lua.org/manual/5.3/manual.html#pdf-string.dump">string.dump</see>), the resulting function can have an arbitrary<br/>
        /// number of upvalues.) All upvalues are fresh, that is, they are not shared<br/>
        /// with any other function.<br/><br/>
        ///
        /// <paramref name="chunkName"/> is used as the name of the chunk for error messages and debug information (see <see href="https://www.lua.org/manual/5.3/manual.html#4.9">§4.9</see>).<br/>
        /// When absent, it defaults to chunk, if chunk is a string, or to "=(load)" otherwise.<br/><br/>
        ///
        /// The string <paramref name="mode"/> controls whether the chunk can be text or binary(that is, a precompiled chunk).<br/>
        /// It may be the string "b" (only binary chunks), "t" (only text chunks), or "bt" (both binary and text).<br/>
        /// The default is "bt".<br/><br/>
        ///
        /// Lua does not check the consistency of binary chunks.Maliciously crafted binary chunks can crash the interpreter.
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="chunkName"></param>
        /// <param name="mode"></param>
        /// <param name="env"></param>
        /// @CSharpLua.Template = "load({0}, {1}, {2}, {3})"
        public static extern dynamic Load(string chunk, string? chunkName, string? mode, object? env);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0})"
        public static extern (bool, dynamic) PCall(Action func);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1})"
        public static extern (bool, dynamic) PCall<T1>(Action<T1> func, T1 arg1);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2})"
        public static extern (bool, dynamic) PCall<T1, T2>(Action<T1, T2> func, T1 arg1, T2 arg2);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3})"
        public static extern (bool, dynamic) PCall<T1, T2, T3>(Action<T1, T2, T3> func, T1 arg1, T2 arg2, T3 arg3);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3}, {4})"
        public static extern (bool, dynamic) PCall<T1, T2, T3, T4>(Action<T1, T2, T3, T4> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3}, {4}, {5})"
        public static extern (bool, dynamic) PCall<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3}, {4}, {5}, {6})"
        public static extern (bool, dynamic) PCall<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})"
        public static extern (bool, dynamic) PCall<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1})"
        public static extern (bool, dynamic) PCall<TOut>(Func<TOut> func);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1})"
        public static extern (bool, dynamic) PCall<T1, TOut>(Func<T1, TOut> func, T1 arg1);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2})"
        public static extern (bool, dynamic) PCall<T1, T2, TOut>(Func<T1, T2, TOut> func, T1 arg1, T2 arg2);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3})"
        public static extern (bool, dynamic) PCall<T1, T2, T3, TOut>(Func<T1, T2, T3, TOut> func, T1 arg1, T2 arg2, T3 arg3);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3}, {4})"
        public static extern (bool, dynamic) PCall<T1, T2, T3, T4, TOut>(Func<T1, T2, T3, T4, TOut> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3}, {4}, {5})"
        public static extern (bool, dynamic) PCall<T1, T2, T3, T4, T5, TOut>(Func<T1, T2, T3, T4, T5, TOut> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3}, {4}, {5}, {6})"
        public static extern (bool, dynamic) PCall<T1, T2, T3, T4, T5, T6, TOut>(Func<T1, T2, T3, T4, T5, T6, TOut> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

        /// <summary>
        /// Calls function <paramref name="func"/> with the given arguments in protected mode. This means that any error inside f is not propagated;<br/>
        /// instead, pcall catches the error and returns a status code. Its first result is the status code (a boolean), which is true<br/>
        /// if the call succeeds without errors. In such case, pcall also returns all results from the call, after this first result.<br/>
        /// In case of any error, pcall returns false plus the error message.
        /// </summary>
        /// <param name="func"></param>
        /// @CSharpLua.Template = "pcall({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})"
        public static extern (bool, dynamic) PCall<T1, T2, T3, T4, T5, T6, T7, TOut>(Func<T1, T2, T3, T4, T5, T6, T7, TOut> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

        /// <summary>
        /// Receives any amount arguments and print them out, also call <see cref="ToString"/> to each ones that not a string
        /// </summary>
        /// <param name="values"></param>
        /// @CSharpLua.Template = "print({*0})"
        public static extern void Print(params dynamic[] values);

        /// <summary>
        /// Checks whether v1 is equal to v2, without invoking the __eq metamethod. Returns a boolean.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "rawequal({0}, {1})"
        public static extern bool RawEqual(object v1, object v2);

        /// <summary>
        /// Gets the real value of table[index], without invoking the __index metamethod.<br/>
        /// table must be a table; index may be any value.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "rawget({0}, {1})"
        public static extern dynamic RawGet(Table table, dynamic index);

        /// <summary>
        /// Sets the real value of <paramref name="table"/>[<paramref name="index"/>] to <paramref name="value"/>, without invoking the __newindex metamethod.<br/>
        /// table must be a table, index any value different from nil and NaN, and value any Lua value.<br/>
        /// This function returns <paramref name="table"/>.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "rawset({0}, {1}, {2})"
        public static extern Table RawSet(Table table, dynamic index, dynamic value);

        /// <summary>
        /// Return the length of the table without invoking it __len metamethod
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "rawlen({0})"
        public static extern int RawLen(Table table);

        /// <summary>
        /// Return the length of the string without invoking __len metamethod
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "rawlen({0})"
        public static extern int RawLen(string str);

        /// <summary>
        /// returns all <paramref name="args"/> after argument number <paramref name="index"/>; a negative number indexes from the end (-1 is the last argument)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "select({0}, {*1})"
        public static extern dynamic Select(int index, params dynamic[] args);

        /// <summary>
        ///  returns the total number of extra arguments it received.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "select({0}, {*1})"
        public static extern dynamic Select(string index, params dynamic[] args);

        /// <summary>
        /// Receives a <paramref name="value"/> of any type and converts it to a string in a human-readable format. (For complete control of how numbers are converted, use string.format.)<br/>
        /// If the metatable of v has a __tostring field, then <see cref="ToString(dynamic)"> calls the corresponding value with ei as argument, and uses the result of the call as its result.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "tostring({0})"
        public static extern string ToString(dynamic value);

        /// <summary>
        /// Tries to convert its argument to a number. If the argument is already a number or a string convertible to a number,<br/>
        /// then returns this number; otherwise, it returns nil.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static extern decimal? ToNumber(dynamic e);

        /// <summary>
        /// <paramref name="e"/> must be a string to be interpreted as an integer numeral in that base.<br/>
        /// The base may be any integer between 2 and 36, inclusive. In bases above 10, the letter 'A' (in either upper or lower case) represents 10, 'B' represents 11, and so forth, with 'Z' representing 35.<br/>
        /// If the string <paramref name="e"/> is not a valid numeral in the given base, the function returns nil.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="numberBase"></param>
        /// <returns></returns>
        public static extern decimal? ToNumber(dynamic e, int numberBase);

        /// <summary>
        /// Get the table metatable if it exist
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "getmetatable({0})"
        public static extern IMetatables? GetMetatable(Table table);

        /// <summary>
        /// Apply <paramref name="metatable"/> property to a <paramref name="table"/>
        /// </summary>
        /// <param name="table"></param>
        /// <param name="metatable"></param>
        /// <returns>The original <paramref name="table"/> after it metatable property has been set</returns>
        /// @CSharpLua.Template = "setmetatable({0}, {1})"
        public static extern Table SetMetatable(Table table, IMetatables metatable);

        /// <summary>
        /// Remove metatable property from the <paramref name="table"/>
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "setmetatable({0}, nil)"
        public static extern Table RemoveMetatable(Table table);

        /// <summary>
        /// Return a string represent it types in lua
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// @CSharpLua.Template = "type({0})"
        public static extern string Type(object obj);

        /// <summary>
        /// Re-present as an 'end' keyword in lua. DO NOT INTERACT WITH THIS
        /// </summary>
        /// @CSharpLua.Template = "end"
        public static readonly object End;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "None")]
        /// @CSharpLua.Ignore
        public interface IMetatables
        {
            dynamic __add(dynamic lhs, dynamic rhs);

            dynamic __sub(dynamic lhs, dynamic rhs);

            dynamic __mul(dynamic lhs, dynamic rhs);

            dynamic __div(dynamic lhs, dynamic rhs);

            dynamic __mod(dynamic lhs, dynamic rhs);

            dynamic __pow(dynamic lhs, dynamic rhs);

            dynamic __unm(dynamic lhs, dynamic rhs);

            dynamic __idiv(dynamic lhs, dynamic rhs);

            dynamic __band(dynamic lhs, dynamic rhs);

            dynamic __bor(dynamic lhs, dynamic rhs);

            dynamic __bxor(dynamic lhs, dynamic rhs);

            dynamic __bnot(dynamic lhs, dynamic rhs);

            dynamic __shl(dynamic lhs, dynamic rhs);

            dynamic __shr(dynamic lhs, dynamic rhs);

            dynamic __concat(dynamic lhs, dynamic rhs);

            int __len(dynamic obj);

            bool __eq(dynamic lhs, dynamic rhs);

            bool __lt(dynamic lhs, dynamic rhs);

            bool __le(dynamic lhs, dynamic rhs);

            dynamic __index(Table tbl, dynamic index);

            void __newindex(Table tbl, dynamic index, dynamic value);

            dynamic __call(params dynamic[] args);
        }

        /// <summary>
        /// A Lua table, this used for type-casting!
        /// </summary>
        /// @CSharpLua.Ignore
        public sealed class Table
        {
            /// <summary>
            /// The table metatable field, some metamethod not exist! so do a null check before execute them
            /// </summary>
            public IMetatables? __metatable;

            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "{}"
            public Table()
            { }

            /// <summary>
            /// Get a specific value from table
            /// </summary>
            /// <param name="key">Any value is accepted by lua</param>
            /// <returns></returns>
            /// @CSharpLua.Template = "{this}[{0}]"
            public extern Tout Get<Tout>(dynamic key);

            /// <summary>
            ///
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// @CSharpLua.Template = "{this}[{0}] = {1}"
            public extern void Set<TVal>(dynamic key, TVal value);

            /// <summary>
            /// Create a lua table
            /// </summary>
            /// <returns></returns>
            /// @CSharpLua.Template = "{}"
            public static extern Table Create();

            /// <summary>
            /// Given a list that contain full of character or number<br/>
            /// returns the string list[<paramref name="i"/>]..<paramref name="separator"/>..list[<paramref name="i"/>+1] ··· <paramref name="separator"/>..list[<paramref name="j"/>].<br/>
            /// The default value for <paramref name="separator"/> is the empty string, the default for <paramref name="i"/> is 1, and the default for <paramref name="j"/> is list length.<br/>
            /// If <paramref name="i"/> is greater than <paramref name="j"/>, returns the empty string.
            /// </summary>
            /// <param name="separator"></param>
            /// <param name="i"></param>
            /// <param name="j"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "table.concat({this}, {0}, {1}, {2})"
            public extern string ConCatenate(string? separator, int? i, int? j);

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            /// @CSharpLua.Template = "table.concat({this})"
            public extern string ConCatenate();

            /// <summary>
            /// Insert <paramref name="value"/> to the end of the table
            /// </summary>
            /// <param name="value"></param>
            /// @CSharpLua.Template = "table.insert({this}, {0})"
            public extern void Insert<TVal>(TVal value);

            /// <summary>
            /// Insert <paramref name="value"/> to the given <paramref name="position"/> index of the table
            /// </summary>
            /// <param name="value"></param>
            /// @CSharpLua.Template = "table.insert({this}, {1}, {2})"
            public extern void Insert<TVal>(int position, TVal value);

            /// <summary>
            /// Return a new Table with all arguments stored in numeric key and a "n" field equal to the total argument passed into this method
            /// </summary>
            /// <param name="args"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "table.pack({*0})"
            public static extern Table Pack(params dynamic[] args);

            /// <summary>
            /// Remove an element from <paramref name="table"/> at <paramref name="index"/> position while also return the removed element value<br/>
            /// This will shift down if the entire <paramref name="table"/> if the removed element is not at the top of the given <paramref name="table"/>
            /// </summary>
            /// <param name="table"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "table.remove({0}, {1})"
            public static extern dynamic Remove(Table table, int? index);

            /// <summary>
            /// Remove an element from table at <paramref name="index"/> position while also return the removed element value<br/>
            /// This will shift down if the entire table if the removed element is not at the top of the given table
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "table.remove({this}, {1})"
            public extern dynamic Remove(int? index);

            /// <summary>
            /// Unpack all element from the given table and return all of it
            /// </summary>
            /// <param name="table"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "table.unpack({0}, {1}, {2})"
            public static extern dynamic Unpack(Table table, int? i, int? j);

            /// <summary>
            /// Typecast the <paramref name="table"/> to the given type <typeparamref name="TOut"/><br/>
            /// This will bypass any protection CSharp.lua make.<br/>
            /// In short doing this just like telling C# this Table is that object without the need to check does it have all required field or other thing
            /// </summary>
            /// <typeparam name="TOut"></typeparam>
            /// <param name="table"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "{0}"
            public static extern TOut Cast<TOut>(Table table);

            /// <summary>
            /// Cast back the <paramref name="obj"/> that you have previously casted the table to
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "{0}"
            public static extern Table Cast<TIn>(TIn obj);

            /// <summary>
            /// Typecast the table to the given type <typeparamref name="TOut"/><br/>
            /// This will bypass any protection CSharp.lua make.<br/>
            /// In short doing this just like telling C# this Table is that object without the need to check does it have all required field or other thing
            /// </summary>
            /// <typeparam name="TOut"></typeparam>
            /// <returns></returns>
            /// @CSharpLua.Template = "{this}"
            public extern TOut Cast<TOut>();

            /// <summary>
            /// Get the length of the table
            /// </summary>
            /// @CSharpLua.Template = "#{this}"
            public readonly int Length;
        }

        /// <summary>
        /// Lua native string table, contain all lua native string method
        /// </summary>
        /// @CSharpLua.Ignore
        public static class String
        {
            /// <summary>
            /// Convert the character given into it internal numeric codes
            /// </summary>
            /// <param name="ch">Single character to be converted</param>
            /// <returns>Numeric representing the passed in character</returns>
            /// @CSharpLua.Template = "string.byte({0})"
            public static extern byte Byte(char ch);

            /// <summary>
            /// Convert the first character in the given string into it internal numeric codes
            /// </summary>
            /// <param name="str">A string to be converted</param>
            /// <returns>Numeric representing the first character of the given string</returns>
            /// @CSharpLua.Template = "string.byte({0})"
            public static extern byte Byte(string str);

            /// <summary>
            /// Convert any character from the given length i to the end of the string
            /// </summary>
            /// <param name="str">A string to be converted</param>
            /// <param name="i">A number range from 1 to the given string length</param>
            /// <returns>An array of numeric representing the converted string</returns>
            /// @CSharpLua.Template = "string.byte({0}, {1})"
            public static extern byte[] Byte(string str, int i);

            /// <summary>
            /// Convert any character from the given <paramref name="i"/> to <paramref name="j"/> in the string
            /// </summary>
            /// <param name="str">A string to be converted</param>
            /// <param name="i">A number range from 1 to the given string length</param>
            /// <param name="j">A number range from i to the given string length</param>
            /// <returns>An array of numeric representing the converted string</returns>
            /// @CSharpLua.Template = "string.byte({0}, {1}, {2})"
            public static extern byte[] Byte(string str, int i, int j);

            /// <summary>
            /// Convert a single numeric byte to it corresponding character
            /// </summary>
            /// <param name="b">A byte value to convert</param>
            /// <returns>A character that is corresponding to the given byte</returns>
            /// @CSharpLua.Template = "string.char({0})"
            public static extern char Char(byte b);

            /// <summary>
            /// Convert given numeric byte to it corresponding character and concatenate them together
            /// </summary>
            /// <param name="b">A byte value to convert</param>
            /// <returns>A string with length equal to the amount of arguments</returns>
            /// @CSharpLua.Template = "string.char({*0})"
            public static extern string Char(params byte[] b);

            /// <summary>
            /// Find and return the first indices match the pattern given from the given string
            /// </summary>
            /// <param name="str">A string to be searched on</param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <returns>The indices of the first match</returns>
            /// @CSharpLua.Template = "string.find({0}, {1})"
            public static extern int Find(string str, string pattern);

            /// <summary>
            /// Find and return the first indices match the pattern given from the given string<br/>
            /// The find begin at given length
            /// </summary>
            /// <param name="str">A string to be searched on</param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="startAt">A number in range of 1 and the maximum given string length</param>
            /// <returns>The indices of the first match</returns>
            /// @CSharpLua.Template = "string.find({0}, {1}, {2})"
            public static extern int Find(string str, string pattern, int startAt);

            /// <summary>
            /// Find and return the first indices match the pattern given from the given string<br/>
            /// The find begin at given length
            /// </summary>
            /// <param name="str">A string to be searched on</param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="startAt">A number </param>
            /// <param name="plain"></param>
            /// <returns>The indices of the first match</returns>
            /// @CSharpLua.Template = "string.find({0}, {1}, {2}, {3})"
            public static extern int Find(string str, string pattern, int startAt, bool plain);

            /// <summary>
            ///
            /// </summary>
            /// <param name="format"></param>
            /// <param name="v"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.format({0}, {*1})"
            public static extern string Format(string format, params string[] v);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <returns>A method that return a string that when call repeated, return a string that match the pattern you defined until it can't found anymore, after then is return null</returns>
            /// @CSharpLua.Template = "string.gmatch({0}, {1})"
            public static extern Func<string?> GMatch(string str, string pattern);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="repl"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.gsub({0}, {1}, {2}, {3})"
            public static extern string GSub(string str, string pattern, string repl, int? n);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="repl"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.gsub({0}, {1}, {2}, {3})"
            public static extern string GSub(string str, string pattern, string repl);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="repl"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.gsub({0}, {1}, {2}, {3})"
            public static extern string GSub(string str, char pattern, string repl, int? n);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="repl"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.gsub({0}, {1}, {2}, {3})"
            public static extern string GSub(string str, char pattern, string repl);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="repl"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.gsub({0}, {1}, {2}, {3})"
            public static extern string GSub(string str, char pattern, char repl, int? n);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="repl"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.gsub({0}, {1}, {2}, {3})"
            public static extern string GSub(string str, char pattern, char repl);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="repl"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.gsub({0}, {1}, {2}, {3})"
            public static extern string GSub(string str, string pattern, char repl, int? n);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="repl"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.gsub({0}, {1}, {2}, {3})"
            public static extern string GSub(string str, string pattern, char repl);

            /// <summary>
            /// Return the length of a string including embedded zero in it
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.len({0})"
            public static extern int Len(string str);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <param name="init"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.match({0}, {1}, {2})"
            public static extern string Match(string str, string pattern, int? init);

            /// <summary>
            ///
            /// </summary>
            /// <param name="str"></param>
            /// <param name="pattern"><see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">Lua pattern</see></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.match({0}, {1})"
            public static extern string Match(string str, string pattern);

            /// <summary>
            /// Pack all given string and return it according to the format string (see <see href="https://www.lua.org/manual/5.3/manual.html#6.4.2">§6.4.2</see>)
            /// </summary>
            /// <param name="format"></param>
            /// <param name="args"></param>
            /// <returns>A binary string contain the packed values of the value passed in, serialized in binary form</returns>
            /// @CSharpLua.Template = "string.pack({0}, {*1})"
            public static extern string Pack(string format, params dynamic[] args);

            /// <summary>
            /// Read back the packed value from <see cref="Pack(string, dynamic[])"/>
            /// </summary>
            /// <param name="format"></param>
            /// <param name="packed"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.unpack({0}, {1})"
            public static extern dynamic UnPack(string format, string packed);

            /// <summary>
            /// Read back the packed value from <see cref="Pack(string, dynamic[])"/>
            /// </summary>
            /// <param name="format"></param>
            /// <param name="packed"></param>
            /// <param name="pos"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.unpack({0}, {1}, {2})"
            public static extern dynamic UnPack(string format, string packed, int? pos);

            /// <summary>
            ///
            /// </summary>
            /// <param name="format"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.packsize({0})"
            public static extern int PackSize(string format);

            /// <summary>
            /// Reverse the string
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.reverse({0})"
            public static extern string Reverse(string str);

            /// <summary>
            /// Substring, that is
            /// </summary>
            /// <param name="str"></param>
            /// <param name="i"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.sub({0}, {1})"
            public static extern string Sub(string str, int i);

            /// <summary>
            /// Substring, that is
            /// </summary>
            /// <param name="str"></param>
            /// <param name="i"></param>
            /// <param name="j"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.sub({0}, {1}, {2})"
            public static extern string Sub(string str, int i, int? j);

            /// <summary>
            /// Uppercase the string
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.upper({0})"
            public static extern string Upper(string str);

            /// <summary>
            /// Returns a string that is the concatenation of n copies of the string s separated by the string sep.<br/>
            /// The default value for sep is the empty string (that is, no separator). Returns the empty string if n is not positive.
            /// </summary>
            /// <param name="str"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.rep({0}, {1})"
            public static extern string Rep(string str, int n);

            /// <summary>
            /// Returns a string that is the concatenation of n copies of the string s separated by the string sep.<br/>
            /// The default value for sep is the empty string (that is, no separator). Returns the empty string if n is not positive.
            /// </summary>
            /// <param name="str"></param>
            /// <param name="n"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "string.rep({0}, {1}, {2})"
            public static extern string Rep(string str, int n, string sep);

        }

        /// @CSharpLua.Ignore
        public static class Utf8
        {
            /// <summary>
            /// The pattern "[\0-\x7F\xC2-\xF4][\x80-\xBF]*" (<see href="https://www.lua.org/manual/5.3/manual.html#6.4.1">§6.4.1</see>),<br/>
            /// which matches exactly one UTF-8 byte sequence, assuming that the subject is a valid UTF-8 string.
            /// </summary>
            /// @CSharpLua.Template = "utf8.charpattern"
            public static readonly string CharPattern;

            /// <summary>
            /// Convert an integer to it corresponding UTF-8 byte sequence
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "utf8.char({0})"
            public static extern char Char(int b);

            /// <summary>
            /// Like <see cref="Char(int)"/> but convert and concat them into string
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "utf8.char({*0})"
            public static extern string Char(params int[] b);

            /// <summary>
            /// Get all of the code point of a string and pack it into a table.
            /// </summary>
            /// <param name="s">A valid UTF-8 string</param>
            /// <returns>Enumerator to enumerate over the code, the value in it are the code point of the character</returns>
            /// @CSharpLua.Template = "table.pack(utf8.codes({0})"
            public static extern void Codes(string s, out Table tbl);

            /// <summary>
            /// Return the lua code point for the given character
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "utf8.codepoint({0})"
            public static extern int CodePoint(char c);

            /// <summary>
            /// Return the table contain all of the code point of the given string
            /// </summary>
            /// <param name="s">A valid UTF-8 string</param>
            /// <returns>You could cast this table into a List and use it</returns>
            /// @CSharpLua.Template = "{ utf8.codepoint({0}, 1, #{0}) }"
            public static extern void CodePoint(string s, out Table tbl);

            /// <summary>
            /// Return the table contain all of the code point of the given string at the given length based on start and end params
            /// </summary>
            /// <param name="s">UTF-8 valid string</param>
            /// <param name="start">The start position of the code point</param>
            /// <param name="end">The end position of the code point</param>
            /// <returns>You could cast this table into a List and use it</returns>
            /// @CSharpLua.Template = "{ utf8.codepoint({0}, {1}, {2}) }"
            public static extern void CodePoint(string s, int start, int end, out Table tbl);

            /// <summary>
            /// Get the number of UTF-8 characters in string s that start between positions i and j (both inclusive).<br/>
            /// The default for i is 1 and for j is -1. If it finds any invalid byte sequence,<br/>
            /// returns a false value plus the position of the first invalid byte.
            /// </summary>
            /// <param name="str">A valid UTF-8 string</param>
            /// <returns>
            /// A table that is first index contain either an integer or boolean based on operation are successful or not<br/>
            /// If not, the first index (1) is a boolean false (or nil) and the second is the position of the character at which the operation failed
            /// </returns>
            /// @CSharpLua.Template = "{ utf8.len({0}, {1}, {2}) }"
            public static extern Table Len(string str, int i = 1, int j = -1);
        }

        /// @CSharpLua.Ignore
        public sealed class Coroutine
        {
            /// <summary>
            ///
            /// </summary>
            /// @CSharpLua.Template = "coroutine.create({0})"
            public Coroutine(Action method)
            { }

            /// <summary>
            ///
            /// </summary>
            /// <param name="f"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "coroutine.create({0})"
            public static extern Coroutine Create(Action f);

            /// <summary>
            ///
            /// </summary>
            /// <param name="coroutine"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "coroutine.isyieldable({0})"
            public static extern bool IsYieldable(Coroutine coroutine);

            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            /// @CSharpLua.Template = "coroutine.isyieldable({this})"
            public extern bool IsYieldable();

            /// <summary>
            /// Get the running coroutine and the extra boolean stating the coroutine is the main one or not
            /// </summary>
            /// <returns></returns>
            /// @CSharpLua.Template = "coroutine.running()"
            public static extern (Coroutine, bool) Running();

            /// <summary>
            /// Resume the <paramref name="coroutine"/> and pass <paramref name="val"/> as the argument of the body function in the <paramref name="coroutine"/><br/>
            /// If the <paramref name="coroutine"/> is yielded, this will restart the <paramref name="coroutine"/> and pass <paramref name="val"/> as the result of the <see cref="Yield"/> method<br/><br/>
            /// If the coroutine runs without any errors, resume returns <see langword="true"/> plus any values passed to yield <br/>
            /// (when the coroutine yields) or any values returned by the body function (when the coroutine terminates). <br/>
            /// If there is any error, resume returns <see langword="false"/> plus the error message.
            /// </summary>
            /// <param name="coroutine"></param>
            /// <remarks>The result is packed in the table, the first index (1) is the <see cref="bool"/> value and then so on</remarks>
            /// @CSharpLua.Template = "table.pack(coroutine.resume({0}, {*1}))"
            public static extern Table Resume(Coroutine coroutine, params dynamic[] val);

            /// <summary>
            /// Resume the coroutine and pass <paramref name="val"/> as the argument of the body function in the coroutine<br/>
            /// If the coroutine is yielded, this will restart the coroutine and pass <paramref name="val"/> as the result of the <see cref="Yield"/> method<br/><br/>
            /// If the coroutine runs without any errors, resume returns <see langword="true"/> plus any values passed to yield <br/>
            /// (when the coroutine yields) or any values returned by the body function (when the coroutine terminates). <br/>
            /// If there is any error, resume returns <see langword="false"/> plus the error message.
            /// </summary>
            /// <remarks>The result is packed in the table, the first index (1) is the <see cref="bool"/> value and then so on</remarks>
            /// @CSharpLua.Template = "table.pack(coroutine.resume({this}, {*0}))"
            public extern Table Resume(params dynamic[] val);

            /// <summary>
            /// Get the status of the <paramref name="coroutine"/>
            /// </summary>
            /// <param name="coroutine"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "coroutine.status({0})"
            public static extern string Status(Coroutine coroutine);

            /// <summary>
            /// Get the status of the coroutine
            /// </summary>
            /// <returns></returns>
            /// @CSharpLua.Template = "coroutine.status({this})"
            public extern string Status();

            /// <summary>
            /// Yield the coroutine, pausing it execution. All argument passed in are the bonus result of the <see cref="Resume(Coroutine, dynamic[])"/> method
            /// </summary>
            /// <param name="yieldResult"></param>
            /// <returns>Value of the argument passed in <see cref="Resume(Coroutine, dynamic[])"/> method</returns>
            /// <remarks>The return value is packed in a table</remarks>
            /// @CSharpLua.Template = "table.pack(coroutine.yield({*0}))"
            public extern Table Yield(params dynamic[] yieldResult);

            /// <summary>
            /// Creates a new coroutine, with body <paramref name="f"/>.<br/>
            /// Returns a function that resumes the coroutine each time it is called.<br/>
            /// Any arguments passed to the function behave as the extra arguments to resume.<br/>
            /// Returns the same values returned by resume, except the first boolean. In case of error, propagates the error.
            /// </summary>
            /// <param name="f"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action<dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action<dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action<dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action<dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action<dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Action<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <summary>
            /// Creates a new coroutine, with body <paramref name="f"/>.<br/>
            /// Returns a function that resumes the coroutine each time it is called.<br/>
            /// Any arguments passed to the function behave as the extra arguments to resume.<br/>
            /// Returns the same values returned by resume, except the first boolean. In case of error, propagates the error.
            /// </summary>
            /// <param name="f"></param>
            /// <returns></returns>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> f);

            /// <inheritdoc/>
            /// @CSharpLua.Template = "coroutine.wrap({0})"
            public static extern Func<Table> Wrap(Func<dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic, dynamic> f);
        }

#pragma warning restore CS8618
#pragma warning restore S4200
#pragma warning restore CS0626
    }
}
