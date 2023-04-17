using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static War3Api.Common;

namespace Source.Modules
{
    // Shorthand for Update, since i lazy as fuck
    using UpdateAction = Action<unit, double, double, StatMode>;

    public enum StatMode
    {
        BASE,
        BONUS,
        TOTAL
    }

    public static class StatSystem
    {
        private static readonly List<Dictionary<unit, double>> Base = new();
        private static readonly List<Dictionary<unit, double>> Bonus = new();
        private static readonly List<string> Name = new();
        private static readonly List<bool> IsFloat = new();
        private static readonly List<UpdateAction> OnUpdate = new();
        private static readonly Dictionary<int, List<UpdateAction>> HooksUpdate = new();
        private static int count = -1;

        /// <summary>
        /// Create a new stats
        /// </summary>
        /// <param name="name">The name of the stats</param>
        /// <param name="isPercent">Is the stats used as percentage?</param>
        /// <param name="onUpdateAction">A method that will invoke every time the user made edit to the stats<br/> This method take args as in order below
        /// <code>(<see cref="unit"/> target, <see langword="double"/> oldValue, <see langword="double"/> newValue, <see cref="StatMode"/> mode)</code>
        /// </param>
        /// <returns>The ID number of the stats in run-time</returns>
        public static int CreateStats(string name, bool isPercent, UpdateAction onUpdateAction)
        {
            Base.Add(new());
            Bonus.Add(new());
            Name.Add(name);
            IsFloat.Add(isPercent);
            OnUpdate.Add(onUpdateAction);
            count++;
            return count;
        }

        /// <summary>
        /// Change how old stats work
        /// </summary>
        /// <param name="ID">The ID re-present the stats in run-time</param>
        /// <param name="name">New name of the stats</param>
        /// <param name="percentFlag">Is it percent?</param>
        /// <param name="onUpdate">New update method that take args as in order below
        /// <code>(<see cref="unit"/> target, <see langword="double"/> oldValue, <see langword="double"/> newValue, <see cref="StatMode"/> mode)</code>
        /// </param>
        /// <param name="useHook"><see langword="true"/> to hooks the update action instead of editing the entire stats instance</param>
        public static void OverrideStats(int ID, string name, bool percentFlag, UpdateAction onUpdate, bool useHook = false)
        {
            if (ID > count || ID < 0) return;

            if (useHook)
            {
                if (HooksUpdate.ContainsKey(ID))
                {
                    HooksUpdate[ID].Add(onUpdate);
                    return;
                }
                var old = OnUpdate[ID];
                HooksUpdate[ID] = new()
                {
                    onUpdate
                };
                OnUpdate[ID] = (unit u, double oldValue, double newValue, StatMode flag) =>
                {
                    for (int i = 0; i < HooksUpdate[ID].Count; i++)
                    {
                        HooksUpdate[ID][i].Invoke(u, oldValue, newValue, flag);
                    }

                    old.Invoke(u, oldValue, newValue, flag);
                };

                return;
            }

            Name[ID] = name;
            IsFloat[ID] = percentFlag;
            OnUpdate[ID] = onUpdate;
        }

