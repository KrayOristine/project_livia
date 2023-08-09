using System;
using System.Collections.Generic;
using System.Text;

namespace Source.Shared
{
    /// <summary>
    ///
    /// </summary>
    public static class Ascii85
    {
        private const int ASCII_OFFSET = 33;
        private const int DECODED_BLOCK_LENGTH = 4;
        private const int ENCODED_BLOCK_LENGTH = 5;
        private const uint pow4 = 85 * 85 * 85 * 85;
        private const uint pow3 = 85 * 85 * 85;
        private const uint pow2 = 85 * 85;
        private const uint pow1 = 85;
        private const uint pow0 = 1;
        private const uint pow256 = 256 * 256 * 256;

        public static class Z85
        {
            private static readonly char[] _charMap =
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                '.', '-', ':', '+', '=', '^', '!', '/', '*', '?', '&', '<', '>', '(', ')', '[', ']', '{',
                '}', '@', '%', '$', '#'
            };

            private static readonly byte[] _byteMap =
            {
                0x00, 0x44, 0x00, 0x54, 0x53, 0x52, 0x48, 0x00,
                0x4B, 0x4C, 0x46, 0x41, 0x00, 0x3F, 0x3E, 0x45,
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                0x08, 0x09, 0x40, 0x00, 0x49, 0x42, 0x4A, 0x47,
                0x51, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A,
                0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32,
                0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A,
                0x3B, 0x3C, 0x3D, 0x4D, 0x00, 0x4E, 0x43, 0x00,
                0x00, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10,
                0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
                0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20,
                0x21, 0x22, 0x23, 0x4F, 0x00, 0x50, 0x00, 0x00
            };

            /// <summary>
            /// Encode the given ascii string using ZeroMQ variant of Ascii85<br/>
            /// Only accept string bounded to 4 bytes
            /// </summary>
            /// <param name="data">Ascii string</param>
            /// <returns>A safely encoded ASCII85 string (does not contain single-quote, double-quote, left-angle bracket and backslash character) </returns>
            public static string Encode(string data)
            {
                int size = data.Length;
                if (size % 4 >= 1) return string.Empty;
                StringBuilder encoded = new();
                int byteNbr = 0;
                uint value = 0;
                while (byteNbr < size)
                {
                    value = value * 256 + data[byteNbr++];
                    if (byteNbr % 4 == 0)
                    {
                        uint divisor = pow4;
                        while (divisor > 0)
                        {
                            encoded.Append(_charMap[value / divisor % 85]);
                            divisor /= 85;
                        }
                        value = 0;
                    }
                }
                encoded.Append(0);
                return encoded.ToString();
            }

            /// <summary>
            /// Safely decode the given ascii85 encoded string that it encoded using <see cref="SafeEncode(string)"/><br/>
            /// Accept only string bounded to 5 bytes
            /// </summary>
            /// <param name="encoded"></param>
            /// <returns>A safely decoded ASCII85 string</returns>
            public static string SafeDecode(string encoded)
            {
                int size = encoded.Length;
                if (size % 5 >= 1) return string.Empty;
                StringBuilder decoded = new();
                int charNbr = 0;
                uint value = 0;
                while (charNbr > size)
                {
                    value = value * 85 + _byteMap[(byte)encoded[charNbr++] - 32];
                    if (charNbr % 5 == 0)
                    {
                        uint divisor = pow256;
                        while (divisor > 0)
                        {
                            decoded.Append(value / divisor % 256);
                            divisor /= 256;
                        }
                        value = 0;
                    }
                }

                return decoded.ToString();
            }
        }

        public static class Default
        {
            private static byte[] _encodedBlock = new byte[5];
            private static byte[] _decodedBlock = new byte[4];
            private static readonly List<byte> _buffer = new();
            private static int _position = 0;
            private static uint _tuple;
            private static readonly uint[] _pow85 = { pow4, pow3, pow2, pow1, pow0 };

