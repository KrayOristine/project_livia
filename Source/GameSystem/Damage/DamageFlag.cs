// ------------------------------------------------------------------------------
// <copyright file="DamageFlag.cs" company="Kray Oristine">
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

namespace Source.GameSystem.Damage
{
    /// <summary>
    /// DamageFlag enum, user can set their own here
    /// </summary>
    public enum DamageFlag
    {
        /// <summary>
        /// DO NOT USE THIS NOR MODIFY IT
        /// </summary>
        None = 0,

        /// <summary>
        /// Is physical
        /// </summary>
        Physical = 1,
        /// <summary>
        /// Is magical
        /// </summary>
        Magical = 1 << 1,
        /// <summary>
        /// The one that benefit both physical and magical at the same time
        /// </summary>
        Astral = 1 << 2,
        /// <summary>
        /// Is a pure damage<br/><br/>
        /// Bypass armor and damage reduction but not other damage modification
        /// </summary>
        Pure = 1 << 3,
        /// <summary>
        /// The damage has been modified by evasion<br/><br/>
        /// Reduce 75% and ignore all debuff effect
        /// </summary>
        Evaded = 1 << 4,
        /// <summary>
        /// Is a critical hit
        /// </summary>
        Critical = 1 << 5,
        /// <summary>
        /// Heal
        /// </summary>
        Heal = 1 << 6, // Heal
        /// <summary>
        /// Damage is done to shield rather than health
        /// </summary>
        Shield = 1 << 7,
        /// <summary>
        /// Bypass evasion calculation
        /// </summary>
        Precise = 1 << 8,

        // Element

        /// <summary>
        /// Is elemental damage<br/><br/>
        /// Will inherit the elemental damage bonus when an element type specified
        /// </summary>
        Elemental = 1 << 9,

        Fire = 1 << 10,
        Water = 1 << 11,
        Earth = 1 << 12,
        Metal = 1 << 13,
        Nature = 1 << 14,
        Wind = 1 << 15,
        Lightning = 1 << 16,
        Light = 1 << 17,
        Dark = 1 << 18,
        /// <summary>
        /// The one that once enabled allow the damage to benefit from all elemental damage bonus
        /// </summary>
        Chaos = 1 << 19,

        // GameSystem flags

        /// <summary>
        /// Damage come from ability
        /// </summary>
        Spell = 1 << 20,
        /// <summary>
        /// Damage is an auto-attack
        /// </summary>
        Attack = 1 << 21,
        /// <summary>
        /// Damage is a damage over time type
        /// </summary>
        Periodic = 1 << 22,
        /// <summary>
        /// Damage come from item
        /// </summary>
        Item = 1 << 23,
        /// <summary>
        /// Damage is an AOE damage type
        /// </summary>
        AOE = 1 << 24,

        // Engine flags, DO NOT EDIT

        /// <summary>
        /// Bypass all modification except the engine it self
        /// </summary>
        RAW = 1 << 25,

        /// <summary>
        /// Ignore engine modification and damage event.<br/>
        /// Adding this to a damage instance at anytime will simply make that running instance skip all of the event register after the one that modify this
        /// </summary>
        INTERNAL = 1 << 26,
    }
}
