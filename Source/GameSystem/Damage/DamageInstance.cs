// ------------------------------------------------------------------------------
// <copyright file="DamageInstance.cs" company="Kray Oristine">
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
using System.Collections.Generic;
using System;
using static War3Api.Common;

namespace Source.GameSystem.Damage
{
    public class DamageInstance
    {
        private unit source;
        private unit target;
        internal DamageTrigger? Recursive { get; set; }

        public int SourceType { get; private set; }
        public int TargetType { get; private set; }
        public player SourcePlayer { get; private set; }
        public player TargetPlayer { get; private set; }
        public float Damage { get; set; }
        public bool IsAttack { get; set; }
        public bool IsRanged { get; set; }

        /// <summary>
        /// Bit-field. See <see cref="DamageFlag"/> for better information on which bit is represent which flag
        /// </summary>
        public int Flags { get; set; }

        public attacktype AttackType { get; set; }
        public damagetype DamageType { get; set; }
        public weapontype WeaponType { get; set; }
        public float PrevAmt { get; private set; }
        public bool IsEmpty { get; private set; }

        //! Internal cache
        private static readonly Stack<DamageInstance> cache = new();

        public unit Source
        {
            get => source; set
            {
                source = value;
                SourceType = GetUnitTypeId(value);
                SourcePlayer = GetOwningPlayer(value);
            }
        }

        public unit Target
        {
            get => target; set
            {
                target = value;
                TargetType = GetUnitTypeId(value);
                TargetPlayer = GetOwningPlayer(value);
            }
        }

        public weapontype PrevWeaponType { get; private set; }
        public damagetype PrevDamageType { get; private set; }
        public attacktype PrevAttackType { get; private set; }

        public void Update(unit source, unit target, float damage, bool isAttack, bool isRanged, attacktype attackType, damagetype damageType, weapontype weaponType)
        {
            this.source = source;
            this.target = target;
            SourceType = GetUnitTypeId(source);
            TargetType = GetUnitTypeId(target);
            SourcePlayer = GetOwningPlayer(source);
            TargetPlayer = GetOwningPlayer(target);
            Damage = damage;
            AttackType = attackType;
            DamageType = damageType;
            WeaponType = weaponType;
            IsAttack = isAttack;
            IsRanged = isRanged;
            Flags = 0;
            PrevAmt = damage;
            PrevAttackType = attackType;
            PrevDamageType = damageType;
            PrevWeaponType = weaponType;
            IsEmpty = false;
        }

        public static void Recycle(DamageInstance d)
        {
            if (d == null) throw new ArgumentNullException(nameof(d), "Damage instance is null");
            d.IsEmpty = true;
            cache.Push(d);
        }

        private DamageInstance(unit src, unit tgt, float dmg, bool iatk, bool irgd, attacktype tatk, damagetype tdmg, weapontype twpn)
        {
            source = src;
            target = tgt;
            SourceType = GetUnitTypeId(src);
            TargetType = GetUnitTypeId(tgt);
            SourcePlayer = GetOwningPlayer(src);
            TargetPlayer = GetOwningPlayer(tgt);
            Damage = dmg;
            AttackType = tatk;
            DamageType = tdmg;
            WeaponType = twpn;
            IsAttack = iatk;
            IsRanged = irgd;
            Flags = new();
            PrevAmt = dmg;
            PrevAttackType = tatk;
            PrevDamageType = tdmg;
            PrevWeaponType = twpn;
            IsEmpty = false;
            Recursive = null;
        }

        public bool HasFlag(DamageFlag whichFlag)
        {
            return (Flags & (int)whichFlag) > 0;
        }

        public void AddFlag(DamageFlag flag)
        {
            Flags |= (int)flag;
        }

        public void RemoveFlag(DamageFlag flag)
        {
            Flags &= ~(int)flag;
        }

        public static DamageInstance Create(unit source, unit target, float damage, bool isAttack, bool isRanged, attacktype attackType, damagetype damageType, weapontype weaponType)
        {
            DamageInstance d;
            if (cache.Count > 0)
            {
                d = cache.Pop();
                d.Update(source, target, damage, isAttack, isRanged, attackType, damageType, weaponType);
            }
            else
            {
                d = new(source, target, damage, isAttack, isRanged, attackType, damageType, weaponType);
            }

            return d;
        }
    }
}
