// ------------------------------------------------------------------------------
// <copyright file="BinaryWriter.cs" company="Kray Oristine">
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
// </copyright>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.Shared
{
    public sealed class BinaryWriter
    {
        private Lua.Table format;
        private Lua.Table values;

        public BinaryWriter()
        {
            format = new();
            values = new();
            format.Push(">");
        }

        public void Clear()
        {
            format = new();
            values = new();
            format.Push(">");
        }

        public void Write(string value)
        {
            format.Push("z");
            values.Push(value);
        }

        public void Write(int value)
        {
            format.Push("i4");
            values.Push(value);
        }

        public void Write(float value)
        {
            format.Push("f");
            values.Push(value);
        }

        public void Write(double value)
        {
            format.Push("d");
            values.Push(value);
        }

        public void Write(short value)
        {
            format.Push("h");
            values.Push(value);
        }

        public void Write(sbyte value)
        {
            format.Push("b");
            values.Push(value);
        }

        public void Write(ushort value)
        {
            format.Push("H");
            values.Push(value);
        }

        public void Write(uint value)
        {
            format.Push("I4");
            values.Push(value);
        }

        public void Write(byte value)
        {
            format.Push("B");
            values.Push(value);
        }

        public override string ToString()
        {
            return Lua.String.Pack(format.ConCatenate(), values.Unpack());
        }
    }
}
