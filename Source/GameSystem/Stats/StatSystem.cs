using System;
using System.Collections.Generic;
using static War3Api.Common;

// Shorthand for Update, since i lazy as fuck
using UpdateAction = System.Action<War3Api.Common.unit, double, double, Source.GameSystem.Stats.ModifyMode>;

namespace Source.GameSystem.Stats
{
    /// <summary>
    /// Core class that handle stat modification
    /// </summary>
    static class StatSystem
    {
        private static readonly List<Dictionary<unit, double>> Base = new();
        private static readonly List<Dictionary<unit, double>> Bonus = new();
        private static readonly List<string> Name = new();
        private static readonly List<bool> IsFloat = new();
        private static readonly List<UpdateAction> OnUpdate = new();
        private static readonly Dictionary<StatType, List<UpdateAction>> HooksUpdate = new();
        private static int count = -1;

        /// <summary>
        /// Create a new stats
        /// </summary>
        /// <param name="name">The name of the stats</param>
        /// <param name="isPercent">Is the stats used as percentage?</param>
        /// <param name="onUpdateAction">A method that will invoke every time the user made edit to the stats<br/> This method take args as in order below
        /// <code>(<see cref="unit"/> target, <see langword="double"/> oldValue, <see langword="double"/> newValue, <see cref="ModifyMode"/> mode)</code>
        /// </param>
        /// <returns>The id number of the stats in run-time</returns>
        public static int Create(string name, bool isPercent, UpdateAction onUpdateAction)
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
        /// <param name="id">The id re-present the stats in run-time</param>
        /// <param name="name">New name of the stats</param>
        /// <param name="percentFlag">Is it percent?</param>
        /// <param name="onUpdate">New update method that take args as in order below
        /// <code>(<see cref="unit"/> target, <see langword="double"/> oldValue, <see langword="double"/> newValue, <see cref="ModifyMode"/> mode)</code>
        /// </param>
        /// <param name="useHook"><see langword="true"/> to hooks the update action instead of editing the entire stats instance</param>
        public static void Override(StatType id, string name, bool percentFlag, UpdateAction onUpdate, bool useHook = false)
        {
            if (useHook)
            {
                if (HooksUpdate.ContainsKey(id))
                {
                    HooksUpdate[id].Add(onUpdate);
                    return;
                }
                var old = OnUpdate[(int)id];
                HooksUpdate[id] = new()
                {
                    onUpdate
                };
                OnUpdate[(int)id] = (u, oldValue, newValue, flag) =>
                {
                    for (int i = 0; i < HooksUpdate[id].Count; i++)
                    {
                        HooksUpdate[id][i].Invoke(u, oldValue, newValue, flag);
                    }

                    old.Invoke(u, oldValue, newValue, flag);
                };

                return;
            }

            Name[(int)id] = name;
            IsFloat[(int)id] = percentFlag;
            OnUpdate[(int)id] = onUpdate;
        }

