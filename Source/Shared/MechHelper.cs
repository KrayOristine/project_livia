// ------------------------------------------------------------------------------
// <copyright file="MechHelper.cs" company="Kray Oristine">
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
using WCSharp.Events;
using WCSharp.Shared.Data;
using WCSharp.Dummies;
using static War3Api.Blizzard;
using static War3Api.Common;

namespace Source.Shared
{
    enum AbilityId : int
    {
        Abun = 1096971630,
        Silence = 1513107504, // Z000
        Stun = 1513107505, // Z001
        Slow = 1513107506 // Z002
    }

    public sealed class UnitPool
    {
        private readonly List<unit> units;
        private readonly int id;
        private static readonly player p = Player(PLAYER_NEUTRAL_PASSIVE);

        public UnitPool(int id)
        {
            this.id = id;
            units = new List<unit>();
        }

        public void Recycle(unit u)
        {
            if (GetUnitTypeId(u) != id || !UnitAlive(u)) return;

            units.Add(u);
            SetUnitX(u, Rectangle.WorldBounds.Right);
            SetUnitY(u, Rectangle.WorldBounds.Top);
            SetUnitOwner(u, p, false);
            ShowUnit(u, false);
            BlzPauseUnitEx(u, true);
        }

        public unit Retrieve(player owner, float x, float y, float z, float facing)
        {
            if (units.Count == 0)
            {
                unit u2 = CreateUnit(owner, id, x, y, facing * bj_RADTODEG);
                SetUnitFlyHeight(u2, z, 0);
                return u2;
            }

            unit u = units[^1];
            units.RemoveAt(units.Count - 1);
            BlzPauseUnitEx(u, false);
            ShowUnit(u, true);
            SetUnitX(u, x);
            SetUnitY(u, y);
            SetUnitFlyHeight(u, z, 0);
            BlzSetUnitFacingEx(u, facing * bj_DEGTORAD);
            SetUnitOwner(u, owner, false);

            return u;
        }
    }

    /// <summary>
    /// General class for Fear effect
    /// </summary>
    public sealed class FearSystem : IPeriodicDisposableAction
    {
        internal const int MAX_CHANGE = 200;
        internal const int DIRECTION_CHANGE = 5;

        private static readonly Dictionary<unit, FearSystem> activeDict = new();
        private static readonly Stack<FearSystem> cache = new();
        private static readonly PeriodicDisposableTrigger<FearSystem> trigger = new(0.2f);
        private static readonly Dictionary<unit, bool> flag = new();
        private static readonly Dictionary<unit, float> x = new();
        private static readonly Dictionary<unit, float> y = new();

        public bool Active { get; set; }
        private unit Target { get; set; }
        private int Change { get; set; }
        private bool IsSelected { get; set; }
        public float Duration { get; set; }
        public effect? Effect { get; set; }

        public void Action()
        {
            if (Duration <= 0 || !UnitAlive(Target))
            {
                Active = false;
                return;
            }

            Duration -= 0.2f;
            Change++;

            if (Change == DIRECTION_CHANGE)
            {
                Change = 0;
                flag[Target] = true;
                x[Target] = GetRandomReal(GetUnitX(Target) - MAX_CHANGE, GetUnitX(Target) + MAX_CHANGE);
                y[Target] = GetRandomReal(GetUnitY(Target) - MAX_CHANGE, GetUnitY(Target) + MAX_CHANGE);
                IssuePointOrderById(Target, Constants.ORDER_MOVE, x[Target], y[Target]);
            }
        }

        public void Dispose()
        {
            flag[Target] = true;
            IssueImmediateOrderById(Target, Constants.ORDER_STOP);
            if (Effect != null)
            {
                DestroyEffect(Effect);
                Effect = null;
            }
            UnitRemoveAbility(Target, (int)AbilityId.Abun);
            if (IsSelected)
            {
                SelectUnitAddForPlayer(Target, GetOwningPlayer(Target));
            }
            activeDict.Remove(Target);
            cache.Push(this);
        }

