// Ignore Spelling: atktype dmgtype

using Source.Shared;
using System;
using System.Collections.Generic;
using static War3Api.Blizzard;
using static War3Api.Common;

namespace Source.GameSystem.Damage
{
    /// <summary>
    /// The damage Engine, without it nothing will work
    /// </summary>
    public static class Engine
    {
        internal static readonly int LIMBO_DEPTH = 8; // Maximum amount of times the engine itself can dream
        internal static readonly float DEATH_DOOR = 0.405f; // If blizzard decided to change this, it should be a simple fix here

        // Declare all used variables of the engine
        private static readonly trigger t1 = CreateTrigger();

        private static readonly trigger t2 = CreateTrigger();
        private static readonly trigger t3 = CreateTrigger();
        private static readonly timer alarm = CreateTimer();
        private static bool alarmSet = false;
        private static bool canKick = false;
        private static bool totem = false;
        private static bool dreaming = false;
        private static int dreamDepth = 0;
        private static bool kicking = false;
        private static bool eventRuns = false;
        private static int sourceAOE = 1;
        private static int sourceStacks = 1;
        private static unit? orgSource;
        private static unit? orgTarget;
        private static readonly Dictionary<unit, bool> targets = new();

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
        private static readonly List<DamageInstance> recursiveStacks = new();
        private static readonly Dictionary<unit, bool> recursiveSource = new();
        private static readonly Dictionary<unit, bool> recursiveTarget = new();
        private static bool prep = false;

        public static DamageInstance Current
        {
            get => current;
        }

        private static bool IsLastInstance { get => !lastInstance.IsEmpty; set
            {
                if (!value)
                {
                    DamageInstance.Recycle(lastInstance);
                }
            }
        }
        private static bool IsCurrent { get => !current.IsEmpty; set
            {
                if (!value)
                {
                    DamageInstance.Recycle(current);
                }
            }
        }

        public static float Life { get; set; } = 0.0f;
        public static int SourceStacks { get => sourceStacks; }

        public static DamageTypes NextType { get; set; }

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
                if (dreaming) EnableTrigger(t3);
                else
                {
                    EnableTrigger(t1);
                    EnableTrigger(t2);
                }
                return;
            }

