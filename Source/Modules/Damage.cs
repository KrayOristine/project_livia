using Source.Utils;
using System;
using System.Collections.Generic;
using static War3Api.Blizzard;
using static War3Api.Common;

/*
	WC3Sharp Damage Engine by Ozzzzymaniac
	Original version by BribeFromTheHive

	Current Version: 1.0
 */

namespace Source.Modules
{
	public enum DamageTypes
	{
		None,
		Physical,
		Magical,
		Pure, // Bypass armor damage reduction
		Evasion, // Set damage to 0
		Critical, // Dealt extra damage
		Heal, // Is heal
		Shield, // Negated by shield

		// System flags

		Spell, // Came from abilities that not specify damage type by default
		Periodic, // Came from damage over times
		Item, // Came from items
		AOE, // Is an AOE damage

		// Engine Flags

		RAW, // Bypass all modification except the engine it self
		INTERNAL, // Ignore the engine modification
		PET, // PET!

		// MUST BEHIND ALL OTHER FLAGS
		LAST
	}

	public struct DamageInstance
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
		public bool[] Flags;
		public attacktype AttackType;
		public damagetype DamageType;
		public weapontype WeaponType;
		public readonly float prevAmt;
		public readonly attacktype PrevAttackType;
		public readonly damagetype PrevDamageType;
		public readonly weapontype PrevWeaponType;
		internal DamageTrigger? recursive;

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

