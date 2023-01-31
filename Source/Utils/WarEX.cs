using System;
using static War3Api.Common;
using static War3Api.Blizzard;
using WCSharp.Shared;

namespace Source.Utils
{
    public static class WarEX
    {
#pragma warning disable CS0626
#pragma warning disable S4200
        /// <summary>
        /// @CSharpLua.Template = "BlzGetHeroPrimaryStat({0})"
        /// </summary>
        public static extern int BlzGetHeroPrimaryStat(unit u);

        /// <summary>
        /// @CSharpLua.Template = "BlzSetHeroStatEx({0})"
        /// </summary>
        public static extern void BlzSetHeroStatEx(unit u, int i, int amt);

        /// <summary>
        /// @CSharpLua.Template = "BlzGetHeroStat({0})"
        /// </summary>
        public static extern int BlzGetHeroStat(unit u, int i);
#pragma warning restore S4200
#pragma warning restore CS0626


        internal static location? zeroLoc;

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
            return (EXGetLocationZ(GetUnitX(u), GetUnitY(u)) + GetUnitFlyHeight(u));
        }

        public static void Init()
        {
            zeroLoc = Location(0, 0);
        }
    }
}