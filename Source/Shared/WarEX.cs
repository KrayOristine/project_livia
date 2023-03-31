using static War3Api.Common;

namespace Source.Shared
{
    public static class WarEX
    {
#pragma warning disable CS0626
#pragma warning disable S4200
        /// @CSharpLua.Template = "BlzGetHeroPrimaryStat({0})"
        public static extern int BlzGetHeroPrimaryStat(unit u);

        /// @CSharpLua.Template = "BlzSetHeroStatEx({0})"
        public static extern void BlzSetHeroStatEx(unit u, int i, int amt);

        /// @CSharpLua.Template = "BlzGetHeroStat({0})"
        public static extern int BlzGetHeroStat(unit u, int i);
#pragma warning restore S4200
#pragma warning restore CS0626


        internal static location zeroLoc = Location(0,0);

        public static readonly boolexpr SafeFilter = Filter(() => true);

        public static string EXStringHash(string str)
        {
            return StringHash(str).ToString("X");
        }

        public static int EXGetHeroPrimary(unit h, bool includeBonus = false)
        {
            int i = BlzGetHeroPrimaryStat(h);
            switch (i)
            {
                case 1: return GetHeroStr(h, includeBonus);
                case 2: return GetHeroInt(h, includeBonus);
                case 3: return GetHeroAgi(h, includeBonus);
                default:
                    Logger.Error("WarEX", "Invalid Primary Stats");
                    return 0;
            }
        }

        public static void EXSetHeroPrimary(unit h, int newValue, bool add = true)
        {
            int i = BlzGetHeroPrimaryStat(h);
            switch (i)
            {
                case 1:
                    if (add) newValue += GetHeroStr(h, false);
                    SetHeroStr(h, newValue, true);
                    break;
                case 2:
                    if (add) newValue += GetHeroAgi(h, false);
                    SetHeroAgi(h, newValue, true);
                    break;
                case 3:
                    if (add) newValue += GetHeroInt(h, false);
                    SetHeroInt(h, newValue, true);
                    break;
                default:
                    Logger.Error("WarEX", "Invalid Primary Stats");
                    break;
            }
        }

        public static float EXGetLocationZ(float xPos, float yPos)
        {
            MoveLocation(zeroLoc, xPos, yPos);
            return GetLocationZ(zeroLoc);
        }

        public static void EXSetUnitZ(unit u, float zPos)
        {
            if (u == null) return;
            SetUnitFlyHeight(u, zPos - EXGetLocationZ(GetUnitX(u), GetUnitY(u)), 0);
        }

        /// <summary>
        /// Asynchronous function
        /// </summary>
        public static float EXGetUnitZ(unit u)
        {
            return EXGetLocationZ(GetUnitX(u), GetUnitY(u)) + GetUnitFlyHeight(u);
        }

        public static void EXGroupEnumUnitInRect(group g, rect r, boolexpr? filter)
        {
            filter ??= SafeFilter;

            GroupEnumUnitsInRect(g, r, filter);
            DestroyBoolExpr(filter);
        }

        public static void EXGroupEnumUnitInRange(group g, float x, float y, float radius, boolexpr? filter)
        {
            filter ??= SafeFilter;

            GroupEnumUnitsInRange(g, x, y, radius, filter);
            DestroyBoolExpr(filter);
        }

        public static void EXGroupEnumUnitInRangeOfLoc(group g, location l, float radius, boolexpr? filter, bool wantDestroy = false)
        {
            float x = l == null ? 0 : GetLocationX(l);
            float y = l == null ? 0 : GetLocationY(l);
            filter ??= SafeFilter;

            GroupEnumUnitsInRange(g, x, y, radius, filter);
            if (wantDestroy) RemoveLocation(l);
            DestroyBoolExpr(filter);
        }

        public static void EXGroupEnumUnitOfPlayer(group g, player p, boolexpr? filter)
        {
            filter ??= SafeFilter;

            GroupEnumUnitsOfPlayer(g, p, filter);
            DestroyBoolExpr(filter);
        }

        public static void EXGroupEnumUnitSelected(group g, player p, boolexpr? filter)
        {
            filter ??= SafeFilter;

            GroupEnumUnitsSelected(g, p, filter);
            DestroyBoolExpr(filter);
        }
    }
}
