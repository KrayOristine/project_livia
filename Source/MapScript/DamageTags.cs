﻿// ------------------------------------------------------------------------------
// <copyright file="DamageTags.cs" company="Kray Oristine">
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
using Source.Shared;
using System;
using System.Collections.Generic;
using static War3Api.Common;
using static War3Api.Blizzard;
using WCSharp.Events;
using Source.GameSystem.Damage;

namespace Source.MapScript
{
    public sealed class StackingDamageTag : IPeriodicDisposableAction
    {
        private static readonly Stack<StackingDamageTag> cache = new();
        private static readonly PeriodicDisposableTrigger<StackingDamageTag> trig = new(0.01f);

        public unit Target;
        public string Text = "";
        public float Damage = 0f;
        public texttag Tag;
        public float endOfLife = 0f;
        public bool Active { get; set; }

        private StackingDamageTag(unit u)
        {
            Target = u;
            Tag = CreateTextTag();
            SetTextTagPermanent(Tag, true);
            SetTextTagPos(Tag, GetUnitX(u), GetUnitY(u), 125);
        }

        public void Dispose()
        {
            DestroyTextTag(Tag);
            Text = "";
            Damage = 0f;
            endOfLife = 0f;
            Active = false;
            cache.Push(this);
            DamageTag.StackDispose(Target);
        }

        public void Action()
        {
            endOfLife += 0.01f;
            if (!UnitAlive(Target) || endOfLife == 5)
            {
                Active = false;
                return;
            }
            SetTextTagPos(Tag, GetUnitX(Target), GetUnitY(Target), 100);
        }

        public static StackingDamageTag Create(unit target)
        {
            if (cache.Count == 0) return new(target);

            return cache.Pop().Update(target);
        }

        /// <summary>
        /// Update the target unit
        /// </summary>
        /// <param name="u">The unit target unit</param>
        /// <returns>The same <see cref="StackingDamageTag"/> object</returns>
        public StackingDamageTag Update(unit u)
        {
            Target = u;
            Tag = CreateTextTag();
            endOfLife = 0;
            SetTextTagPermanent(Tag, true);
            SetTextTagPos(Tag, GetUnitX(u), GetUnitY(u), 100);
            trig.Add(this);
            return this;
        }

        /// <summary>
        /// Update the amount of damage
        /// </summary>
        /// <param name="newDamage">The amount of damage to change</param>
        /// <param name="add">If <see langword="true"/> - will instead add rather than set</param>
        /// <param name="size">The size of the new text</param>
        /// <returns>The same <see cref="StackingDamageTag"/> object</returns>
        public StackingDamageTag Update(string newText, float newDamage, bool add = true, float size = 0.024f)
        {
            Text = newText;
            Damage = (add ? Damage + newDamage : newDamage);
            endOfLife = 0;

            SetTextTagText(Tag, Text, size);
            return this;
        }
    }
    public static class DamageTag
    {
        private static readonly Dictionary<unit, StackingDamageTag> damageStack = new(); // for stacking damage option, which is enabled by default
        private static readonly Color chaosStart = Color.FromArgb(0x00C8A2C8); // lilac
        private static readonly Color chaosEnd = Color.FromArgb(0x0039254D); // dark purple

        private static string GetDamageColor(List<bool> flags, bool attack, float amount)
        {
            if (amount < 0.0f || flags[(int)DamageFlag.Heal]) return "|c0096FF96+ "; // green
            else if (flags[(int)DamageFlag.Pure] || flags[(int)DamageFlag.RAW] || flags[(int)DamageFlag.INTERNAL]) return "|c00FFFFFF"; // white
            else if (flags[(int)DamageFlag.Evaded] || flags[(int)DamageFlag.Shield]) return "|c00808080"; // gray
            else if (flags[(int)DamageFlag.Critical]) return "|c00E10600"; // slightly red
            else if (flags[(int)DamageFlag.Physical] || attack) return "|c00FF7F00"; // orange
            else if (flags[(int)DamageFlag.Magical] || flags[(int)DamageFlag.Spell]) return "|c002389DA"; // slightly darker blue
            else if (flags[(int)DamageFlag.Elemental])
            {
                if (flags[(int)DamageFlag.Fire]) return "|c00CD001A"; // cherry red
                else if (flags[(int)DamageFlag.Water]) return "|c0074CCF4"; // very light blue
                else if (flags[(int)DamageFlag.Nature]) return "|c002D5A27"; // green leaf color
                else if (flags[(int)DamageFlag.Metal]) return "|c00FFCC00"; // UCS gold
                else if (flags[(int)DamageFlag.Wind]) return "|c00D1F1F9"; // wind
                else if (flags[(int)DamageFlag.Lightning]) return "|c00C8A2C8"; // lilac
                else if (flags[(int)DamageFlag.Light]) return "|c00FFFFED"; // light yellow
                else if (flags[(int)DamageFlag.Dark]) return "|c0039254D"; // dark purple
            }

            return "|c00E45AAA"; // Some how it passed all filter, let indicate it a 'error damage'
        }