        public FearSystem(unit target)
        {
            Active = false;
            Target = target;
        }

        private static FearSystem GetCache(unit u)
        {
            if (cache.Count > 0)
            {
                var temp = cache.Pop();
                temp.Target = u;

                return temp;
            }

            return new FearSystem(u);
        }

        /// <summary>
        /// Fear the specified unit for a duration, also apply effect at given attachment point
        /// </summary>
        /// <param name="target">Target to fear them</param>
        /// <param name="duration">How long it should be feared</param>
        /// <param name="sfx"></param>
        /// <param name="attachmentPoint"></param>
        public static void Apply(unit target, float duration, string sfx, string attachmentPoint)
        {
            FearSystem temp;
            if (activeDict.ContainsKey(target)) temp = activeDict[target];
            else
            {
                temp = GetCache(target);
                activeDict[target] = temp;

                temp.Change = 0;
                temp.IsSelected = IsUnitSelected(target, GetOwningPlayer(target));

                UnitAddAbility(target, (int)AbilityId.Abun);

                if (temp.IsSelected) SelectUnit(target, false);

                if (!string.IsNullOrEmpty(sfx) && !string.IsNullOrEmpty(attachmentPoint))
                {
                    temp.Effect = AddSpecialEffectTarget(sfx, target, attachmentPoint);
                }
            }

            temp.Duration = duration;
            flag[target] = true;
            x[target] = GetRandomReal(GetUnitX(target) - MAX_CHANGE, GetUnitX(target) + MAX_CHANGE);
            y[target] = GetRandomReal(GetUnitY(target) - MAX_CHANGE, GetUnitY(target) + MAX_CHANGE);
            IssuePointOrderById(target, Constants.ORDER_MOVE, x[target], y[target]);
            trigger.Add(temp);
        }

        /// <summary>
        /// Apply Fear to the target
        /// </summary>
        /// <param name="target">Target to be feared</param>
        /// <param name="duration">How long should it last?</param>
        public static void Apply(unit target, float duration)
        {
            FearSystem temp;
            if (activeDict.ContainsKey(target)) temp = activeDict[target];
            else
            {
                temp = GetCache(target);
                activeDict[target] = temp;

                temp.Change = 0;
                temp.IsSelected = IsUnitSelected(target, GetOwningPlayer(target));

                UnitAddAbility(target, (int)AbilityId.Abun);

                if (temp.IsSelected) SelectUnit(target, false);
            }

            temp.Duration = duration;
            flag[target] = true;
            x[target] = GetRandomReal(GetUnitX(target) - MAX_CHANGE, GetUnitX(target) + MAX_CHANGE);
            y[target] = GetRandomReal(GetUnitY(target) - MAX_CHANGE, GetUnitY(target) + MAX_CHANGE);
            IssuePointOrderById(target, Constants.ORDER_MOVE, x[target], y[target]);
            trigger.Add(temp);
        }

        /// <summary>
        /// UnFear the given target
        /// </summary>
        /// <param name="target"></param>
        public static void UnFear(unit target)
        {
            if (!activeDict.ContainsKey(target)) return;
            activeDict[target].Active = false;
        }

        public static bool IsFeared(unit u) => activeDict.ContainsKey(u);

        private static void EventOnSelected()
        {
            unit u = GetTriggerUnit();
            if (!IsFeared(u)) return;
            if (!IsUnitSelected(u, GetOwningPlayer(u))) return;

            SelectUnit(u, false);
        }

        private static void EventOnOrder()
        {
            unit u = GetOrderedUnit();
            if (!IsFeared(u)) return;
            if (!flag.ContainsKey(u))
            {
                flag[u] = true;
                IssuePointOrderById(u, Constants.ORDER_MOVE, x[u], y[u]);
            }
            else
            {
                flag.Remove(u);
            }
        }