		public DamageInstance(unit src, unit tgt, float dmg, bool iatk, bool irgd, attacktype tatk, damagetype tdmg, weapontype twpn)
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
			Flags = new bool[15];
			Array.Fill(Flags, false);
			prevAmt = dmg;
			PrevAttackType = tatk;
			PrevDamageType = tdmg;
			PrevWeaponType = twpn;
			recursive = null;
		}
	}

	public struct DamageTrigger
	{
		public Action trig;
		public bool isFrozen;
		public bool isInception;
		public int sleepDepth;
		public int weight;
		public int minAOE;

		public DamageTrigger(Action func, int priority)
		{
			trig = func;
			weight = priority;
			isFrozen = false;
			isInception = false;
			sleepDepth = 0;
			minAOE = 1;
		}
	}

	public enum DamageEvent
	{
		DAMAGE, // Upon the damage first run
		ARMOR, // Armor event run
		DAMAGED,
		AFTER,
		SOURCE,
		LETHAL
	}

	public static class Damage
	{
		internal static readonly int LIMBO_DEPTH = 8; // Maximum amount of times the engine itself can dream
		internal static readonly float DEATH_DOOR = 0.405f; // If blizzard decided to change this, it should be a simple fix here

		// Declare all used variables of the engine
		private static trigger t1;

		private static trigger t2;
		private static trigger t3;
		private static timer alarm;
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
		private static bool isCurrent = false;
		private static DamageInstance lastInstance;
		private static bool isLastInstance = false;
		private static bool skipEngine = false;
		private static readonly List<DamageInstance> recursiveStacks = new();
		private static readonly Dictionary<unit, bool> recursiveSource = new();
		private static readonly Dictionary<unit, bool> recursiveTarget = new();
		private static bool prep = false;

		public static DamageInstance Current
		{
			get => current;
		}

		public static float Life { get; set; } = 0.0f;
		public static int SourceStacks { get => sourceStacks; }

		private static bool modifyNext = false;
		private static unit? n_src;
		private static unit? n_tgt;
		private static float? n_dmg;
		private static attacktype? n_tatk;
		private static damagetype? n_tdmg;
		private static weapontype? n_twpn;

		public static void ModifyNext(unit? src, unit? tgt, float? dmg, attacktype? tatk, damagetype? tdmg, weapontype? twpn)
		{
			if (modifyNext) return; // A next is already queued, ignore request

			modifyNext = true;
			n_src = src;
			n_tgt = tgt;
			n_dmg = dmg;
			n_tatk = tatk;
			n_tdmg = tdmg;
			n_twpn = twpn;
		}

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

		public static LinkedListNode<DamageTrigger>? Register(DamageEvent whichEvent, int priority, Action callback)
		{
			var head = eventList[(int)whichEvent];
			if ((int)whichEvent >= (int)DamageEvent.SOURCE)
			{
				hasSource = hasSource || whichEvent == DamageEvent.SOURCE;
				hasLethal = hasLethal || whichEvent == DamageEvent.LETHAL;
			}

			var data = new DamageTrigger(callback, priority);
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
			() => current.Flags[(int)DamageTypes.Pure] || skipEngine,
			() => current.Damage <= 0,
			() => current.DamageType == DAMAGE_TYPE_UNKNOWN,
			() => current.DamageType == DAMAGE_TYPE_UNKNOWN,
			() => false,
			() => false
		};

		private static void RunEvent(DamageEvent whichEvent)
		{
			try
			{
				var head = eventList[(int)whichEvent];
				var checks = breakChecks[(int)whichEvent];
				//Logger.Debug("DE", "dreaming: " + dreaming + " | checks: " + checks.Invoke() + " | head.First != null: " + (head.First != null) + " | events: " + whichEvent.ToString());
				if (dreaming || checks.Invoke() || head.First == null) return;

				var node = head.First;
				userIndex = node.Value;
				Damage.Enable(false);
				EnableTrigger(t3);
				dreaming = true;
				//Logger.Debug("Damage Engine", "RunEvent is running for " + (int)whichEvent);

				while (true)
				{
					if (!userIndex.isFrozen && !hasSource || (whichEvent != DamageEvent.SOURCE || sourceAOE > userIndex.minAOE))
					{
						userIndex.trig.Invoke();
					}
					if (node.Next == null) break;

					node = node.Next;
					userIndex = node.Value;
				}

				//Logger.Debug("Damage Engine", "RunEvent is finished");
				dreaming = false;
				Damage.Enable(true);
				DisableTrigger(t3);
			}
			catch (Exception ex)
			{
				Logger.Error("Damage Engine", "RunEvent - " + ex.Message);
			}
		}

		private static DamageInstance Create(unit src, unit tgt, float dmg, bool iatk, bool irgd, attacktype tatk, damagetype tdmg, weapontype twpn)
		{
			var d = new DamageInstance(src, tgt, dmg, iatk, irgd, tatk, tdmg, twpn);
			if (modifyNext)
			{
				if (n_src != null) d.Source = n_src;
				if (n_tgt != null) d.Target = n_tgt;
				d.Damage = (float)(n_dmg is null ? dmg : n_dmg);
				d.AttackType = n_tatk is null ? tatk : n_tatk;
				d.DamageType = n_tdmg is null ? tdmg : n_tdmg;
				d.WeaponType = n_twpn is null ? twpn : n_twpn;

				n_src = null;
				n_tgt = null;
				n_dmg = null;
				n_tatk = null;
				n_tdmg = null;
				n_twpn = null;
				modifyNext = false;
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
			if (isCurrent)
			{
				RunEvent(DamageEvent.AFTER);
				isCurrent = false;
			}
			skipEngine = false;
		}

		private static bool DoPreEvent(DamageInstance d, bool isNatural)
		{
			try
			{
				current = d;
				recursiveSource.Add(d.Source, true);
				recursiveTarget.Add(d.Target, true);
				if (d.Damage == 0.0f) return false;
				skipEngine = d.DamageType == DAMAGE_TYPE_UNKNOWN;
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
			catch (Exception ex)
			{
				Logger.Error("Damage Engine", "DoPreEvent - " + ex.Message);
				return false;
			}
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
			isCurrent = false;
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
					if (recursiveStacks[i].recursive != null)
					{
						DamageTrigger rs = (DamageTrigger)recursiveStacks[i].recursive;
						rs.isFrozen = false;
						rs.sleepDepth = 0;
					}
				}
			}
			recursiveStacks.Clear();
			dreamDepth = 0;
			kicking = false;
			dreaming = false;
			Enable(true);
			recursiveSource.Clear();
			recursiveTarget.Clear();

			//Logger.Debug("Damage Engine", "Finish - Groups cleared");
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
			try
			{
				alarmSet = false;
				dreaming = false;
				Enable(true);
				if (totem) { FailsafeClear(); }
				else
				{
					canKick = true;
					kicking = false;
					Finish();
				}
				AOEEnd();
				isCurrent = false;
			}
			catch (Exception ex)
			{
				Logger.Error("Damage Engine", "AlarmExec - " + ex.Message);
			}
		}

		public static void Init()
		{
			//Logger.Debug("Damage Engine", "Begin Initialization state");
			t1 = CreateTrigger();
			t2 = CreateTrigger();
			t3 = CreateTrigger();
			alarm = CreateTimer();

			for (int i = 0; i <= bj_MAX_PLAYERS; i++)
			{
				player p = Player(i);
				TriggerRegisterPlayerUnitEvent(t1, p, EVENT_PLAYER_UNIT_DAMAGING, null);
				TriggerRegisterPlayerUnitEvent(t2, p, EVENT_PLAYER_UNIT_DAMAGED, null);
				TriggerRegisterPlayerUnitEvent(t3, p, EVENT_PLAYER_UNIT_DAMAGING, null);
			}

			TriggerAddCondition(t1, Condition(() =>
			{
				try
				{
					var d = CreateFromEvent();
					//Logger.Debug("Damage Engine", "Pre-damage event is running for " + GetUnitName(GetTriggerUnit()));
					if (alarmSet)
					{
						if (totem)
						{
							if (d.DamageType == DAMAGE_TYPE_SPIRIT_LINK || d.DamageType == DAMAGE_TYPE_DEFENSIVE || d.DamageType == DAMAGE_TYPE_PLANT)
							{
								lastInstance = current;
								isLastInstance = true;
								totem = false;
								canKick = false;
							}
							else { FailsafeClear(); }
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

					targets.Add(d.Target, true);
					if (DoPreEvent(d, true))
					{
						canKick = true;
						Finish();
					}

					totem = !isLastInstance || attackImmune[GetHandleId(d.AttackType)] || damageImmune[GetHandleId(d.DamageType)] || !IsUnitType(d.Target, UNIT_TYPE_MAGIC_IMMUNE);

					return false;
				}
				catch (Exception ex)
				{
					Logger.Error("Damage Engine", ex.Message);
					return false;
				}
			}));

			TriggerAddCondition(t2, Condition(() =>
			{
				try
				{
					float r = GetEventDamage();
					var d = current;

					//Logger.Debug("Damage Engine", "Second damage event is running for " + GetUnitName(GetTriggerUnit()));
					if (prep) prep = false;
					else if (dreaming || d.prevAmt == 0) return false;
					else if (totem) totem = false;
					else
					{
						AfterDamage();
						d = lastInstance;
						current = d;
						isLastInstance = false;
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

					return false;
				}
				catch (Exception ex)
				{
					Logger.Error("Damage Engine", ex.Message);
					return false;
				}
			}));

			TriggerAddCondition(t3, Condition(() =>
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
				return false;
			}));
			DisableTrigger(t3);

			//Logger.Debug("Damage Engine", "Initialization has been finished");
		}
	};
}