/*
    Warcraft 3 C# Damage Engine
    This was made by converting Bribe Damage Engine from Lua and JASS into C#
    This converted version includes minor changes to the engine to optimize it

    Current Version: 1.0
 */

namespace Source.GameSystem.Damage
{
    /// <summary>
    /// DamageTypes enum, user can set their own here
    /// </summary>
    public enum DamageTypes
    {
        /// <summary>
        /// DO NOT USE THIS NOR MODIFY IT
        /// </summary>
        None = -1,

        /// <summary>
        /// Is physical
        /// </summary>
        Physical,
        /// <summary>
        /// Is magical
        /// </summary>
        Magical,
        /// <summary>
        /// Is a pure damage<br/><br/>
        /// Bypass armor and damage reduction but not other damage modification
        /// </summary>
        Pure,
        /// <summary>
        /// The damage has been modified by evasion<br/><br/>
        /// Reduce 80% and ignore all debuff effect
        /// </summary>
        Evaded,
        /// <summary>
        /// Is a critical hit
        /// </summary>
        Critical,
        /// <summary>
        /// Heal
        /// </summary>
        Heal, // Heal
        /// <summary>
        /// Damage is done to shield rather than health
        /// </summary>
        Shield,
        /// <summary>
        /// Bypass evasion calculation
        /// </summary>
        Precise,

        // Element

        /// <summary>
        /// Is elemental damage<br/><br/>
        /// Will inherit the elemental damage bonus when an element type specified
        /// </summary>
        Elemental,

        Fire,
        Water,
        Earth,
        Metal,
        Nature,
        Wind,
        Lightning,
        Light,
        Dark,
        /// <summary>
        /// The one that once enabled allow the damage to benefit from all elemental damage bonus
        /// </summary>
        Chaos,

        // GameSystem flags

        /// <summary>
        /// Damage come from ability
        /// </summary>
        Spell,
        /// <summary>
        /// Damage is an auto-attack
        /// </summary>
        Attack,
        /// <summary>
        /// Damage is a damage over time type
        /// </summary>
        Periodic,
        /// <summary>
        /// Damage come from item
        /// </summary>
        Item,
        /// <summary>
        /// Damage is an AOE damage type
        /// </summary>
        AOE,

        // Engine flags, DO NOT EDIT

        /// <summary>
        /// Bypass all modification except the engine it self
        /// </summary>
        RAW,

        /// <summary>
        /// Ignore engine modification and damage event.<br/>
        /// Adding this to a damage instance at anytime will simply make that running instance skip all of the event register after the one that modify this
        /// </summary>
        INTERNAL,
    }
}
