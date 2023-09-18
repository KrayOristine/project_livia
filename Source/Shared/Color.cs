// ------------------------------------------------------------------------------
// <copyright file="Color.cs" company="Kray Oristine">
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

// Ignore Spelling: lhs rhs argb

using System;
using System.Text;

namespace Source.Shared
{
    /// <summary>
    /// Alternative for C# GameSystem.Drawing Color class<br/>
    ///
    /// Will try the best way to replicate the way Color works<br/>
    ///
    /// Version: 1.0<br/>
    /// </summary>
    public sealed class Color : IEquatable<Color>
    {
        private readonly byte alpha;
        private readonly byte red;
        private readonly byte green;
        private readonly byte blue;

        /// <summary>
        /// Create this class and bypass argument checking (if you know what you're doing)
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Color(byte alpha, byte red, byte green, byte blue)
        {
            this.alpha = alpha;
            this.red = red;
            this.blue = blue;
            this.green = green;
        }

        /// <summary>
        /// Alpha value of the Color
        /// </summary>
        public byte A { get => alpha; }

        /// <summary>
        /// Red value of the Color
        /// </summary>
        public byte R { get => red; }

        /// <summary>
        /// Green value of the Color
        /// </summary>
        public byte G { get => green; }

        /// <summary>
        /// Blue value of the color
        /// </summary>
        public byte B { get => blue; }

        /// <summary>
        /// Create a color class with matching given color ranges
        /// </summary>
        /// <param name="alpha">Color alpha value</param>
        /// <param name="red">Color red value</param>
        /// <param name="green">Color green value</param>
        /// <param name="blue">Color blue value</param>
        /// <returns>A new color object</returns>
        /// <exception cref="ArgumentException">If any passed args are larger than 255 or smaller than 0</exception>
        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            if (alpha < 0 || alpha > 255) throw new ArgumentException("Alpha parameters out of valid color ranges");
            if (red < 0 || red > 255) throw new ArgumentException("Red parameters out of valid color ranges");
            if (green < 0 || green > 255) throw new ArgumentException("Green parameters out of valid color ranges");
            if (blue < 0 || blue > 255) throw new ArgumentException("Blue parameters out of valid color ranges");

            return new((byte)alpha, (byte)red, (byte)green, (byte)blue);
        }

        /// <summary>
        /// Create a color class with matching given color ranges
        /// </summary>
        /// <param name="red">Color red value</param>
        /// <param name="green">Color green value</param>
        /// <param name="blue">Color blue value</param>
        /// <returns>A new color object</returns>
        /// <exception cref="ArgumentException">If any passed args are larger than 255 or smaller than 0</exception>
        public static Color FromArgb(int red, int green, int blue)
        {
            if (red < 0 || red > 255) throw new ArgumentException("Red parameters out of valid color ranges");
            if (green < 0 || green > 255) throw new ArgumentException("Green parameters out of valid color ranges");
            if (blue < 0 || blue > 255) throw new ArgumentException("Blue parameters out of valid color ranges");

            return new(0, (byte)red, (byte)green, (byte)blue);
        }

        /// <summary>
        /// Create a color class from a baseColor with a new alpha
        /// </summary>
        /// <param name="alpha">Color alpha value</param>
        /// <param name="baseColor">Base <see cref="Color"/> class</param>
        /// <returns>A new color object</returns>
        /// <exception cref="ArgumentException">If any passed args are larger than 255 or smaller than 0</exception>
        public static Color FromArgb(int alpha, Color baseColor)
        {
            if (alpha < 0 || alpha > 255) throw new ArgumentException("Alpha parameters out of valid color ranges");

            return new((byte)alpha, baseColor.red, baseColor.green, baseColor.blue);
        }

        /// <summary>
        /// Create a color class with matching given color ranges
        /// </summary>
        /// <param name="argb">A hexadecimal value re-presenting color ranges for alpha, red, green, blue</param>
        /// <returns>A new color object</returns>
        public static Color FromArgb(int argb)
        {
            byte a = (byte)((argb & 0xFF000000) >> 24);
            byte r = (byte)((argb & 0x00FF0000) >> 16);
            byte g = (byte)((argb & 0x0000FF00) >> 8);
            byte b = (byte)(argb & 0x000000FF);

            return new(a, r, g, b);
        }

        /// <summary>
        /// Get back the int re-presentation of the ARGB color ranges
        /// </summary>
        /// <returns>A number re-presenting ARGB color ranges</returns>
        public int ToArgb()
        {
            return (alpha << 24 | red << 16 | green << 8 | blue);
        }

        /// <summary>
        /// Get a number re-presenting the ARGB color ranges that match the given parameters
        /// </summary>
        /// <param name="alpha">Color alpha value</param>
        /// <param name="red">Color red value</param>
        /// <param name="green">Color green value</param>
        /// <param name="blue">Color blue value</param>
        /// <returns>A number re-presenting ARGB color ranges</returns>
        public static int ToArgb(int alpha, int red, int green, int blue)
        {
            return (alpha << 24 | red << 16 | green << 8 | blue);
        }

        /// <summary>
        /// Get back the hexadecimal re-presentation of the ARGB color ranges
        /// </summary>
        /// <returns>A hex string re-present ARGB color ranges</returns>
        public string ToArgbString()
        {
            return $"{alpha:X2}{red:X2}{green:X2}{blue:X2}";
        }

