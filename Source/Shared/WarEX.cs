using War3Api;

namespace Source.Shared
{
    public static class WarEX
    {
#pragma warning disable CS0626
#pragma warning disable S4200
        /// @CSharpLua.Template = "BlzGetHeroPrimaryStat({0})"
        public static extern int BlzGetHeroPrimaryStat(Common.unit u);

        /// @CSharpLua.Template = "BlzSetHeroStatEx({0}, {1}, {2})"
        public static extern void BlzSetHeroStatEx(Common.unit u, int i, int amt);

        /// @CSharpLua.Template = "BlzGetHeroStat({0}, {1})"
        public static extern int BlzGetHeroStat(Common.unit u, int i);
#pragma warning restore S4200
#pragma warning restore CS0626


        internal static Common.location zeroLoc = Common.Location(0,0);

        public static readonly Common.boolexpr SafeFilter = Common.Filter(() => true);

        public static string StringHash(string str)
        {
            return Common.StringHash(str).ToString("X");
        }

        public static int GetHeroPrimary(Common.unit h, bool includeBonus = false)
        {
            int i = BlzGetHeroPrimaryStat(h);
            switch (i)
            {
                case 1: return Common.GetHeroStr(h, includeBonus);
                case 2: return Common.GetHeroInt(h, includeBonus);
                case 3: return Common.GetHeroAgi(h, includeBonus);
                default:
                    Logger.Error("WarEX", "Invalid Primary Stats");
                    return 0;
            }
        }

        public static void SetHeroPrimary(Common.unit h, int newValue, bool add = true)
        {
            int i = BlzGetHeroPrimaryStat(h);
            switch (i)
            {
                case 1:
                    if (add) newValue += Common.GetHeroStr(h, false);
                    Common.SetHeroStr(h, newValue, true);
                    break;
                case 2:
                    if (add) newValue += Common.GetHeroAgi(h, false);
                    Common.SetHeroAgi(h, newValue, true);
                    break;
                case 3:
                    if (add) newValue += Common.GetHeroInt(h, false);
                    Common.SetHeroInt(h, newValue, true);
                    break;
                default:
                    Logger.Error("WarEX", "Invalid Primary Stats");
                    break;
            }
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

        public static void EXGroupEnumUnitInRange(Common.group g, float x, float y, float radius, Common.boolexpr? filter)
        {
            filter ??= SafeFilter;

            Common.GroupEnumUnitsInRange(g, x, y, radius, filter);
        }

        public static void EXGroupEnumUnitInRangeOfLoc(Common.group g, Common.location l, float radius, Common.boolexpr? filter, bool wantDestroy = false)
        {
            float x = l == null ? 0 : Common.GetLocationX(l);
            float y = l == null ? 0 : Common.GetLocationY(l);
            filter ??= SafeFilter;

            Common.GroupEnumUnitsInRange(g, x, y, radius, filter);
            if (wantDestroy) Common.RemoveLocation(l);
        }

        public static void EXGroupEnumUnitOfPlayer(Common.group g, Common.player p, Common.boolexpr? filter)
        {
            filter ??= SafeFilter;

            Common.GroupEnumUnitsOfPlayer(g, p, filter);
        }

        public static void EXGroupEnumUnitSelected(Common.group g, Common.player p, Common.boolexpr? filter)
        {
            filter ??= SafeFilter;

            Common.GroupEnumUnitsSelected(g, p, filter);
        }
    }
}
