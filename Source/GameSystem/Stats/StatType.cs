// ------------------------------------------------------------------------------
// <copyright file="StatType.cs" company="Kray Oristine">
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

namespace Source.GameSystem.Stats
{
    /// <summary>
    /// An enum object re-presenting the stat id in runtime environment
    ///
    /// May differ based on user usage or modification
    /// </summary>
    public enum StatType
    {
        //! Base distribution stats

        /// <summary>
        /// Increase user physical power by 3 and physical penetration by 1.2
        /// </summary>
        Strength,

        /// <summary>
        /// Increase user health by 155 and 1% base health regeneration rate
        /// </summary>
        Vitality,

        /// <summary>
        /// Increase user attack speed by 6
        /// </summary>
        Dexterity,

        /// <summary>
        /// Increase user accuracy by 3 and evasion by 1
        /// </summary>
        Perception,

        /// <summary>
        /// Increase user spell power by 5 and spell penetration by 1.2
        /// </summary>
        Intelligence,

        /// <summary>
        /// Increase user mana by 53 and 0.5% base mana regeneration rate
        /// </summary>
        Wisdom,

        /// <summary>
        /// Increase user luck (yes)
        /// </summary>
        Luck,

        //! Character balancing

        /// <summary>
        /// Define the percentage effectiveness of user attack speed in reduction of attack interval
        /// </summary>
        AttackRating,

        /// <summary>
        /// Allow user to reduce enemy evasion
        /// </summary>
        Accuracy,

        /// <summary>
        /// Allow user to have a chance to evade
        /// </summary>
        Evasion,

        /// <summary>
        /// A integer number that will be converted into percentage using magic mathematical calculation.<br/>
        /// At which point it indicate how faster the user could attack (after deducted or increased by <see cref="AttackRating"/>)
        /// </summary>
        AttackSpeed,

        /// <summary>
        /// Define the user movement speed
        /// </summary>
        MoveSpeed,

        /// <summary>
        /// Define the user sanity, kept it as high as possible
        /// </summary>
        Sanity,

        //! Physical damage

        /// <summary>
        /// Physical damage scaling point
        /// </summary>
        PhysicalPower,

        /// <summary>
        /// Define the float number (non percentage) that will be converted into percentage in reduction of damage (only on physical damage instance)
        /// </summary>
        PhysicalArmor,

        /// <summary>
        /// Define the integer number that will be added to damage (only on physical damage instance)
        /// </summary>
        PhysicalDamageBonus,

        /// <summary>
        /// Define the integer number that will be used to reduce the damage deal to a target (only on physical damage instance)
        /// </summary>
        PhysicalDamageReduction,

        /// <summary>
        /// Define the float number that will be used to reduce the damage deal to a target (only on physical damage instance)
        /// </summary>
        PhysicalDamageResistance,

        /// <summary>
        /// Define both integer/float number that will be used to reduce the target physical armor value before calculation (only on physical damage instance)
        /// </summary>
        PhysicalPenetration,

        //! Magical damage
        /// <summary>
        /// Spell damage scaling point
        /// </summary>
        SpellPower,

        /// <summary>
        /// Define the float number (non percentage) that will be converted into percentage in reduction of damage (only on spell damage instance)
        /// </summary>
        SpellArmor,

        /// <summary>
        /// Define the integer number that will be added to damage (only on spell damage instance)
        /// </summary>
        SpellDamageBonus,

        /// <summary>
        /// Define the integer number that will be used to reduce the damage deal to a target (only on spell damage instance)
        /// </summary>
        SpellDamageReduction,

        /// <summary>
        /// Define the float number that will be used to reduce the damage deal to a target (only on spell damage instance)
        /// </summary>
        SpellDamageResistance,

        /// <summary>
        /// Define both integer/float number that will be used to reduce the target spell armor value before calculation (only on spell damage instance)
        /// </summary>
        SpellPenetration,

        //! element damage is a sub-type of spell damage
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        FirePotency,
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        WaterPotency,
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        NaturePotency,
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        EarthPotency,
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        MetalPotency,
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        WindPotency,
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        LightningPotency,
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        LightPotency,
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        DarkPotency,
        /// <summary>
        /// An integer value that will be converted into percentage, indicate the effectiveness of a specific element damage
        /// </summary>
        ChaosPotency,

        //! Other damage stuff

