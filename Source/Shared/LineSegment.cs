// ------------------------------------------------------------------------------
// <copyright file="LineSegment.cs" company="Kray Oristine">
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
using static War3Api.Blizzard;
using static War3Api.Common;

namespace Source.Shared
{
    public static class LineSegment
    {
        private static readonly rect r = Rect(0, 0, 0, 0);
        private static readonly group g = CreateGroup();

        private static float ox = 0f;
        private static float oy = 0f;
        private static float da = 0f;
        private static float db = 0f;
        private static float ui = 0f;
        private static float uj = 0f;

        private static void Prepare(float ax, float ay, float bx, float by, float offset)
        {
            // Get center coordinates of the rectangle
            ox = 0.5f * (ax + bx);
            oy = 0.5f * (ay + by);

            // Get rectangle major axis as vector
            float dx = 0.5f * (bx - ax);
            float dy = 0.5f * (by - ay);

            // Get half of the rectangle length and height
            da = Lua.Math.Sqrt(dx * dx + dy * dy);
            db = offset;

            // Get unit vector of the major axis
            ui = dx / da;
            uj = dy / da;

            // Prep bounding rectangle
            float xn, xx, yn, yx;
            if (ax > bx)
            {
                xn = bx;
                xx = ax;
            }
            else
            {
                xn = ax;
                xx = bx;
            }
            if (ay > by)
            {
                yn = by;
                yx = ay;
            }
            else
            {
                yn = ay;
                yx = by;
            }

            SetRect(r, xn - offset, yn - offset, xx + offset, yx + offset);
        }

        private static bool ContainWidget(widget w, float offset)
        {
            float wdx = GetWidgetX(w) - ox;
            float wdy = GetWidgetY(w) - oy;
            float dx = wdx * ui + wdy * uj;
            float dy = wdx * -uj + wdy * ui;
            da += offset;
            db += offset;

            return dx * dx <= da * da && dy * dy <= db * db;
        }

        private static readonly List<widget> lastEnum = new();

        public static List<widget> LastEnum { get => lastEnum; }

        public static List<widget> EnumUnit(float minX, float maxX, float minY, float maxY, float offset, bool checkCollision = false)
        {
            Prepare(minX, minY, maxX, maxY, offset);
            WarEX.EXGroupEnumUnitInRect(g, r, null);
            lastEnum.Clear();

            for (int i = 0; i < BlzGroupGetSize(g); i++)
            {
                unit u = BlzGroupUnitAt(g, i);
                if (ContainWidget(u, checkCollision ? BlzGetUnitCollisionSize(u) : 0.0f))
                {
                    lastEnum.Add(u);
                }
            }
            GroupClear(g);

            return lastEnum;
        }

        private static bool DestructableFilter()
        {
            destructable d = GetFilterDestructable();
            if (ContainWidget(d, 0.0f)) lastEnum.Add(d);
            return false;
        }

        public static List<widget> EnumDestructable(float minX, float maxX, float minY, float maxY, float offset, bool checkCollision = false)
        {
            Prepare(minX, minY, maxX, maxY, offset);
            lastEnum.Clear();

            var f = Filter(DestructableFilter);
            EnumDestructablesInRect(r, f, DoNothing);
            DestroyBoolExpr(f);

            return lastEnum;
        }

        private static bool ItemFilter()
        {
            item itm = GetFilterItem();
            if (ContainWidget(itm, 0.0f)) lastEnum.Add(itm);
            return false;
        }

        public static List<widget> EnumItem(float minX, float maxX, float minY, float maxY, float offset, bool checkCollision = false)
        {
            Prepare(minX, minY, maxX, maxY, offset);
            lastEnum.Clear();

            var f = Filter(ItemFilter);
            EnumItemsInRect(r, f, DoNothing);
            DestroyBoolExpr(f);

            return lastEnum;
        }
    }
}
