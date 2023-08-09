using System;
using System.Collections.Generic;
using System.Text;
using static War3Api.Common;

namespace Source.GameSystem.Hero
{
    public static class HeroClass
    {
        private static readonly Dictionary<unit, uint> gameHero = new();

        /// <summary>
        /// All hero class
        /// </summary>
        public enum ClassType : uint
        {
            None = 0,
            Paladin = 1, // ss1
            MartialArtist = 1 << 1, // ss1
            Archer = 1 << 2, // ss1
            Assassin = 1 << 3, // ss1
            Necromancer = 1 << 4, // ss1
            Priest = 1 << 6, // ss1
            NatureEnchanter = 1 << 7, // ss1
            Elementalist = 1 << 8, // ss1
            Dragonkind = 1 << 9, // special (ss1 - 2 - 3)
            DemiDivine = 1 << 10, // special (ss1 - 2 - 3)
            DragonSlayer = 1 << 11, // ss2
            MoonPrayer = 1 << 12, // ss2
            ShadowLord = 1 << 13, // ss2
            Puppeteer = 1 << 14, // ss2
            Tentacle = 1 << 15, // ss3
            RuneCarver = 1 << 16, // ss3
            GodSmith = 1 << 17, // ss3
            Death = 1 << 18, // ss3
        }

        public static bool IsClassType(unit whichUnit, ClassType type)
        {
            return (gameHero[whichUnit] == (uint)type);
        }

        public static uint GetClassType(unit whichUnit)
        {
            return gameHero[whichUnit];
        }
    }
}
