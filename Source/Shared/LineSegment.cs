using System;
using System.Collections.Generic;
using static War3Api.Blizzard;
using static War3Api.Common;

namespace Source.Shared
{
    public static class LineSegment
    {
        internal static rect r = Rect(0, 0, 0, 0);
        internal static group g = CreateGroup();

        internal static float ox = 0f;
        internal static float oy = 0f;
        internal static float dx = 0f;
        internal static float dy = 0f;
        internal static float da = 0f;
        internal static float db = 0f;
        internal static float ui = 0f;
        internal static float uj = 0f;
        internal static float wdx = 0f;
        internal static float wdy = 0f;

        internal static void Prepare(float ax, float ay, float bx, float by, float offset)
        {
            // Get center coordinates of the rectangle
            ox = 0.5f * (ax + bx);
            oy = 0.5f * (ay + by);

            // Get rectangle major axis as vector
            dx = 0.5f * (bx - ax);
            dy = 0.5f * (by - ay);

            // Get half of the rectangle length and height
            da = (float)Math.Sqrt(dx * dx + dy * dy);
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

        internal static bool ContainWidget(widget w, float offset)
        {
            wdx = GetWidgetX(w) - ox;
            wdy = GetWidgetY(w) - oy;
            dx = wdx * ui + wdy * uj;
            dy = wdx * -uj + wdy * ui;
            da += offset;
            db += offset;

            return dx * dx <= da * da && dy * dy <= db * db;
        }

        internal static readonly List<widget> lastEnums = new();
        internal static bool enumUsed = false;

        public static List<widget> LastEnums { get => lastEnums; }

        public static List<widget> EnumsUnit(float minX, float maxX, float minY, float maxY, float offset, bool checkCollision = false)
        {

            Prepare(minX, minY, maxX, maxY, offset);
            WarEX.EXGroupEnumUnitInRect(g, r, null);
            lastEnums.Clear();

            for (int i = 0; i < BlzGroupGetSize(g); i++)
            {
                unit u = BlzGroupUnitAt(g, i);
                if (ContainWidget(u, checkCollision ? BlzGetUnitCollisionSize(u) : 0.0f))
                {
                    lastEnums.Add(u);
                }
            }
            GroupClear(g);

            return lastEnums;
        }

        internal static bool DestructableFilter()
        {
            destructable d = GetFilterDestructable();
            if (ContainWidget(d, 0.0f)) lastEnums.Add(d);
            return false;
        }

        public static List<widget> EnumsDestructable(float minX, float maxX, float minY, float maxY, float offset, bool checkCollision = false)
        {
            Prepare(minX, minY, maxX, maxY, offset);
            lastEnums.Clear();

            var f = Filter(DestructableFilter);
            EnumDestructablesInRect(r, f, DoNothing);
            DestroyBoolExpr(f);

            return lastEnums;
        }

        internal static bool ItemFilter()
        {
            item itm = GetFilterItem();
            if (ContainWidget(itm, 0.0f)) lastEnums.Add(itm);
            return false;
        }

        public static List<widget> EnumsItem(float minX, float maxX, float minY, float maxY, float offset, bool checkCollision = false)
        {
            Prepare(minX, minY, maxX, maxY, offset);
            lastEnums.Clear();

            var f = Filter(ItemFilter);
            EnumItemsInRect(r, f, DoNothing);
            DestroyBoolExpr(f);

            return lastEnums;
        }
    }
}
