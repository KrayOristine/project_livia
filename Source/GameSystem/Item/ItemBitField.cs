// Ignore Spelling: Sacrification Runed

namespace Source.GameSystem.Item
{
    public static class ItemBitField
    {
        /// <summary>
        /// Item types
        /// </summary>
        public enum Types : uint
        {
            None = 0,
            Consumable = 1,
            Potion = 1 << 1,
            Material = 1 << 2,
            Equipment = 1 << 3,
            Gem = 1 << 4,
            Rune = 1 << 5,
            Relic = 1 << 6,
            Currency = 1 << 7,
            Other = 1 << 8,
            Unknown = 1 << 9,
        }

        /// <summary>
        /// Item Rarities
        /// </summary>
        public enum Rarities : uint
        {
            None = 0,
            Common = 1,
            Uncommon = 1 << 1,
            Rare = 1 << 2,
            Epic = 1 << 3,
            Legendary = 1 << 4,
            Mythical = 1 << 5, // ss2
            Dragon = 1 << 6, // ss2
            Divine = 1 << 7, // ss3
            Ancient = 1 << 8, // ss3
        }

        /// <summary>
        /// Prefixes for potion, consumable, equipment, gems, rune or relic items
        /// </summary>
        public enum Prefixes : uint
        {
            None = 0,
            Corrupted = 1,
            Runed = 1 << 1,
            Frenzy = 1 << 2,
            Madness = 1 << 3,
            Divine = 1 << 4,
        }

        /// <summary>
        /// Suffixes for equipment, gems or relic item
        /// </summary>
        public enum Suffixes : uint
        {
            None = 0,
            Power = 1,
            Love = 1 << 1,
            Protection = 1 << 2,
            Sacrification = 1 << 3,
            Mind = 1 << 4,
            Death = 1 << 5,
            Greed = 1 << 6,
            Envy = 1 << 7,
            Warth = 1 << 8,
            Pride = 1 << 9,
            Gluttony = 1 << 10,
            Lust = 1 << 11,
            Sloth = 1 << 12,
        }

        /// <summary>
        /// Item flags, for marking item
        /// </summary>
        public enum Flags : uint
        {
            None = 0,
            Quest = 1,
            Disabled = 1 << 1,
            Removed = 1 << 2,
            Tomes = 1 << 3,
        }
    }
}