        /// <summary>
        /// Get stats from a unit
        /// </summary>
        /// <param name="id">Integer number re-present the stat id in run-time</param>
        /// <param name="u">The target unit stats</param>
        /// <param name="mode">Stat mode - based on the value</param>
        /// <returns>The stats that the unit has</returns>
        public static double Get(int id, unit u, StatMode mode)
        {
            if (id > count || id < 0) return 0;

            switch (mode)
            {
                case StatMode.BASE:
                    return (Base[id].ContainsKey(u) ? Base[id][u] : 0);
                case StatMode.BONUS:
                    return (Bonus[id].ContainsKey(u) ? Bonus[id][u] : 0);
                case StatMode.TOTAL:
                    double total = 0;
                    total += Base[id].ContainsKey(u) ? Base[id][u] : 0;
                    total += Bonus[id].ContainsKey(u) ? Bonus[id][u] : 0;
                    return total;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Set a specific unit stats
        /// </summary>
        /// <param name="id">Integer number re-present the stat id in run-time</param>
        /// <param name="u">The target unit stats</param>
        /// <param name="mode">Stat mode - If use TOTAL, it will be divided into base and bonus but may cause incorrect amount of stats due to rounding</param>
        /// <param name="newValue">The new value of the stats</param>
        public static void Set(int id, unit u, StatMode mode, double newValue)
        {
            if (id > count || id < 0) return;

            double old;
            switch (mode)
            {
                case StatMode.BASE:
                    old = Base[id].ContainsKey(u) ? Base[id][u] : 0;
                    Base[id][u] = newValue;
                    OnUpdate[id].Invoke(u, old, newValue, mode);
                    return;
                case StatMode.BONUS:
                    old = Bonus[id].ContainsKey(u) ? Bonus[id][u] : 0;
                    Bonus[id][u] = newValue;
                    OnUpdate[id].Invoke(u, old, newValue, mode);
                    return;
                case StatMode.TOTAL:
                    double divided = Math.Ceiling(newValue / 2);
                    old = Base[id].ContainsKey(u) ? Base[id][u] : 0;
                    Base[id][u] = divided;
                    OnUpdate[id].Invoke(u, old, divided, StatMode.BASE);
                    old = Bonus[id].ContainsKey(u) ? Bonus[id][u] : 0;
                    Bonus[id][u] = divided;
                    OnUpdate[id].Invoke(u, old, divided, StatMode.BONUS);
                    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// Wrapper for adding stats
        /// </summary>
        /// <param name="id">Integer number re-present the stat id in run-time</param>
        /// <param name="u">The target unit stats</param>
        /// <param name="mode">Stat mode - If use TOTAL, it will be divided into base and bonus but may cause incorrect amount of stats due to rounding</param>
        /// <param name="amount">The amount that will be added</param>
        public static void Add(int id, unit u, StatMode mode, double amount)
        {
            if (id > count || id < 0) return;

            double old;
            double newVal;
            switch (mode)
            {
                case StatMode.BASE:
                    old = Base[id].ContainsKey(u) ? Base[id][u] : 0;
                    newVal = old + amount;
                    Base[id][u] = newVal;
                    OnUpdate[id].Invoke(u, old, newVal, mode);
                    return;
                case StatMode.BONUS:
                    old = Bonus[id].ContainsKey(u) ? Bonus[id][u] : 0;
                    newVal = old + amount;
                    Bonus[id][u] = newVal;
                    OnUpdate[id].Invoke(u, old, newVal, mode);
                    return;
                case StatMode.TOTAL:
                    double divided = Math.Ceiling(amount / 2);
                    old = Base[id].ContainsKey(u) ? Base[id][u] : 0;
                    newVal = old + divided;
                    Base[id][u] = newVal;
                    OnUpdate[id].Invoke(u, old, newVal, StatMode.BASE);
                    old = Bonus[id].ContainsKey(u) ? Bonus[id][u] : 0;
                    newVal = old + divided;
                    Bonus[id][u] = newVal;
                    OnUpdate[id].Invoke(u, old, newVal, StatMode.BONUS);
                    return;
                default: return;
            }
        }

        /// <summary>
        /// Wrapper for subtracting stats (for lazy guy that not want to write out a single fucking dash character)<br/>
        /// Also great for "obfuscating" the code where you subtract a negative value instead of adding and<br/>
        /// add a negative value instead of using this
        /// </summary>
        /// <param name="id">Integer number re-present the stat id in run-time</param>
        /// <param name="u">The target unit stats</param>
        /// <param name="mode">Stat mode - If use TOTAL, it will be divided into base and bonus but may cause incorrect amount of stats due to rounding</param>
        /// <param name="amount">The amount that will be added</param>
        public static void Sub(int id, unit u, StatMode mode, double amount)
        {
            if (id > count || id < 0) return;

            double old;
            double newVal;
            switch (mode)
            {
                case StatMode.BASE:
                    old = Base[id].ContainsKey(u) ? Base[id][u] : 0;
                    newVal = old - amount;
                    Base[id][u] = newVal;
                    OnUpdate[id].Invoke(u, old, newVal, mode);
                    return;
                case StatMode.BONUS:
                    old = Bonus[id].ContainsKey(u) ? Bonus[id][u] : 0;
                    newVal = old - amount;
                    Bonus[id][u] = newVal;
                    OnUpdate[id].Invoke(u, old, newVal, mode);
                    return;
                case StatMode.TOTAL:
                    double divided = Math.Ceiling(amount / 2);
                    old = Base[id].ContainsKey(u) ? Base[id][u] : 0;
                    newVal = old - divided;
                    Base[id][u] = newVal;
                    OnUpdate[id].Invoke(u, old, newVal, StatMode.BASE);
                    old = Bonus[id].ContainsKey(u) ? Bonus[id][u] : 0;
                    newVal = old - divided;
                    Bonus[id][u] = newVal;
                    OnUpdate[id].Invoke(u, old, newVal, StatMode.BONUS);
                    return;
                default: return;
            }
        }

        public static string GetName(int id)
        {
            if (id > count || id < 0) return "";

            return Name[id];
        }

        public static bool GetIsFloat(int id)
        {
            if (id > count || id < 0) return false;

            return IsFloat[id];
        }
    }


    /// <summary>
    /// Class that handle stats creation and features
    /// </summary>
    public static class ExStats
    {
        internal const int baseId = 1496330289; //"Y001"

        internal static void SetAbility(unit u, int offset, abilityintegerlevelfield field, int value, bool remove = false)
        {
            if (GetUnitAbilityLevel(u, baseId + offset) == 0)
            {
                UnitAddAbility(u, baseId + offset);
                UnitMakeAbilityPermanent(u, true, baseId + offset);
            }
            if (value == 0 && remove)
            {
                UnitRemoveAbility(u, baseId + offset);
                return;
            }
            if (BlzSetAbilityIntegerLevelField(BlzGetUnitAbility(u, baseId + offset), field, 0, value))
            {
                IncUnitAbilityLevel(u, baseId + offset);
                DecUnitAbilityLevel(u, baseId + offset);
            }
        }

        internal static void SetAbility(unit u, int offset, abilityreallevelfield field, float value, bool remove = false)
        {
            if (GetUnitAbilityLevel(u, baseId + offset) == 0)
            {
                UnitAddAbility(u, baseId + offset);
                UnitMakeAbilityPermanent(u, true, baseId + offset);
            }
            if (value == 0 && remove)
            {
                UnitRemoveAbility(u, baseId + offset);
                return;
            }
            if (BlzSetAbilityRealLevelField(BlzGetUnitAbility(u, baseId + offset), field, 0, value))
            {
                IncUnitAbilityLevel(u, baseId + offset);
                DecUnitAbilityLevel(u, baseId + offset);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3963:\"static\" fields should be initialized inline", Justification = "Ignore")]
        static ExStats()
        {
            // Base stats

            StatSystem.CreateStats("STRENGTH", false, (unit u, double oldValue, double newValue, StatMode mode) =>
            {
                if (mode == StatMode.BASE)
                {
                    SetHeroStr(u, (int)newValue, true);
                    return;
                }

                SetAbility(u, 0, ABILITY_ILF_STRENGTH_BONUS_ISTR, (int)newValue);
            });
            StatSystem.CreateStats("AGILITY", false, (unit u, double oldValue, double newValue, StatMode mode) =>
            {
                if (mode == StatMode.BASE)
                {
                    SetHeroAgi(u, (int)newValue, true);
                    return;
                }

                SetAbility(u, 1, ABILITY_ILF_AGILITY_BONUS, (int)newValue);
            });
            StatSystem.CreateStats("INTELLIGENCE", false, (unit u, double oldValue, double newValue, StatMode mode) =>
            {
                if (mode == StatMode.BASE)
                {
                    SetHeroInt(u, (int)newValue, true);
                    return;
                }

                SetAbility(u, 2, ABILITY_ILF_INTELLIGENCE_BONUS, (int)newValue);
            });
            StatSystem.CreateStats("HEALTH", false, (unit u, double oldValue, double newValue, StatMode mode) =>
            {
                if (mode == StatMode.BONUS) SetAbility(u, 3, ABILITY_ILF_MAX_LIFE_GAINED, (int)newValue);

                BlzSetUnitMaxHP(u, BlzGetUnitMaxHP(u) - (int)oldValue + (int)newValue);
            });
            StatSystem.CreateStats("MANA", false, (unit u, double oldValue, double newValue, StatMode mode) =>
            {
                if (mode == StatMode.BONUS) SetAbility(u, 4, ABILITY_ILF_MAX_MANA_GAINED, (int)newValue);

                BlzSetUnitMaxMana(u, BlzGetUnitMaxMana(u) - (int)oldValue + (int)newValue);
            });
            StatSystem.CreateStats("DAMAGE", false, (unit u, double oldValue, double newValue, StatMode mode) =>
            {
                if (mode == StatMode.BONUS)
                {
                    SetAbility(u, 5, ABILITY_ILF_ATTACK_BONUS, (int)newValue);
                    return;
                }

                BlzSetUnitBaseDamage(u, BlzGetUnitBaseDamage(u, 0) - (int)oldValue + (int)newValue, 0);
            });
            StatSystem.CreateStats("ARMOR", false, (unit u, double oldValue, double newValue, StatMode mode) =>
            {
                if (mode == StatMode.BONUS)
                {
                    SetAbility(u, 6, ABILITY_ILF_DEFENSE_BONUS_IDEF, (int)newValue);
                    return;
                }

                BlzSetUnitArmor(u, BlzGetUnitArmor(u) - (int)oldValue + (int)newValue);
            });
            StatSystem.CreateStats("MOVE_SPEED", false, (unit u, double oldValue, double newValue, StatMode mode) =>
            {
                if (mode == StatMode.BONUS)
                {
                    SetAbility(u, 7, ABILITY_ILF_MOVEMENT_SPEED_BONUS, (int)newValue);
                    return;
                }

                SetUnitMoveSpeed(u, (float)newValue);
            });
            StatSystem.CreateStats("ATTACK_SPEED", false, (unit u, double oldValue, double newValue, StatMode mode) =>
            {
                SetAbility(u, 7 + (int)mode, ABILITY_RLF_ATTACK_SPEED_INCREASE_ISX1, (float)newValue);
            });
            StatSystem.CreateStats("HP_REGEN", false, (unit u, double oldValue, double newValue, StatMode mode) => {
                SetAbility(u, 15 + (int)mode, ABILITY_RLF_AMOUNT_OF_HIT_POINTS_REGENERATED, (float)newValue);
            });

            StatSystem.CreateStats("MP_REGEN", false, (unit u, double oldValue, double newValue, StatMode mode) => {
                SetAbility(u, 17 + (int)mode, ABILITY_RLF_AMOUNT_REGENERATED, (float)newValue);
            });

            // Bonus stats
        }
    }

    /// <summary>
    /// An enum object re-presenting the stat id in runtime environment
    ///
    /// May differ based on user usage or modification
    /// </summary>
    public enum Stats
    {
        // System added stats

        STRENGTH,
        AGILITY,
        INTELLIGENCE,
        HEALTH,
        MANA,
        DAMAGE,
        ARMOR,
        MOVE_SPEED,
        ATTACK_SPEED,
        HEALTH_REGEN,
        MANA_REGEN,

        // User added stats

        // PHYSICAL
        PHYSICAL_VAMP,
        PHYSICAL_DAMAGE,
        PHYSICAL_RESISTANCE,
        PHYSICAL_PENETRATION,
        PHYSICAL_CRITICAL,
        PHYSICAL_CRIT_DAMAGE,

        // MAGICAL
        MAGICAL_ARMOR,
        MAGICAL_VAMP,
        MAGICAL_DAMAGE,
        MAGICAL_RESISTANCE,
        MAGICAL_PENETRATION,
        MAGICAL_CRITICAL,
        MAGICAL_CRIT_DAMAGE,

        // ELEMENTAL
        ELEMENT_ARMOR,
        ELEMENT_REDUCTION,
        ELEMENT_RESISTANCE,
        ELEMENT_PENETRATION,

        // FIRE
        FIRE_DAMAGE,
        FIRE_RESISTANCE,
        FIRE_REDUCTION,
        FIRE_CRITICAL,
        FIRE_CRIT_DAMAGE,

        // WATER
        WATER_DAMAGE,
        WATER_RESISTANCE,
        WATER_REDUCTION,
        WATER_CRITICAL,
        WATER_CRIT_DAMAGE,

        // NATURE
        NATURE_DAMAGE,
        NATURE_RESISTANCE,
        NATURE_REDUCTION,
        NATURE_CRITICAL,
        NATURE_CRIT_DAMAGE,

        // EARTH
        EARTH_DAMAGE,
        EARTH_RESISTANCE,
        EARTH_REDUCTION,
        EARTH_CRITICAL,
        EARTH_CRIT_DAMAGE,

        // METAL
        METAL_DAMAGE,
        METAL_RESISTANCE,
        METAL_REDUCTION,
        METAL_CRITICAL,
        METAL_CRIT_DAMAGE,

        // WIND
        WIND_DAMAGE,
        WIND_RESISTANCE,
        WIND_REDUCTION,
        WIND_CRITICAL,
        WIND_CRIT_DAMAGE,

        // LIGHTNING
        LIGHTNING_DAMAGE,
        LIGHTNING_RESISTANCE,
        LIGHTNING_REDUCTION,
        LIGHTNING_CRITICAL,
        LIGHTNING_CRIT_DAMAGE,

        // LIGHT
        LIGHT_DAMAGE,
        LIGHT_RESISTANCE,
        LIGHT_REDUCTION,
        LIGHT_CRITICAL,
        LIGHT_CRIT_DAMAGE,

        // DARK
        DARK_DAMAGE,
        DARK_RESISTANCE,
        DARK_REDUCTION,
        DARK_CRITICAL,
        DARK_CRIT_DAMAGE,

        // CHAOS
        CHAOS_DAMAGE,
        CHAOS_RESISTANCE,
        CHAOS_REDUCTION,
        CHAOS_CRITICAL,
        CHAOS_CRIT_DAMAGE,

        // ALL DAMAGE
        DAMAGE_BLOCK_CHANCE,
        DAMAGE_BLOCK_RATE,
        DAMAGE_BLOCK_AMOUNT,
        DAMAGE_REDUCTION,
        DAMAGE_RESISTANCE,
        ARMOR_PENETRATION,
        CRITICAL_RATE,
        CRITICAL_DAMAGE,
        LIFE_STEAL,
        OMNI_VAMP,

        // TANKY
        DEBUFF_RESISTANCE,
        EVASION,

        // SUPPORT
        ACCURACY,
        CRITICAL_HEAL_RATE,
        CRITICAL_HEAL_FACTOR,
        HEAL_EFFECTIVE,
        SELF_HEAL_EFFECTIVE,
        SHIELD_EFFECTIVE,
        SELF_SHIELD_EFFECTIVE,

		// ABILITY
		SKILL_AOE,
		SKILL_EFFECT_DURATION,
		SKILL_EFFECT_POTENCY,

        // FINALIZE
        FINAL_BONUS_DAMAGE,
        FINAL_DAMAGE_RESISTANCE,
        ONESHOT_RATE
    }
}