        static FearSystem()
        {
            try
            {
                PlayerUnitEvents.Register(PlayerEvent.SelectsUnit, EventOnSelected);
                PlayerUnitEvents.Register(UnitTypeEvent.ReceivesOrder, EventOnOrder);
                PlayerUnitEvents.Register(UnitTypeEvent.ReceivesPointOrder, EventOnOrder);
                PlayerUnitEvents.Register(UnitTypeEvent.ReceivesTargetOrder, EventOnOrder);
                PlayerUnitEvents.Register(UnitTypeEvent.ReceivesUnitTypeOrder, EventOnOrder);
            }
            catch (Exception ex)
            {
                Logger.Error("Mech Helper", ex.ToString());
            }
        }
    }

    /// <summary>
    /// General class for Disarm effect
    /// </summary>
    public sealed class DisarmSystem : IPeriodicDisposableAction
    {
        private static readonly Dictionary<unit, int> activeDict = new();
        private static readonly Stack<DisarmSystem> cache = new();
        private static readonly PeriodicDisposableTrigger<DisarmSystem> trigger = new(0.03125f);
        private static readonly Dictionary<unit, DisarmSystem> disarmLoops = new();

        public DisarmSystem(unit target)
        {
            Target = target;
        }

        public bool Active { get; set; }
        public float Duration { get; set; }
        public unit Target { get; set; }

        public void Dispose()
        {
            StackCheck(Target, false);
            disarmLoops.Remove(Target);
            cache.Push(this);
        }

        public void Action()
        {
            if (Duration <= 0 || !UnitAlive(Target))
            {
                Active = false;
                return;
            }

            Duration -= 0.03125f;
        }

        public static DisarmSystem GetCache(unit u)
        {
            if (cache.Count > 0)
            {
                var temp = cache.Pop();
                temp.Target = u;

                return temp;
            }

            return new(u);
        }

        public static bool IsDisarmed(unit target) => activeDict.ContainsKey(target);

        private static void StackCheck(unit target, bool apply)
        {
            if (!activeDict.ContainsKey(target))
            {
                activeDict[target] = 0;
            }

            activeDict[target] += apply ? 1 : -1;

            if (activeDict[target] > 0) UnitAddAbility(target, (int)AbilityId.Abun);
            else UnitRemoveAbility(target, (int)AbilityId.Abun);
        }

        /// <summary>
        /// Apply Disarm to target
        /// </summary>
        /// <param name="target">Target of the effect</param>
        public static void Apply(unit target)
        {
            StackCheck(target, true);
        }

        /// <summary>
        /// Apply disarm to target for a set duration
        /// </summary>
        /// <param name="target">Disarm target</param>
        /// <param name="duration">How long should it last</param>
        public static void Apply(unit target, float duration)
        {
            DisarmSystem temp = disarmLoops.ContainsKey(target) ? disarmLoops[target] : GetCache(target);
            temp.Duration = duration;

            StackCheck(target, true);

            trigger.Add(temp);
        }

        public static void UnDisarm(unit target)
        {
            if (disarmLoops.ContainsKey(target))
            {
                disarmLoops[target].Active = false;
                return;
            }

            StackCheck(target, false);
        }
    }

    /// <summary>
    /// General class contain all crowd control effect that is shared across script<br/>
    ///
    /// Only use to apply effect since it doesn't care any unit stats
    /// </summary>
    public static class CrowdControl
    {
        internal static group g = CreateGroup();

        internal static void ModifyDuration(unit target, int id, ability abil, float time)
        {
            BlzSetAbilityRealLevelField(abil, ABILITY_RLF_DURATION_NORMAL, 0, time);
            BlzSetAbilityRealLevelField(abil, ABILITY_RLF_DURATION_HERO, 0, time);
            IncUnitAbilityLevel(target, id);
            DecUnitAbilityLevel(target, id);
        }