        /// <summary>
        /// Get stats from a unit
        /// </summary>
        /// <param name="id">Integer number re-present the stat id in run-time</param>
        /// <param name="u">The target unit stats</param>
        /// <param name="mode">Stat mode - based on the value</param>
        /// <returns>The stats that the unit has</returns>
        public static double Get(StatType id, unit u, ModifyMode mode)
        {
            switch (mode)
            {
                case ModifyMode.BASE:
                    return Base[(int)id].ContainsKey(u) ? Base[(int)id][u] : 0;
                case ModifyMode.BONUS:
                    return Bonus[(int)id].ContainsKey(u) ? Bonus[(int)id][u] : 0;
                case ModifyMode.TOTAL:
                    double total = 0;
                    total += Base[(int)id].ContainsKey(u) ? Base[(int)id][u] : 0;
                    total += Bonus[(int)id].ContainsKey(u) ? Bonus[(int)id][u] : 0;
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
        public static void Set(StatType id, unit u, ModifyMode mode, double newValue)
        {
            double old;
            switch (mode)
            {
                case ModifyMode.BASE:
                    old = Base[(int)id].ContainsKey(u) ? Base[(int)id][u] : 0;
                    Base[(int)id][u] = newValue;
                    OnUpdate[(int)id].Invoke(u, old, newValue, mode);
                    return;
                case ModifyMode.BONUS:
                    old = Bonus[(int)id].ContainsKey(u) ? Bonus[(int)id][u] : 0;
                    Bonus[(int)id][u] = newValue;
                    OnUpdate[(int)id].Invoke(u, old, newValue, mode);
                    return;
                case ModifyMode.TOTAL:
                    double divided = Math.Ceiling(newValue / 2);
                    old = Base[(int)id].ContainsKey(u) ? Base[(int)id][u] : 0;
                    Base[(int)id][u] = divided;
                    OnUpdate[(int)id].Invoke(u, old, divided, ModifyMode.BASE);
                    old = Bonus[(int)id].ContainsKey(u) ? Bonus[(int)id][u] : 0;
                    Bonus[(int)id][u] = divided;
                    OnUpdate[(int)id].Invoke(u, old, divided, ModifyMode.BONUS);
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
        public static void Add(StatType id, unit u, ModifyMode mode, double amount)
        {
            double old;
            double newVal;
            switch (mode)
            {
                case ModifyMode.BASE:
                    old = Base[(int)id].ContainsKey(u) ? Base[(int)id][u] : 0;
                    newVal = old + amount;
                    Base[(int)id][u] = newVal;
                    OnUpdate[(int)id].Invoke(u, old, newVal, mode);
                    return;
                case ModifyMode.BONUS:
                    old = Bonus[(int)id].ContainsKey(u) ? Bonus[(int)id][u] : 0;
                    newVal = old + amount;
                    Bonus[(int)id][u] = newVal;
                    OnUpdate[(int)id].Invoke(u, old, newVal, mode);
                    return;
                case ModifyMode.TOTAL:
                    double divided = Math.Ceiling(amount / 2);
                    old = Base[(int)id].ContainsKey(u) ? Base[(int)id][u] : 0;
                    newVal = old + divided;
                    Base[(int)id][u] = newVal;
                    OnUpdate[(int)id].Invoke(u, old, newVal, ModifyMode.BASE);
                    old = Bonus[(int)id].ContainsKey(u) ? Bonus[(int)id][u] : 0;
                    newVal = old + divided;
                    Bonus[(int)id][u] = newVal;
                    OnUpdate[(int)id].Invoke(u, old, newVal, ModifyMode.BONUS);
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
        public static void Sub(StatType id, unit u, ModifyMode mode, double amount)
        {
            double old;
            double newVal;
            switch (mode)
            {
                case ModifyMode.BASE:
                    old = Base[(int)id].ContainsKey(u) ? Base[(int)id][u] : 0;
                    newVal = old - amount;
                    Base[(int)id][u] = newVal;
                    OnUpdate[(int)id].Invoke(u, old, newVal, mode);
                    return;
                case ModifyMode.BONUS:
                    old = Bonus[(int)id].ContainsKey(u) ? Bonus[(int)id][u] : 0;
                    newVal = old - amount;
                    Bonus[(int)id][u] = newVal;
                    OnUpdate[(int)id].Invoke(u, old, newVal, mode);
                    return;
                case ModifyMode.TOTAL:
                    double divided = Math.Ceiling(amount / 2);
                    old = Base[(int)id].ContainsKey(u) ? Base[(int)id][u] : 0;
                    newVal = old - divided;
                    Base[(int)id][u] = newVal;
                    OnUpdate[(int)id].Invoke(u, old, newVal, ModifyMode.BASE);
                    old = Bonus[(int)id].ContainsKey(u) ? Bonus[(int)id][u] : 0;
                    newVal = old - divided;
                    Bonus[(int)id][u] = newVal;
                    OnUpdate[(int)id].Invoke(u, old, newVal, ModifyMode.BONUS);
                    return;
                default: return;
            }
        }

        public static string GetName(StatType id)
        {
            return Name[(int)id];
        }

        public static bool GetIsFloat(StatType id)
        {
            return IsFloat[(int)id];
        }
    }
}
