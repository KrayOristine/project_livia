// ------------------------------------------------------------------------------
// <copyright file="Engine.cs" company="Kray Oristine">
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

// Ignore Spelling: atktype dmgtype

using Source.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static War3Api.Blizzard;
using static War3Api.Common;

namespace Source.GameSystem.Damage
{
    /// <summary>
    /// The damage Engine, without it nothing will work
    /// </summary>
    public static class Engine
    {
        internal static readonly int MAX_RECURSIVE_TOLERANCE = 32; // Maximum amount of times the engine itself can dream
        internal static readonly float DEATH_DOOR = 0.405f; // If blizzard decided to change this, it should be a simple fix here

        // Declare all used variables of the engine
        private static readonly trigger damagingTrigger = new();
        private static readonly trigger damagedTrigger = new();
        private static readonly trigger recursiveTrigger = new();
        private static readonly timer AsyncTimer = new();
        private static bool alarmStarted = false;
        private static bool isNotNativeRecursiveDamage = false;
        private static bool waitingForDamageEvents = false;
        private static bool callbackInProgress = false;
        private static int recursiveCallbackDepth = 0;
        private static bool recursiveCallbackInProcess = false;
        private static bool nativeEventsFinished = false;
        private static int sourceAOE = 1;
        private static int sourceStacks = 1;
        private static unit? orgSource;
        private static unit? orgTarget;
        private static readonly HashSet<unit> targets = new();

        private static readonly bool[] attackImmune = {
            false, // ATTACK_TYPE_NORMAL
            true, // ATTACK_TYPE_MELEE
            true, // ATTACK_TYPE_PIERCE
            true, // ATTACK_TYPE_SIEGE
            false, // ATTACK_TYPE_MAGIC
            true, // ATTACK_TYPE_CHAOS
            true, // ATTACK_TYPE_HERO
        };

        private static readonly bool[] damageImmune = {
            true, // DAMAGE_TYPE_UNKNOWN
            false, // NONE
            false, // NONE
            false, // NONE
            true, // DAMAGE_TYPE_NORMAL
            true, // DAMAGE_TYPE_ENHANCED
            false, // NONE
            false, // NONE
            false, // DAMAGE_TYPE_FIRE
            false, // DAMAGE_TYPE_COLD
            false, // DAMAGE_TYPE_LIGHTNING
            true, // DAMAGE_TYPE_POISON
            true, // DAMAGE_TYPE_DISEASE
            false, // DAMAGE_TYPE_DIVINE
            false, // DAMAGE_TYPE_MAGIC
            false, // DAMAGE_TYPE_SONIC
            true, // DAMAGE_TYPE_ACID
            false, // DAMAGE_TYPE_FORCE
            false, // DAMAGE_TYPE_DEATH
            false, // DAMAGE_TYPE_MIND
            false, // DAMAGE_TYPE_PLANT
            false, // DAMAGE_TYPE_DEFENSIVE
            true, // DAMAGE_TYPE_DEMOLITION
            true, // DAMAGE_TYPE_SLOW_POISON
            false, // DAMAGE_TYPE_SPIRIT_LINK
            false, // DAMAGE_TYPE_SHADOW_STRIKE
            true, // DAMAGE_TYPE_UNIVERSAL
        };

        private static DamageTrigger userIndex;
        private static DamageInstance current;
        private static DamageInstance lastInstance;
        private static bool skipEngine = false;
        private static readonly List<DamageInstance> recursiveStacks = new(32);
        private static readonly HashSet<unit> recursiveSource = new();
        private static readonly HashSet<unit> recursiveTarget = new();
        private static bool prep = false;
        private static bool inception = false;

        public static DamageInstance Current
        {
            get => current;
        }

        private static bool IsLastInstance
        {
            get => !lastInstance.IsEmpty; set
            {
                if (!value)
                {
                    DamageInstance.Recycle(lastInstance);
                }
            }
        }

        private static bool IsCurrent
        {
            get => !current.IsEmpty; set
            {
                if (!value)
                {
                    DamageInstance.Recycle(current);
                }
            }
        }

        public static float LethalDamageHP { get; set; } = 0.0f;
        public static int SourceStacks { get => sourceStacks; }

        public static int NextType { get; set; }