            if (dreaming) DisableTrigger(t3);
            else
            {
                DisableTrigger(t1);
                DisableTrigger(t2);
            }
        }

        private static readonly Func<bool>[] breakChecks =
        {
            () => current != null && (current.Flags[(int)DamageTypes.Pure] || skipEngine || current.Flags[(int)DamageTypes.INTERNAL]),
            () => current != null && (current.Damage <= 0 || current.Flags[(int)DamageTypes.INTERNAL]),
            () => current != null && (current.DamageType == DAMAGE_TYPE_UNKNOWN || current.Flags[(int)DamageTypes.INTERNAL]),
            () => current != null && (current.DamageType == DAMAGE_TYPE_UNKNOWN || current.Flags[(int) DamageTypes.INTERNAL]),
            () => current != null && (current.Flags[(int)DamageTypes.INTERNAL]),
            () => current != null && (current.Flags[(int)DamageTypes.INTERNAL])
        };

        private static void RunEvent(DamageEvent whichEvent)
        {
            var head = eventList[(int)whichEvent];
            var checks = breakChecks[(int)whichEvent];
            Logger.Debug("Damage Engine", $"RunEvent is running for {whichEvent}");
            if (dreaming || checks.Invoke() || head.First == null) return;
            var node = head.First;
            userIndex = node.Value;
            Enable(false);
            EnableTrigger(t3);
            dreaming = true;

            while (true)
            {
                if (!userIndex.isFrozen && !hasSource || whichEvent != DamageEvent.SOURCE || sourceAOE > userIndex.minAOE)
                {
                    try
                    {
                        userIndex.trig.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                if (node.Next == null) break;
                node = node.Next;
                userIndex = node.Value;
            }

            dreaming = false;
            Enable(true);
            DisableTrigger(t3);
        }

        private static DamageInstance Create(unit src, unit tgt, float dmg, bool iatk, bool irgd, attacktype tatk, damagetype tdmg, weapontype twpn)
        {
            var d = DamageInstance.Create(src, tgt, dmg, iatk, irgd, tatk, tdmg, twpn);

            d.Flags[(int)DamageTypes.Spell] = tatk == ATTACK_TYPE_NORMAL && !iatk;
            d.Flags[(int)DamageTypes.Physical] = iatk;
            if (NextType != DamageTypes.None)
            {
                d.Flags[(int)NextType] = true;
            }
            return d;
        }

        private static void AddRecursive(DamageInstance d)
        {
            if (d.Damage == 0) return;
            d.recursive = userIndex;
            if (!kicking && recursiveSource.ContainsKey(d.Source) && recursiveTarget.ContainsKey(d.Target))
            {
                if (!userIndex.isInception) userIndex.isFrozen = true;
                else if (!userIndex.isFrozen && userIndex.sleepDepth < dreamDepth)
                {
                    userIndex.sleepDepth++;
                    userIndex.isFrozen = userIndex.sleepDepth >= LIMBO_DEPTH;
                }
            }
            recursiveStacks.Add(d);
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

        private static bool DoPreEvent(DamageInstance d, bool isNatural)
        {
            current = d;
            recursiveSource[d.Source] = true;
            recursiveTarget[d.Target] = true;
            if (d.Damage == 0.0f) return false;
            skipEngine = d.DamageType == DAMAGE_TYPE_UNKNOWN || d.Flags[(int)DamageTypes.INTERNAL];
            RunEvent(DamageEvent.DAMAGE);
            if (isNatural)
            {
                BlzSetEventAttackType(d.AttackType);
                BlzSetEventDamageType(d.DamageType);
                BlzSetEventWeaponType(d.WeaponType);
                BlzSetEventDamage(d.Damage);
            }
            return true;
        }

        private static void FailsafeClear()
        {
            canKick = true;
            kicking = false;
            totem = false;
            RunEvent(DamageEvent.DAMAGED);
            eventRuns = true;
            Finish();
        }

        private static void Finish()
        {
            if (eventRuns)
            {
                eventRuns = false;
                AfterDamage();
            }
            IsCurrent = false;
            skipEngine = false;
            if (!canKick && kicking) return;
            if (recursiveStacks.Count > 0)
            {
                kicking = true;
                int i = 0;
                do
                {
                    dreamDepth++;
                    int ex = recursiveStacks.Count;
                    do
                    {
                        prep = true;
                        var d = recursiveStacks[i];
                        if (UnitAlive(d.Target))
                        {
                            DoPreEvent(d, false);
                            if (d.Damage > 0.0f)
                            {
                                DisableTrigger(t1);
                                EnableTrigger(t2);
                                totem = true;
                                UnitDamageTarget(d.Source, d.Target, d.Damage, d.IsAttack, d.IsRanged, d.AttackType, d.DamageType, d.WeaponType);
                            }
                            else
                            {
                                RunEvent(DamageEvent.DAMAGED);
                                if (d.Damage < 0.0f) SetWidgetLife(d.Target, GetWidgetLife(d.Target) - d.Damage);
                            }
                            AfterDamage();
                        }
                        i++;
                    } while (i < ex);
                } while (i < recursiveStacks.Count);

                for (i = 0; i < recursiveStacks.Count; i++)
                {
                    DamageTrigger? recursive = recursiveStacks[i].recursive;
                    if (recursive == null) continue;

                    recursive.isFrozen = false;
                    recursive.sleepDepth = 0;
                }

                recursiveStacks.Clear();
            }
            dreamDepth = 0;
            kicking = false;
            dreaming = false;
            Enable(true);
            recursiveSource.Clear();
            recursiveTarget.Clear();
        }

        public static DamageInstance Apply(unit source, unit target, float amount, bool attack, bool ranged, attacktype atktype, damagetype dmgtype)
        {
            DamageInstance d;
            if (dreaming)
            {
                d = Create(source, target, amount, attack, ranged, atktype, dmgtype, WEAPON_TYPE_WHOKNOWS);
                AddRecursive(d);
            }
            else
            {
                UnitDamageTarget(source, target, amount, attack, ranged, atktype, dmgtype, WEAPON_TYPE_WHOKNOWS);
                d = current;
                Finish();
            }
            return d;
        }

        public static DamageInstance ApplyMagic(unit source, unit target, float amount, bool attack, bool ranged, attacktype atktype, damagetype dmgtype)
        {
            var d = Apply(source, target, amount, attack, ranged, atktype, dmgtype);
            d.Flags[(int)DamageTypes.Magical] = true;

            return d;
        }

        public static DamageInstance ApplyPhysical(unit source, unit target, float amount, bool attack, bool ranged, attacktype atktype, damagetype dmgtype)
        {
            var d = Apply(source, target, amount, attack, ranged, atktype, dmgtype);
            d.Flags[(int)DamageTypes.Physical] = true;

            return d;
        }

        public static DamageInstance ApplyPure(unit source, unit target, float amount, bool attack, bool ranged, attacktype atktype)
        {
            var d = Apply(source, target, amount, attack, ranged, atktype, DAMAGE_TYPE_UNIVERSAL);
            d.Flags[(int)DamageTypes.Pure] = true;

            return d;
        }

        private static DamageInstance CreateFromEvent()
        {
            return Create(GetEventDamageSource(), BlzGetEventDamageTarget(), GetEventDamage(), BlzGetEventIsAttack(), false, BlzGetEventAttackType(), BlzGetEventDamageType(), BlzGetEventWeaponType());
        }

        // Using this to avoid creating the same function ref
        private static void AlarmExec()
        {
            alarmSet = false;
            dreaming = false;
            Enable(true);
            if (totem)
            {
                FailsafeClear();
            }
            else
            {
                canKick = true;
                kicking = false;
                Finish();
            }
            AOEEnd();
            IsCurrent = false;
        }

        private static void OnDamaging()
        {
            try
            {
                var d = CreateFromEvent();
                if (alarmSet)
                {
                    if (totem)
                    {
                        switch (GetHandleId(d.DamageType))
                        {
                            case 20:
                            case 21:
                            case 24:
                                lastInstance = current;
                                IsLastInstance = true;
                                totem = false;
                                canKick = false;
                                break;

                            default:
                                FailsafeClear();
                                break;
                        }
                    }
                    else { Finish(); }

                    if (d.Source != orgSource)
                    {
                        AOEEnd();
                        orgSource = d.Source;
                        orgTarget = d.Target;
                    }
                    else if (d.Target == orgTarget) sourceStacks++;
                    else if (!targets.ContainsKey(d.Target)) sourceAOE++;
                }
                else
                {
                    alarmSet = true;
                    TimerStart(alarm, 0.0f, false, AlarmExec);
                    orgSource = d.Source;
                    orgTarget = d.Target;
                }

                targets[d.Target] = true;
                if (DoPreEvent(d, true))
                {
                    canKick = true;
                    Finish();
                }

                totem = !IsLastInstance || attackImmune[GetHandleId(d.AttackType)] || damageImmune[GetHandleId(d.DamageType)] || !IsUnitType(d.Target, UNIT_TYPE_MAGIC_IMMUNE);
            }
            catch (Exception ex)
            {
                Logger.Error("Damage Engine", ex.Message);
            }
        }

        private static void OnDamaged()
        {
            try
            {
                float r = GetEventDamage();
                var d = current;

                if (prep) prep = false;
                else if (dreaming || d.PrevAmt == 0) return;
                else if (totem) totem = false;
                else
                {
                    AfterDamage();
                    d = lastInstance;
                    current = d;
                    IsLastInstance = false;
                    canKick = true;
                }

                d.Damage = r;
                if (r > 0.0f)
                {
                    RunEvent(DamageEvent.ARMOR);
                    if (hasLethal)
                    {
                        Life = GetWidgetLife(d.Target) - d.Damage;
                        if (Life <= DEATH_DOOR)
                        {
                            RunEvent(DamageEvent.LETHAL);
                            d.Damage = GetWidgetLife(d.Target) - Life;
                        }
                    }
                }
                if (d.DamageType != DAMAGE_TYPE_UNKNOWN) RunEvent(DamageEvent.DAMAGED);

                BlzSetEventDamage(d.Damage);
                eventRuns = true;
                if (d.Damage == 0) Finish();
            }
            catch (Exception ex)
            {
                Logger.Error("Damage Engine", ex.Message);
            }
        }

        public static void OnRecursive()
        {
            try
            {
                AddRecursive(CreateFromEvent());
                BlzSetEventDamage(0.0f);
            }
            catch (Exception ex)
            {
                Logger.Error("Damage Engine", ex.Message);
            }
        }

        public static bool InitEngine()
        {
            for (int i = 0; i <= bj_MAX_PLAYERS; i++)
            {
                player p = Player(i);
                TriggerRegisterPlayerUnitEvent(t1, p, EVENT_PLAYER_UNIT_DAMAGING, null);
                TriggerRegisterPlayerUnitEvent(t2, p, EVENT_PLAYER_UNIT_DAMAGED, null);
                TriggerRegisterPlayerUnitEvent(t3, p, EVENT_PLAYER_UNIT_DAMAGING, null);
            }

            TriggerAddAction(t1, OnDamaging);
            TriggerAddAction(t2, OnDamaged);
            TriggerAddAction(t3, OnRecursive);
            DisableTrigger(t3);

            return true;
        }

    };
}
