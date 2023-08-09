using System.Collections.Generic;

/*
 * Ozzzzymaniac Custom Save/Load
 *
 * This is the core function of the maps
 */

namespace Source.GameSystem.Save
{
    public static class SaveableObject
    {
        internal static readonly List<int> heroStorage = new();
        internal static readonly List<int> abilityStorage = new();
        internal static readonly List<int> upgradeStorage = new();
        internal static int heroCounter = 0;
        internal static int abilityCounter = 0;
        internal static int upgradeCounter = 0;

        public static void AddSaveableHero(int id)
        {
            if (heroStorage.Contains(id)) return;

            heroStorage.Insert(id, heroCounter);
            heroStorage.Insert(heroCounter, id);
            heroCounter++;
        }

        public static void AddSaveableAbility(int id)
        {
            if (abilityStorage.Contains(id)) return;

            abilityStorage.Insert(id, abilityCounter);
            abilityStorage.Insert(abilityCounter, id);
            abilityCounter++;
        }

        public static void AddSaveableUpgrade(int id)
        {
            if (upgradeStorage.Contains(id)) return;

            upgradeStorage.Insert(id, upgradeCounter);
            upgradeStorage.Insert(upgradeCounter, id);
            upgradeCounter++;
        }

        public static int GetHeroId(int id)
        {
            return heroStorage.Contains(id) ? heroStorage[id] : -1;
        }

        public static int GetAbilityId(int id)
        {
            return abilityStorage.Contains(id) ? abilityStorage[id] : -1;
        }

        public static int GetUpgradeId(int id)
        {
            return upgradeStorage.Contains(id) ? upgradeStorage[id] : -1;
        }
    }
}