        private static readonly LinkedList<DamageTrigger>[] eventList =
        {
            new(),
            new(),
            new(),
            new(),
            new(),
            new()
        };

        private static bool hasSource = false;
        private static bool hasLethal = false;

        /// <summary>
        /// Register an Action that will be executed on specific event<br/>
        /// See <see cref="DamageEvent"/> list to see which event is available
        /// </summary>
        /// <param name="whichEvent">Event that trigger the Action</param>
        /// <param name="priority">Higher number run last, lower run first</param>
        /// <param name="callback">Action that will be triggered every time the event happen</param>
        /// <returns>A node that the Trigger is registered at!, required to be removed</returns>
        public static LinkedListNode<DamageTrigger> Register(DamageEvent whichEvent, int priority, Action callback)
        {
            var head = eventList[(int)whichEvent];
            if ((int)whichEvent >= (int)DamageEvent.SOURCE)
            {
                hasSource = hasSource || whichEvent == DamageEvent.SOURCE;
                hasLethal = hasLethal || whichEvent == DamageEvent.LETHAL;
            }

            var data = DamageTrigger.Create(callback, priority);
            if (head.First == null) return head.AddFirst(data);

            var node = head.First;
            while (true)
            {
                if (node.Value.weight > priority) return head.AddBefore(node, data);
                if (node.Next == null) break;
                node = node.Next;
            }
            return head.AddAfter(node, data);
        }

        /// <summary>
        /// Remove the damage trigger node
        /// </summary>
        /// <param name="node"></param>
        public static void Remove(LinkedListNode<DamageTrigger> node)
        {
            var head = node.List;
            if (head == null) return; // The node itself has no link or it has already removed (unlinked)

            if (head.Count - 1 == 0)
            {
                if (head == eventList[(int)DamageEvent.SOURCE]) hasSource = false;
                else if (head == eventList[(int)DamageEvent.LETHAL]) hasLethal = false;
            }

            head.Remove(node);
            node.Value.Recycle();
        }

        /// <summary>
        /// Remove the specific trigger at specific event
        /// </summary>
        /// <param name="trig">The trigger to remove</param>
        /// <param name="whichEvent">The event to find the trigger</param>
        public static void Remove(DamageTrigger trig, DamageEvent whichEvent)
        {
            var head = eventList[(int)whichEvent];
            if (head == null) return; // Some how the event was invalid

            var node = head.Find(trig);
            if (node == null) return; // No node found

            if (head.Count - 1 == 0)
            {
                if (whichEvent == DamageEvent.SOURCE)
                {
                    hasSource = false;
                }
                else if (whichEvent == DamageEvent.LETHAL)
                {
                    hasLethal = false;
                }
            }

            head.Remove(node);
            node.Value.Recycle();
        }

        public static void Enable(bool flags)
        {
            if (flags)
            {
                if (callbackInProgress) recursiveTrigger.Enable();
                else
                {
                    damagingTrigger.Enable();
                    damagedTrigger.Enable();
                }
                return;
            }

            if (callbackInProgress) recursiveTrigger.Disable();
            else
            {
                damagingTrigger.Disable();
                damagedTrigger.Disable();
            }
        }

        /// <summary>
        /// Are you ready for uncontrollable dreaming?
        /// </summary>
        /// <param name="flags"></param>
        public static void AllowInception(bool flags)
        {
            inception = flags;
        }

        private static readonly Func<bool>[] breakChecks =
        {
            () => current == null || current.HasFlag(DamageFlag.Pure) || skipEngine || current.HasFlag(DamageFlag.INTERNAL),
            () => current == null || current.Damage <= 0 || current.HasFlag(DamageFlag.INTERNAL),
            () => current == null || current.DamageType == DAMAGE_TYPE_UNKNOWN || current.HasFlag(DamageFlag.INTERNAL),
            () => current == null || current.DamageType == DAMAGE_TYPE_UNKNOWN || current.HasFlag(DamageFlag.INTERNAL),
            () => current == null || current.HasFlag(DamageFlag.INTERNAL),
            () => current == null || current.HasFlag(DamageFlag.INTERNAL)
        };