        /// <summary>
        /// Define the chance that a damage instance is specified as blocked, reducing the damage taken based on the target block factor and block amount
        /// </summary>
        DamageBlockChance,
        /// <summary>
        /// Define the percentage value of damage that will be reduced on a successful block
        /// </summary>
        DamageBlockFactor,
        /// <summary>
        /// Define the integer value of damage that will be reduced on a successful block
        /// </summary>
        DamageBlockAmount,
        /// <summary>
        /// Define the chance that a damage is specified as resist, completely set the amount of damage to 0
        /// </summary>
        DamageImmunityChance,
        /// <summary>
        /// Define the amount of damage that will be reduced on any instance of damage (except pure and above)
        /// </summary>
        DamageReduction,
        /// <summary>
        /// Define the percentage amount of damage that will be reduced on any instance of damage (except pure and above)
        /// </summary>
        DamageResistance,

        /// <summary>
        /// Define the chance that a damage instance is specified as critical hit, increasing it damage based on critical factor and amount
        /// </summary>
        CriticalChance,
        /// <summary>
        /// Define the percentage amount of damage to be increased on a critical strike
        /// </summary>
        CriticalFactor,
        /// <summary>
        /// Define the flat amount of damage to be increased on a critical strike
        /// </summary>
        CriticalAmount,
        /// <summary>
        /// Define the percentage amount of chance will used to reduce the attacker critical rate before critical calculation
        /// </summary>
        CriticalResist,
        /// <summary>
        /// Define the percentage amount of critical damage will be reduced after calculation
        /// </summary>
        CriticalDamageResistance,

        /// <summary>
        /// Define the percentage of damage that come from auto-attack that is converted into life.
        /// </summary>
        Lifesteal,

        /// <summary>
        /// Define the percentage of damage that come from auto-attack that is converted into life.
        /// </summary>
        PhysicalVamp,

        /// <summary>
        /// Like Life-steal but work on any instance of spell damage
        /// </summary>
        SpellVamp,

        /// <summary>
        /// Like Life-steal but work on any instance of damage.
        /// </summary>
        OmniVamp,

        //! Supportive stats
        /// <summary>
        /// Define the percentage value that will be used to reduce most debuff duration by that value.
        /// </summary>
        StatusResistance,
        /// <summary>
        /// Define the percentage value that will be used to increase all healing amount from user by that value
        /// </summary>
        HealPotency,
        /// <summary>
        /// Define the percentage value that will be used to increase all shield amount from user by that value
        /// </summary>
        ShieldPotency,
        /// <summary>
        /// Define the percentage value that will be used to increase user buff potency and duration
        /// </summary>
        BuffPotency,
        /// <summary>
        /// Define the percentage value that indicate how fast should this player cooldown tick
        /// </summary>
        AbilityHaste,

        //! internal stats (do not visible to player)
        /// <summary>
        /// The amount of damage that will be reflected to attacker on any instance of physical damage
        /// </summary>
        DamageReflectPhysical,
        /// <summary>
        /// The amount of damage that will be reflected to attacker on any instance of spell damage
        /// </summary>
        DamageReflectSpell,
        /// <summary>
        /// The amount of damage that will be reflected to attacker on any instance of damage
        /// </summary>
        DamageReflectAll,
        /// <summary>
        /// The amount of damage that will be ignored on any instance of physical damage
        /// </summary>
        DamageCancelPhysical,
        /// <summary>
        /// The amount of damage that will be ignored on any instance of spell damage
        /// </summary>
        DamageCancelSpell,
        /// <summary>
        /// The amount of damage that will be ignored on any instance of damage
        /// </summary>
        DamageCancelAll,
        /// <summary>
        /// The amount of damage that will bypass shield on any instance of physical damage
        /// </summary>
        ShieldBypassFactorPhysical,
        /// <summary>
        /// The amount of damage that will bypass shield on any instance of spell damage
        /// </summary>
        ShieldBypassFactorSpell,
        /// <summary>
        /// The amount of damage that will bypass shield on any instance of damage
        /// </summary>
        ShieldBypassFactorAll,

        /// <summary>
        /// is it <see cref="Damage.DamageFlag.INTERNAL"/> but more in-efficient
        /// </summary>
        GodMode,

        /// <summary>
        /// DO NOT MODIFY THIS STAT!
        /// </summary>
        OneshotChance
    }
}
