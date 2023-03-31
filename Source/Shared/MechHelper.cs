using System;
using System.Collections.Generic;
using WCSharp.Events;
using WCSharp.Shared.Data;
using static War3Api.Blizzard;
using static War3Api.Common;

namespace Source.Shared
{
    internal enum AbilityId
    {
        Abun = 1096971630
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

        public static int Init()
        {
            try
            {
                PlayerUnitEvents.Register(PlayerEvent.SelectsUnit, EventOnSelected);
                PlayerUnitEvents.Register(UnitTypeEvent.ReceivesOrder, EventOnOrder);
                PlayerUnitEvents.Register(UnitTypeEvent.ReceivesPointOrder, EventOnOrder);
                PlayerUnitEvents.Register(UnitTypeEvent.ReceivesTargetOrder, EventOnOrder);
                PlayerUnitEvents.Register(UnitTypeEvent.ReceivesUnitTypeOrder, EventOnOrder);

                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error("Mech Helper", ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// If this value equal to -1, it mean that the initialization is failed
        /// </summary>
        public static readonly int doInit = Init();
    }

    public sealed class DisarmSystem : IPeriodicDisposableAction
    {
        private static readonly Dictionary<unit, int> activeDict = new();
        private static readonly Stack<DisarmSystem> cache = new();
        private static readonly PeriodicDisposableTrigger<DisarmSystem> trigger = new(1 / 32f);
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

            Duration -= 1 / 32f;
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
    /// General class contain all crowd control effect that is shared across script
    /// </summary>
    public static class CrowdControl
    {

    }
}