        private static void RunEvent(DamageEvent whichEvent)
        {
            var head = eventList[(int)whichEvent];
            var checks = breakChecks[(int)whichEvent];
            var node = head.First;
            Logger.Debug("Damage Engine", $"RunEvent is running for {whichEvent}");
            if (callbackInProgress || checks.Invoke() || node == null) return;
            userIndex = node.Value;
            damagingTrigger.Disable();
            damagedTrigger.Disable();
            recursiveTrigger.Enable();
            callbackInProgress = true;

            while (true)
            {
                if (!userIndex.isFrozen && !hasSource ||
                     whichEvent != DamageEvent.SOURCE ||
                     sourceAOE > userIndex.minAOE)
                {
                    /*
                     [[
                    xpcall(method, SourceShared.Logger.ErrorSingle)
                     ]]
                     */
                }

                if (node.Next == null) break;
                node = node.Next;
                userIndex = node.Value;
            }

            callbackInProgress = false;
            damagingTrigger.Enable();
            damagedTrigger.Enable();
            recursiveTrigger.Disable();
        }

        private static DamageInstance Create(unit src, unit tgt, float dmg, bool iatk, bool irgd, attacktype tatk, damagetype tdmg, weapontype twpn)
        {
            var d = DamageInstance.Create(src, tgt, dmg, iatk, irgd, tatk, tdmg, twpn);
            if (tatk == ATTACK_TYPE_NORMAL && !iatk) d.AddFlag(DamageFlag.Spell);
            if (iatk) d.AddFlag(DamageFlag.Attack);
            if (NextType != 0)
            {
                d.Flags = NextType;
                NextType = 0;
            }
            return d;
        }

        private static void AddRecursive(DamageInstance d)
        {
            if (d.Damage == 0) return;
            d.Recursive = userIndex;
            var allowIncep = inception || userIndex.allowInception;
            if (!recursiveCallbackInProcess && recursiveSource.Contains(d.Source) && recursiveTarget.Contains(d.Target))
            {
                if (!allowIncep) userIndex.isFrozen = true;
                else if (!userIndex.isFrozen && userIndex.InceptionFloor < recursiveCallbackDepth)
                {
                    userIndex.InceptionFloor++;
                    userIndex.isFrozen = userIndex.InceptionFloor >= MAX_RECURSIVE_TOLERANCE;
                }
            }
            recursiveStacks.Add(d);
        }

        private static void Unfreeze()
        {
            for (int i = 0; i < recursiveStacks.Count; i++)
            {
                DamageTrigger? recursive = recursiveStacks[i].Recursive;
                if (recursive == null) continue;

                recursive.isFrozen = false;
                recursive.InceptionFloor = 0;
            }

            recursiveStacks.Clear();
            recursiveCallbackDepth = 0;
            recursiveCallbackInProcess = false;
            callbackInProgress = false;
            EnableTrigger(damagingTrigger);
            EnableTrigger(damagedTrigger);
            recursiveSource.Clear();
            recursiveTarget.Clear();
        }

        private static void AOEEnd()
        {
            RunEvent(DamageEvent.SOURCE);
            sourceAOE = 1;
            sourceStacks = 1;
            orgSource = null;
            orgTarget = null;
            targets.Clear();
        }

        private static void AfterDamage()
        {
            if (IsCurrent)
            {
                RunEvent(DamageEvent.AFTER);
                IsCurrent = false;
            }
            skipEngine = false;
        }

        private static bool RunDamagingEvent(DamageInstance d, bool isNatural)
        {
            current = d;
            recursiveSource.Add(d.Source);
            recursiveTarget.Add(d.Target);
            if (d.Damage == 0.0f) return true;
            skipEngine = d.DamageType == DAMAGE_TYPE_UNKNOWN || d.HasFlag(DamageFlag.INTERNAL);
            RunEvent(DamageEvent.DAMAGE);
            if (isNatural)
            {
                BlzSetEventAttackType(d.AttackType);
                BlzSetEventDamageType(d.DamageType);
                BlzSetEventWeaponType(d.WeaponType);
                BlzSetEventDamage(d.Damage);
            }
            return false;
        }