        /// <summary>
        /// Get a hex string re-presenting the ARGB color ranges that match the given parameters
        /// </summary>
        /// <param name="alpha">Color alpha value</param>
        /// <param name="red">Color red value</param>
        /// <param name="green">Color green value</param>
        /// <param name="blue">Color blue value</param>
        /// <returns>A string re-presenting ARGB color ranges in hex</returns>
        public static string ToArgbString(int alpha, int red, int green, int blue)
        {
            return $"{alpha:X2}{red:X2}{green:X2}{blue:X2}";
        }

        /// <summary>
        /// Get a warcraft color codes string which look like this
        /// <code>|cAARRGGBB</code>
        /// </summary>
        /// <param name="alpha">Color alpha value</param>
        /// <param name="red">Color red value</param>
        /// <param name="green">Color green value</param>
        /// <param name="blue">Color blue value</param>
        /// <returns>A warcraft color codes</returns>
        public string ToWarcraftColor()
        {
            return $"|c{alpha:X2}{red:X2}{green:X2}{blue:X2}";
        }

        /// <summary>
        /// Get a warcraft color codes string which look like this
        /// <code>|cAARRGGBB</code>
        /// </summary>
        /// <param name="alpha">Color alpha value</param>
        /// <param name="red">Color red value</param>
        /// <param name="green">Color green value</param>
        /// <param name="blue">Color blue value</param>
        /// <returns>A warcraft color codes</returns>
        public static string ToWarcraftColor(int alpha, int red, int green, int blue)
        {
            return $"|c{alpha:X2}{red:X2}{green:X2}{blue:X2}";
        }

        /// <summary>
        /// Generate a gradient based on input value
        /// </summary>
        /// <param name="length"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>An array of gradient color as <see cref="Color"/> object</returns>
        public static Color[] GenerateGradient(int length, Color start, Color end)
        {
            if (length < 0) throw new ArgumentException("Invalid length provided to GenerateGradient", nameof(length));
            int startA = start.A;
            int startR = start.R;
            int startG = start.G;
            int startB = start.B;
            int stepA = (end.A - startA) / (length - 1);
            int stepR = (end.R - startR) / (length - 1);
            int stepG = (end.G - startG) / (length - 1);
            int stepB = (end.B - startB) / (length - 1);
            var colorList = new Color[length];
            for (int i = 0; i < length; i++)
            {
                colorList[i] = FromArgb(startA + (stepA * i), startR + (stepR * i), startG + (stepG * i), stepB + (stepB * i));
            }

            return colorList;
        }

        /// <summary>
        /// Apply a gradient list
        /// </summary>
        /// <param name="str">A base string to apply to</param>
        /// <param name="gradient">A list of <see cref="Color"/> object</param>
        /// <returns>A warcraft colorized string based on given list<br/> Character that exceed the gradient list length are truncated</returns>
        public static string ApplyGradient(string str, Color[] gradient)
        {
            if (gradient.Length == 0) throw new ArgumentException("Invalid gradient list");
            if (string.IsNullOrEmpty(str)) throw new ArgumentException("Invalid base string");
            StringBuilder result = new();
            for (int i = 0; i  < gradient.Length; i++)
            {
                string colorHex = gradient[i].ToArgbString();
                result.Append("|c").Append(colorHex).Append(str[i]).Append("|r");
            }

            return result.ToString();
        }

        /// <summary>
        /// Generate and apply gradient directly instead of return a list of <see cref="Color"/> object
        /// </summary>
        /// <param name="str">A base string to apply</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>A warcraft colorized string based on the result of gradient calculation</returns>
        public static string GenerateApplyGradient(string str, Color start, Color end)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str), "Invalid base string provided");
            int length = str.Length;
            int startA = start.A;
            int startR = start.R;
            int startG = start.G;
            int startB = start.B;
            int stepA = (end.A - startA) / (length - 1);
            int stepR = (end.R - startR) / (length - 1);
            int stepG = (end.G - startG) / (length - 1);
            int stepB = (end.B - startB) / (length - 1);
            StringBuilder result = new();
            for (int i = 0; i < length; i++)
            {
                string colorHex = ToArgbString(startA + (stepA * i), startR + (stepR * i), startG + (stepG * i), stepB + (stepB * i));
                result.Append("|c").Append(colorHex).Append(str[i]).Append("|r");
            }

            return result.ToString();
        }

        /// <summary>
        /// Check equality of the current compared to target Color object
        /// </summary>
        /// <param name="target">Target of the compare methods</param>
        /// <returns>Is the target match?</returns>
        public bool Equals(Color? target)
        {
            if (target == null) return false;

            if (alpha != target.alpha) return false;
            else if (red != target.red) return false;
            else if (green != target.green) return false;
            else if (blue != target.blue) return false;

            return true;
        }

        /// <summary>
        /// Check equality of an object to see is it a Color and match this object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as Color);
        }

        /// <summary>
        /// Get hash code based on alpha, red, green and blue value
        /// </summary>
        /// <returns>Hash result of alpha, red, green, blue value</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(alpha, red, green, blue);
        }

        public static bool operator ==(Color? lhs, Color? rhs)
        {
            return lhs switch
            {
                null => false,
                _ => lhs.Equals(rhs),
            };
        }

        public static bool operator !=(Color? lhs, Color? rhs)
        {
            return lhs switch
            {
                null => true,
                _ => !lhs.Equals(rhs),
            };
        }
    }
}