        /// <summary>
        /// Silence a single target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration">How long should it last</param>
        public static void Silence(unit target, float duration)
        {
            unit dummy = DummySystem.GetDummy();

            SetUnitX(dummy, GetUnitX(target));
            SetUnitY(dummy, GetUnitY(target));
            UnitAddAbility(dummy, (int)AbilityId.Silence);
            ModifyDuration(dummy, (int)AbilityId.Silence, BlzGetUnitAbility(dummy, (int)AbilityId.Silence), duration);
            IssueTargetOrderById(dummy, Constants.ORDER_DRUNKEN_HAZE, target);
            DummySystem.RecycleDummy(dummy, 0f);
        }

        /// <summary>
        /// Silence an entire unit group
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration">How long should it last</param>
        public static void Silence(group target, float duration)
        {
            int size = BlzGroupGetSize(target);
            if (size == 0) return;

            unit curr; // current loops unit
            while (size > 0)
            {
                curr = BlzGroupUnitAt(target, 0);
                if (curr != null && UnitAlive(curr))
                {
                    unit dummy = DummySystem.GetDummy();
                    SetUnitX(dummy, GetUnitX(curr));
                    SetUnitY(dummy, GetUnitY(curr));
                    UnitAddAbility(dummy, (int)AbilityId.Silence);
                    ModifyDuration(dummy, (int)AbilityId.Silence, BlzGetUnitAbility(dummy, (int)AbilityId.Silence), duration);
                    IssueTargetOrderById(dummy, Constants.ORDER_DRUNKEN_HAZE, curr);
                    DummySystem.RecycleDummy(dummy, 0f);
                }

                size--;
            }
        }

        /// <summary>
        /// AOE silence at given position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        /// <param name="duration">How long should it last</param>
        public static void Silence(float x, float y, float radius, float duration)
        {
            GroupClear(g);
            GroupEnumUnitsInRange(g, x, y, radius, null);
            Silence(g, duration);
        }

        /// <summary>
        /// Stun a given unit for x seconds
        /// </summary>
        /// <param name="target">Stun target</param>
        /// <param name="duration">How long should it last</param>
        public static void Stun(unit target, float duration)
        {
            unit dummy = DummySystem.GetDummy();

            SetUnitX(dummy, GetUnitX(target));
            SetUnitY(dummy, GetUnitY(target));
            UnitAddAbility(dummy, (int)AbilityId.Stun);
            ModifyDuration(dummy, (int)AbilityId.Stun, BlzGetUnitAbility(dummy, (int)AbilityId.Stun), duration);
            IssueTargetOrderById(dummy, Constants.ORDER_THUNDERBOLT, target);
            DummySystem.RecycleDummy(dummy, 0f);
        }

        /// <summary>
        /// Stun all unit in a group for x seconds
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration">How long should it last</param>
        public static void Stun(group target, float duration)
        {
            int size = BlzGroupGetSize(target);
            if (size == 0) return;

            unit curr; // current loops unit
            while (size > 0)
            {
                curr = BlzGroupUnitAt(target, 0);
                if (curr != null && UnitAlive(curr))
                {
                    unit dummy = DummySystem.GetDummy();
                    SetUnitX(dummy, GetUnitX(curr));
                    SetUnitY(dummy, GetUnitY(curr));
                    UnitAddAbility(dummy, (int)AbilityId.Stun);
                    ModifyDuration(dummy, (int)AbilityId.Stun, BlzGetUnitAbility(dummy, (int)AbilityId.Stun), duration);
                    IssueTargetOrderById(dummy, Constants.ORDER_THUNDERBOLT, curr);
                    DummySystem.RecycleDummy(dummy, 0f);
                }

                size--;
            }
        }

        /// <summary>
        /// Stun any unit in a range
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        /// <param name="duration">How long should it last</param>
        public static void Stun(float x, float y, float radius, float duration)
        {
            GroupClear(g);
            GroupEnumUnitsInRange(g, x, y, radius, null);
            Stun(g, duration);
        }
    }
}
