// ------------------------------------------------------------------------------
// <copyright file="GlobalModifier.cs" company="Kray Oristine">
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source.Shared;
using static War3Api.Common;

namespace Source.GameSystem.Damage
{
    /// <summary>
    /// The one that handle all of the common modifier calculation for the damage instance<br/>
    /// Including:
    /// <list type="number">
    /// <item>Damage and Armor mitigation</item>
    /// <item>Evasion and Accuracy calculation</item>
    /// <item>Critical calculation</item>
    /// </list>
    /// </summary>
    public static class GlobalModifier
    {
        public enum ModifierEvent
        {
            /// <summary>
            /// Upon the damage begin to initialize
            /// </summary>
            ON_HIT,
            /// <summary>
            /// Before the armor begin to calculate and deduct the damage
            /// </summary>
            BEFORE_ARMOR,
            /// <summary>
            /// After the damage has been reduced by armor
            /// </summary>
            AFTER_ARMOR,
            /// <summary>
            /// Before the evasion calculation start
            /// </summary>
            BEFORE_EVASION,
            /// <summary>
            /// After the evasion calculation ended<br/>
            /// If the damage instance evasion flag is true then target has successfully evaded the damage
            /// </summary>
            AFTER_EVASION,
            /// <summary>
            /// Before the critical calculation start
            /// </summary>
            BEFORE_CRITICAL,
            /// <summary>
            /// After the critical calculation ended
            /// </summary>
            AFTER_CRITICAL,
            /// <summary>
            /// When the damage is AOE.<br/>
            /// Run after <see cref="ON_HIT"/>
            /// </summary>
            AOE_ON_HIT,
            /// <summary>
            /// When the damage is AOE.<br/>
            /// Run after <see cref="BEFORE_ARMOR"/>
            /// </summary>
            AOE_BEFORE_ARMOR,
            /// <summary>
            /// When the damage is AOE.<br/>
            /// Run after <see cref="AFTER_ARMOR"/>
            /// </summary>
            AOE_AFTER_ARMOR,
            /// <summary>
            /// When the damage is AOE.<br/>
            /// Run after <see cref="BEFORE_EVASION"/>
            /// </summary>
            AOE_BEFORE_EVASION,
            /// <summary>
            /// When the damage is AOE.<br/>
            /// Run after <see cref="AFTER_EVASION"/>
            /// </summary>
            AOE_AFTER_EVASION,
            /// <summary>
            /// When the damage is AOE.<br/>
            /// Run after <see cref="BEFORE_CRITICAL"/>
            /// </summary>
            AOE_BEFORE_CRITICAL,
            /// <summary>
            /// When the damage is AOE.<br/>
            /// Run after <see cref="AFTER_CRITICAL"/>
            /// </summary>
            AOE_AFTER_CRITICAL,
            /// <summary>
            /// Run AFTER ALL damage calculation and WHEN THE DAMAGE is HIGHER OR EQUAL to the TARGET CURRENT life
            /// </summary>
            ON_LETHAL,
        }

        private static readonly Dictionary<unit, Dictionary<ModifierEvent, float>> modTable = new();

        /// <summary>
        /// Add a modifier to the target unit
        /// </summary>
        /// <param name="target"></param>
        /// <param name="modEvent"></param>
        /// <param name="modAmount"></param>
        public static void AddDamageModifier(unit target, ModifierEvent modEvent, float modAmount)
        {

        }

        /// <summary>
        /// Add a modifier that affect global damage calculation
        /// </summary>
        /// <param name="modEvent"></param>
        /// <param name="modAmount"></param>
        public static void AddGlobalModifier(ModifierEvent modEvent, float modAmount)
        {

        }


#pragma warning disable S1144 // Unused private types or members should be removed
        private static readonly LinkedListNode<DamageTrigger> t1 = Engine.Register(DamageEvent.DAMAGE, 0, OnDamage);
        private static readonly LinkedListNode<DamageTrigger> t2 = Engine.Register(DamageEvent.ARMOR, 0, OnDamaged);
        private static readonly LinkedListNode<DamageTrigger> t3 = Engine.Register(DamageEvent.DAMAGED, 0, OnDamaged);
        private static readonly LinkedListNode<DamageTrigger> t4 = Engine.Register(DamageEvent.AFTER, 0, AfterDamaged);
        private static readonly LinkedListNode<DamageTrigger> t5 = Engine.Register(DamageEvent.LETHAL, 0, AOEDamaged);
        private static readonly LinkedListNode<DamageTrigger> t6 = Engine.Register(DamageEvent.LETHAL, 0, LethalDamaged);
#pragma warning restore S1144 // Unused private types or members should be removed

        public static void OnDamage()
        {

        }

        public static void OnArmor()
        {

        }

        public static void OnDamaged()
        {

        }

        public static void AfterDamaged()
        {

        }

        public static void AOEDamaged()
        {

        }

        public static void LethalDamaged()
        {

        }
    }
}