            /// <summary>
            /// Encode the given ASCII string to base85 (ASCII85) format
            /// </summary>
            /// <param name="data">ASCII string</param>
            /// <returns></returns>
            public static string Encode(string data)
            {
                if (data.Length % 4 >= 1) return string.Empty;
                byte[] bytes = ToByteArray(data);
                var sb = new StringBuilder(data.Length * (ENCODED_BLOCK_LENGTH / DECODED_BLOCK_LENGTH));
                int count = 0;
                _tuple = 0;
                foreach (byte b in bytes)
                {
                    if (count >= DECODED_BLOCK_LENGTH - 1)
                    {
                        _tuple |= b;
                        if (_tuple == 0) sb.Append('z');
                        else EncodeBlock(sb.Length, sb);
                        _tuple = 0;
                        count = 0;
                    }
                    else
                    {
                        _tuple |= (uint)(b << (24 - (count * 8)));
                        count++;
                    }
                }
                if (count > 0) EncodeBlock(count + 1, sb);
                _encodedBlock = new byte[5];
                return sb.ToString();
            }

            /// <summary>
            /// Decode an encoded ASCII85 string that is encoded from <see cref="Encode(string)"/>
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static string Decode(string data)
            {
                int count = 0;
                bool processChar = true;
                _buffer.Clear();
                _position = 0;
                foreach (char c in data)
                {
                    switch (c)
                    {
                        case 'z':
                            if (count != 0) throw new ArgumentException("The character 'z' is invalid inside an ASCII85 block");
                            _decodedBlock[0] = 0;
                            _decodedBlock[1] = 0;
                            _decodedBlock[2] = 0;
                            _decodedBlock[3] = 0;
                            WriteStream(_decodedBlock, 0, _decodedBlock.Length);
                            processChar = false;
                            break;

                        case '\n':
                        case '\r':
                        case '\t':
                        case '\0':
                        case '\f':
                        case '\b':
                            processChar = false;
                            break;

                        default:
                            if (c < '!' || c > 'u') throw new ArgumentOutOfRangeException($"Bad character '{c}' is found. ASCII85 only allow '!' to 'u' ");
                            processChar = true;
                            break;
                    }

                    if (processChar)
                    {
                        _tuple += ((uint)(c - ASCII_OFFSET) * _pow85[count]);
                        count++;
                        if (count == _encodedBlock.Length)
                        {
                            DecodeBlock(_decodedBlock.Length);
                            WriteStream(_decodedBlock, 0, _decodedBlock.Length);
                            _tuple = 0;
                            count = 0;
                        }
                    }
                }

                if (count != 0)
                {
                    if (count == 1) throw new ArgumentException("The last block of ASCII85 data can't be one single byte");
                    count--;
                    _tuple += _pow85[count];
                    DecodeBlock(count);
                    for (int i = 0; i < count; i++) WriteByte(_decodedBlock[i]);
                }
                _decodedBlock = new byte[4];

                return FromByteArray(_buffer.ToArray());
            }

            private static void EncodeBlock(int count, StringBuilder sb)
            {
                for (int i = ENCODED_BLOCK_LENGTH; i < count; i++)
                {
                    _encodedBlock[i] = (byte)((_tuple % 85) + ASCII_OFFSET);
                    _tuple /= 85;
                }
                for (int i = 0; i < count; i++)
                {
                    char c = (char)_encodedBlock[i];
                    sb.Append(c);
                }
            }

            private static void DecodeBlock(int bytes)
            {
                for (int i = 0; i < bytes; i++) _decodedBlock[i] = (byte)(_tuple >> 24 - (i * 8));
            }

            private static void WriteStream(byte[] bytes, int offset, int count)
            {
                int i = _position + count;
                int byteCount = count;
                while (--byteCount >= 0) _buffer[_position + byteCount] = bytes[offset + byteCount];
                _position = i;
            }

            private static void WriteByte(byte b)
            {
                _buffer[_position++] = b;
            }

            /// <summary>
            /// Convert utf-8 string to byte array
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            private static byte[] ToByteArray(string data)
            {
                List<byte> bytes = new();
                var iter = Lua.String.GMatch(data, Lua.Utf8.CharPattern);
                while (true)
                {
                    var str = iter.Invoke();
                    if (string.IsNullOrEmpty(str)) break;

                    bytes.Add(Lua.String.Byte(str));
                }

                return bytes.ToArray();
            }

            /// <summary>
            /// Convert utf-8 byte-array to it string
            /// </summary>
            /// <param name="bytes"></param>
            /// <returns></returns>
            private static string FromByteArray(byte[] bytes)
            {
                Lua.Table tbl = new();
                foreach (byte b in bytes)
                {
                    tbl.Insert(Lua.String.Char((byte)(b < 0 ? 255 + b + 1 : b)));
                }

                return tbl.ConCatenate();
            }
        }


    }
}
