// ------------------------------------------------------------------------------
// <copyright file="WarEX.cs" company="Kray Oristine">
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
using War3Api;

namespace Source.Shared
{
    public static class WarEX
    {

        internal static Common.location zeroLoc = Common.Location(0,0);

        public static readonly Common.boolexpr SafeFilter = Common.Filter(() => true);

        public static string StringHash(string str)
        {
            return Common.StringHash(str).ToString("X");
        }

        public static float GetLocZ(float xPos, float yPos)
        {
            Common.MoveLocation(zeroLoc, xPos, yPos);
            return Common.GetLocationZ(zeroLoc);
        }

        public static void SetUnitZ(Common.unit u, float zPos)
        {
            if (u == null) return;
            Common.SetUnitFlyHeight(u, zPos - GetLocZ(Common.GetUnitX(u), Common.GetUnitY(u)), 0);
        }

        /// <summary>
        /// Asynchronous function
        /// </summary>
        public static float GetUnitZ(Common.unit u)
        {
            return GetLocZ(Common.GetUnitX(u), Common.GetUnitY(u)) + Common.GetUnitFlyHeight(u);
        }

        public static void EXGroupEnumUnitInRect(Common.group g, Common.rect r, Common.boolexpr? filter)
        {
            filter ??= SafeFilter;

            Common.GroupEnumUnitsInRect(g, r, filter);
        }

        public static void EXGroupEnumUnitInRange(Common.group g, float x, float y, float radius, Common.boolexpr? filter = null)
        {
            filter ??= SafeFilter;

            Common.GroupEnumUnitsInRange(g, x, y, radius, filter);
        }

        public static void EXGroupEnumUnitInRangeOfLoc(Common.group g, Common.location l, float radius, Common.boolexpr? filter = null, bool wantDestroy = false)
        {
            float x = l == null ? 0 : Common.GetLocationX(l);
            float y = l == null ? 0 : Common.GetLocationY(l);
            filter ??= SafeFilter;

            Common.GroupEnumUnitsInRange(g, x, y, radius, filter);
            if (wantDestroy) Common.RemoveLocation(l);
        }

        public static void EXGroupEnumUnitOfPlayer(Common.group g, Common.player p, Common.boolexpr? filter = null)
        {
            filter ??= SafeFilter;

            Common.GroupEnumUnitsOfPlayer(g, p, filter);
        }

        public static void EXGroupEnumUnitSelected(Common.group g, Common.player p, Common.boolexpr? filter = null)
        {
            filter ??= SafeFilter;

            Common.GroupEnumUnitsSelected(g, p, filter);
        }
    }
}