        private static void FailsafeClear()
        {
            isNotNativeRecursiveDamage = true;
            recursiveCallbackInProcess = false;
            waitingForDamageEvents = false;
            RunEvent(DamageEvent.DAMAGED);
            nativeEventsFinished = true;
            RunAfterDamageEvents();
        }

        private static void RunAfterDamageEvents()
        {
            if (nativeEventsFinished)
            {
                nativeEventsFinished = false;
                AfterDamage();
            }
            IsCurrent = false;
            skipEngine = false;
            if (!isNotNativeRecursiveDamage && recursiveCallbackInProcess) return;
            if (recursiveStacks.Count <= 0)
            {
                Unfreeze();
                return;
            }

            recursiveCallbackInProcess = true;
            int i = 0;
            do
            {
                recursiveCallbackDepth++;
                int ex = recursiveStacks.Count;
                do
                {
                    prep = true;
                    var d = recursiveStacks[i];
                    if (d.Target.IsAlive)
                    {
                        RunDamagingEvent(d, false);
                        if (d.Damage > 0.0f)
                        {
                            damagingTrigger.Disable();
                            damagedTrigger.Enable();
                            waitingForDamageEvents = true;
                            d.Source.DealDamage(d.Target, d.Damage, d.IsAttack, d.IsRanged, d.AttackType, d.DamageType, d.WeaponType);
                        }
                        else
                        {
                            RunEvent(DamageEvent.DAMAGED);
                            if (d.Damage < 0.0f) d.Target.Life -= d.Damage;
                        }
                        AfterDamage();
                    }
                    i++;
                } while (i < ex);
            } while (i < recursiveStacks.Count);

            Unfreeze();
        }

        [return: NotNullIfNotNull(nameof(source)), NotNullIfNotNull(nameof(target))]
        public static DamageInstance? Apply(unit source, unit target, float amount, bool attack, bool ranged, attacktype atktype, damagetype dmgtype)
        {
            if (source == null || target == null) return null;
            DamageInstance d;
            if (callbackInProgress)
            {
                d = Create(source, target, amount, attack, ranged, atktype, dmgtype, WEAPON_TYPE_WHOKNOWS);
                AddRecursive(d);
            }
            else
            {
                source.DealDamage(target, amount, attack, ranged, atktype, dmgtype, WEAPON_TYPE_WHOKNOWS);
                d = current;
                RunAfterDamageEvents();
            }
            return d;
        }

        [return: NotNullIfNotNull(nameof(source)), NotNullIfNotNull(nameof(target))]
        public static DamageInstance? ApplyMagic(unit source, unit target, float amount, bool attack, bool ranged, attacktype atktype, damagetype dmgtype)
        {
            var d = Apply(source, target, amount, attack, ranged, atktype, dmgtype);
            d.AddFlag(DamageFlag.Magical);

            return d;
        }

        [return: NotNullIfNotNull(nameof(source)), NotNullIfNotNull(nameof(target))]
        public static DamageInstance? ApplyPhysical(unit source, unit target, float amount, bool attack, bool ranged, attacktype atktype, damagetype dmgtype)
        {
            var d = Apply(source, target, amount, attack, ranged, atktype, dmgtype);
            d.AddFlag(DamageFlag.Physical);

            return d;
        }

        [return: NotNullIfNotNull(nameof(source)), NotNullIfNotNull(nameof(target))]
        public static DamageInstance ApplyPure(unit source, unit target, float amount, bool attack, bool ranged, attacktype atktype)
        {
            var d = Apply(source, target, amount, attack, ranged, atktype, DAMAGE_TYPE_UNIVERSAL);
            d.AddFlag(DamageFlag.Pure);

            return d;
        }

        private static DamageInstance CreateFromEvent()
        {
            return Create(GetEventDamageSource(), BlzGetEventDamageTarget(), GetEventDamage(), BlzGetEventIsAttack(), false, BlzGetEventAttackType(), BlzGetEventDamageType(), BlzGetEventWeaponType());
        }

        // Using this to avoid creating the same function ref (was based on lua version)
        private static void AlarmExec()
        {
            alarmStarted = false;
            callbackInProgress = false;
            damagingTrigger.Enable();
            damagedTrigger.Enable();
            if (waitingForDamageEvents)
            {
                FailsafeClear();
            }
            else
            {
                isNotNativeRecursiveDamage = true;
                recursiveCallbackInProcess = false;
                RunAfterDamageEvents();
            }
            AOEEnd();
            IsCurrent = false;
        }

