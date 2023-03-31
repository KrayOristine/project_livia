using Source.Modules;
using Source.Shared;
using System;
using static War3Api.Common;

namespace Source.Trigger
{
    public static class DamageTags
    {
        private static (string, int) ColorizeDamage(bool[] flags, bool attack, float amount)
        {
            if (amount < 0.0f || flags[(int)DamageType.Heal]) return ("|c0096FF96+ " + amount, 1); // green
            if (flags[(int)DamageType.Pure]) return ("|c00FFFFFF" + amount, 1); // white
            if (flags[(int)DamageType.Evasion]) return ("MISSED!", 1);
            if (flags[(int)DamageType.Critical]) return ("|c00FF0000" + amount + "!", 2); // red
            if (flags[(int)DamageType.Physical] || attack) return ("|c00FF7F00" + amount, 1); // orange
            if (flags[(int)DamageType.Magical] || flags[(int)DamageType.Spell]) return ("|c006969FF" + amount, 1); // blue
            if (flags[(int)DamageType.Shield]) return ("|c00808080" + amount, 1); // gray
            return ("|c00E45AAA" + amount, 1); // Some how it passed all filter, let indicate it a 'error damage'
        }

        private static void OnDamage()
        {
            try
            {
                var d = Damage.Current;
                unit t = d.Target;
                (string, int) tags = ColorizeDamage(d.Flags, d.IsAttack, d.Damage);
                ArcingTT.Create(tags.Item1, t, GetUnitX(t), GetUnitY(t), 1.5f, tags.Item2, GetLocalPlayer());
            }
            catch (Exception ex)
            {
                Logger.Error("Damage Tags", ex.Message);
            }
        }
        public static void Init()
        {
            Damage.Register(DamageEvent.DAMAGED, 999999, OnDamage);
        }
    }
}
