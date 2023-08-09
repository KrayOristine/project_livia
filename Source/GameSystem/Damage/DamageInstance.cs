using System.Collections.Generic;
using static War3Api.Common;

/*
    Warcraft 3 C# Damage Engine
    This was made by converting Bribe Damage Engine from Lua and JASS into C#
    This converted version includes minor changes to the engine to optimize it

    Current Version: 1.0
 */

namespace Source.GameSystem.Damage
{
    public class DamageInstance
    {
        private unit source;
        private unit target;
        public int SourceType;
        public int TargetType;
        public player SourcePlayer;
        public player TargetPlayer;
        public float Damage;
        public bool IsAttack;
        public bool IsRanged;
        public List<bool> Flags;
        public attacktype AttackType;
        public damagetype DamageType;
        public weapontype WeaponType;
        public float PrevAmt;
        private attacktype prevAttackType;
        private damagetype prevDamageType;
        private weapontype prevWeaponType;
        internal DamageTrigger? recursive;
        public bool IsEmpty;

        //* Internal cache
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

        public weapontype PrevWeaponType { get => prevWeaponType; private set => prevWeaponType = value; }
        public damagetype PrevDamageType { get => prevDamageType; private set => prevDamageType = value; }
        public attacktype PrevAttackType { get => prevAttackType; private set => prevAttackType = value; }

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
            Flags = new();
            PrevAmt = damage;
            PrevAttackType = attackType;
            PrevDamageType = damageType;
            PrevWeaponType = weaponType;
            IsEmpty = false;
        }

        public static void Recycle(DamageInstance d)
        {
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
            prevAttackType = tatk;
            prevDamageType = tdmg;
            prevWeaponType = twpn;
            IsEmpty = false;
            recursive = null;
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