        private static void OnDamaging()
        {
            var d = CreateFromEvent();
            if (alarmStarted)
            {
                if (waitingForDamageEvents)
                {
                    var id = d.DamageType.HandleId;
                    if (id == 20 || id == 21 || id == 24)
                    {
                        lastInstance = current;
                        IsLastInstance = true;
                        waitingForDamageEvents = false;
                        isNotNativeRecursiveDamage = false;
                    }
                    else { FailsafeClear(); }
                }
                else { RunAfterDamageEvents(); }

                if (d.Source != orgSource)
                {
                    AOEEnd();
                    orgSource = d.Source;
                    orgTarget = d.Target;
                }
                else if (d.Target == orgTarget) sourceStacks++;
                else if (!targets.Contains(d.Target)) sourceAOE++;
            }
            else
            {
                AsyncTimer.Start(0f, AlarmExec);
                alarmStarted = true;
                orgSource = d.Source;
                orgTarget = d.Target;
            }

            targets.Add(d.Target);
            if (RunDamagingEvent(d, true))
            {
                isNotNativeRecursiveDamage = true;
                RunAfterDamageEvents();
            }

            waitingForDamageEvents = !IsLastInstance || attackImmune[d.AttackType.HandleId] || damageImmune[d.DamageType.HandleId] || !d.Target.IsUnitType(UNIT_TYPE_MAGIC_IMMUNE);
        }

        private static void OnDamaged()
        {
            float r = GetEventDamage();
            var d = current;

            if (prep) prep = false;
            else if (callbackInProgress || d.PrevAmt == 0) return;
            else if (waitingForDamageEvents) waitingForDamageEvents = false;
            else
            {
                AfterDamage();
                d = lastInstance;
                current = d;
                IsLastInstance = false;
                isNotNativeRecursiveDamage = true;
            }

            d.Damage = r;
            if (r > 0.0f)
            {
                RunEvent(DamageEvent.ARMOR);
                if (hasLethal)
                {
                    LethalDamageHP = d.Target.Life - d.Damage;
                    if (LethalDamageHP <= DEATH_DOOR)
                    {
                        RunEvent(DamageEvent.LETHAL);
                        d.Damage = d.Target.Life - LethalDamageHP;
                    }
                }
            }
            if (d.DamageType != DAMAGE_TYPE_UNKNOWN) RunEvent(DamageEvent.DAMAGED);

            @event.Damage = d.Damage;
            nativeEventsFinished = true;
            if (d.Damage == 0) RunAfterDamageEvents();
        }

        public static void OnRecursive()
        {
            AddRecursive(CreateFromEvent());
            @event.Damage = 0f;
        }

#if DEBUG

        public static Action WrapTryCatch(Action method)
        {
            return () =>
            {
                try
                {
                    method.Invoke();
                }
                catch (Exception ex)
                {
                    Logger.Error("Damage Engine", ex.Message);
                }
            };
        }

#endif

        public static bool InitEngine()
        {
            for (int i = 0; i <= bj_MAX_PLAYERS; i++)
            {
                player p = Player(i);
                TriggerRegisterPlayerUnitEvent(damagingTrigger, p, EVENT_PLAYER_UNIT_DAMAGING, null);
                TriggerRegisterPlayerUnitEvent(damagedTrigger, p, EVENT_PLAYER_UNIT_DAMAGED, null);
                TriggerRegisterPlayerUnitEvent(recursiveTrigger, p, EVENT_PLAYER_UNIT_DAMAGING, null);
            }
#if DEBUG
            TriggerAddAction(damagingTrigger, WrapTryCatch(OnDamaging));
            TriggerAddAction(damagedTrigger, WrapTryCatch(OnDamaged));
            TriggerAddAction(recursiveTrigger, WrapTryCatch(OnRecursive));
#else
            TriggerAddAction(damagingTrigger, OnDamaging);
            TriggerAddAction(damagedTrigger, OnDamaged);
            TriggerAddAction(recursiveTrigger, OnRecursive);
#endif
            recursiveTrigger.Disable();

            return true;
        }
    };
}