        private static void NormalDamageTag(DamageInstance d, unit target)
        {
            string colorized;
            if (d.Flags[(int)DamageFlag.Chaos])
            {
                colorized = Color.GenerateApplyGradient(d.Damage.ToString(), chaosStart, chaosEnd);
            }
            else
            {
                colorized = GetDamageColor(d.Flags, d.IsAttack, d.Damage) + d.Damage + "|r";
            }
            ArcingTT.Create(colorized, target, GetUnitX(target), GetUnitY(target), 2f, 2, GetLocalPlayer());
        }

        internal static void StackDispose(unit u)
        {
            if (damageStack.ContainsKey(u))
            {
                damageStack.Remove(u);
            }
        }

        private static void StackDamageTag(DamageInstance d, unit target)
        {
            StackingDamageTag stack;
            string colorized;

            if (damageStack.ContainsKey(target))
            {
                stack = damageStack[target];
            }
            else
            {
                stack = StackingDamageTag.Create(target);
            }

            if (string.IsNullOrEmpty(stack.Text))
            {
                if (d.Flags[(int)DamageFlag.Chaos])
                {
                    colorized = Color.GenerateApplyGradient(d.Damage.ToString(), chaosStart, chaosEnd);
                }
                else
                {
                    colorized = $"{GetDamageColor(d.Flags, d.IsAttack, d.Damage)}{d.Damage}|r";
                }

                stack.Update(colorized, d.Damage, false);
                return;
            }

            float old = stack.Damage;
            float newDmg = old + d.Damage;
            if (d.Flags[(int)DamageFlag.Chaos])
            {
                colorized = Color.GenerateApplyGradient(newDmg.ToString(), chaosStart, chaosEnd);
            } else
            {
                colorized = $"{GetDamageColor(d.Flags, d.IsAttack, d.Damage)}{newDmg}|r";
            }

            stack.Update(colorized, newDmg, false);
        }

        private static void OnDamage()
        {
            try
            {

                var d = Engine.Current;
                int id = GetPlayerId(d.SourcePlayer);
                if (disabled[id]) return;
                if (enabled[id] == 2) NormalDamageTag(d, d.Target);
                else StackDamageTag(d, d.Target);

            }
            catch (Exception ex)
            {
                Logger.Error("Damage Tags", ex.Message);
            }
        }

        private static void OnChatEvent()
        {
            string chat = GetEventPlayerChatString();
            string[] args = chat.ToLower()[9..].Split(' ');
            player p = GetTriggerPlayer();
            int id = GetPlayerId(p);

            foreach (string arg in args)
            {
                if (string.IsNullOrEmpty(arg)) continue;

                if (arg == "stack" || arg == "stacking" || arg == "merge" || arg == "merging")
                {
                    enabled[id] = 2;
                    DisplayTimedTextToPlayer(p, 0.2f, 0.5f, 10, "Floating damage tags merging has been enabled");
                    continue;
                }
                else if (arg == "no stack" || arg == "no stacking" || arg == "no merge" || arg == "no merging")
                {
                    enabled[id] = 1;
                    DisplayTimedTextToPlayer(p, 0.2f, 0.5f, 10, "Floating damage tags merging has been disabled");
                    continue;
                }

                if (arg  == "enable")
                {
                    disabled[id] = false;
                    enabled[id] = 1;
                    DisplayTimedTextToPlayer(p, 0.2f, 0.5f, 10, "Floating damage tags is enabled");
                }
                else if (arg == "disable")
                {
                    disabled[id] = true;
                    DisplayTimedTextToPlayer(p, 0.2f, 0.5f, 10, "Floating damage tags is disabled");
                }
            }
        }

        private static readonly trigger trig = CreateTrigger();
        private static readonly bool[] disabled = new bool[bj_MAX_PLAYERS];
        private static readonly int[] enabled = new int[bj_MAX_PLAYERS];
        public static void Init()
        {
            for (int i = 0; i < bj_MAX_PLAYERS; i++)
            {
                TriggerRegisterPlayerChatEvent(trig, Player(i), "-damage", false);
            }
            TriggerAddAction(trig, OnChatEvent);
            Engine.Register(DamageEvent.DAMAGED, 9999999, OnDamage);
        }
    }
}
