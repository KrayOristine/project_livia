// ------------------------------------------------------------------------------
// <copyright file="ExtraMathh.cs" company="Kray Oristine">
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
using System.Runtime.CompilerServices;

namespace Source.Shared
{
    /// <summary>
    /// Reader classes for reading binary string
    /// </summary>
    public sealed class BinaryReader
    {
        private readonly string _binaryData;
        private int _position;

        /// <summary>
        /// Initialize a new binary reader
        /// </summary>
        /// <param name="data">Binary string data</param>
        public BinaryReader(string data)
        {
            _binaryData = data;
            _position = 1;
        }

        private dynamic Read(string format, int size)
        {
            Lua.Table unpacked = Lua.String.UnPack(format, _binaryData, _position);
            _position += size;
            if (unpacked.Length <= 0) return 0;

            return unpacked.Get<dynamic>(0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ReadDouble() => Read(">d", 4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadFloat() => Read(">f", 4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int16 ReadInt16() => Read(">h", 2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32() => Read(">i4", 4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadInt8() => Read(">b", 1);

        public string ReadString()
        {
            var value = Read(">z", 0);
            _position += value.Length + 1;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt16 ReadUInt16() => Read(">H", 2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32() => Read(">I4", 4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt8() => Read(">B", 1);
    }
}
