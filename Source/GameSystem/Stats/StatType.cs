// Shorthand for Update, since i lazy as fuck

namespace Source.GameSystem.Stats
{
    /// <summary>
    /// An enum object re-presenting the stat id in runtime environment
    ///
    /// May differ based on user usage or modification
    /// </summary>
    public enum StatType
    {

        // Base distribution stats
        STRENGTH,
        VITALITY,
        DEXTERITY,
        PERCEPTION,
        INTELLIGENCE,
        MIND,
        WISDOM,
        LUCK,

        // Character balancing
        ATTACK_RATING,
        ACCURACY,
        EVASION,
        ATTACK_SPEED,
        MOVE_SPEED,
        SANITY,

        // Physical damage
        PHYSICAL_ATTACK,
        PHYSICAL_ARMOR,
        PHYSICAL_DAMAGE_BONUS,
        PHYSICAL_DAMAGE_REDUCTION,
        PHYSICAL_DAMAGE_RESISTANCE,
        PHYSICAL_ARMOR_PENETRATION,

        // Magical damage
        MAGICAL_POWER,
        MAGICAL_ARMOR,
        MAGICAL_DAMAGE_BONUS,
        MAGICAL_DAMAGE_REDUCTION,
        MAGICAL_DAMAGE_RESISTANCE,
        MAGICAL_ARMOR_PENETRATION,

        // Elemental related damage
        ELEMENTAL_DAMAGE_BONUS,
        ELEMENTAL_DAMAGE_REDUCTION,
        ELEMENTAL_DAMAGE_RESISTANCE,
        ELEMENTAL_ARMOR_PENETRATION,
        ELEMENTAL_POTENCY,
        FIRE_POTENCY,
        WATER_POTENCY,
        NATURE_POTENCY,
        EARTH_POTENCY,
        METAL_POTENCY,
        WIND_POTENCY,
        LIGHTNING_POTENCY,
        LIGHT_POTENCY,
        DARK_POTENCY,
        CHAOS_POTENCY,

        // Other damage stuff
        DAMAGE_BLOCK_CHANCE,
        DAMAGE_BLOCK_FACTOR,
        DAMAGE_BLOCK_AMOUNT,
        DAMAGE_REDUCTION,
        DAMAGE_RESISTANCE,
        CRITICAL_CHANCE,
        CRITICAL_FACTOR,
        CRITICAL_AMOUNT,
        CRITICAL_RESIST,
        CRITICAL_DAMAGE_REDUCTION,

        // Supportive stats
        DEBUFF_RESISTANCE,
        HEAL_POTENCY,
        SHIELD_POTENCY,
        SKILL_POTENCY,
        ABILITY_HASTE,

        // Very special one
        ONESHOT_CHANCE
    }
}
