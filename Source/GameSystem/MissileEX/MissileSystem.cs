// ------------------------------------------------------------------------------
// <copyright file="MissileSystem.cs" company="Kray Oristine">
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
using Source.GameSystem.W3OOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WCSharp.Events;
using static War3Api.Common;

namespace Source.GameSystem.MissileEX
{

    /// <summary>
    ///  The one that handle the execution of the missile
    /// </summary>
    public static class MissileSystem
    {
        public const float MISSILE_EXECUTION_INTERVAL = 1f / 40f;
        private static readonly Timer tmr = new();
        private static readonly List<RelativisticMissile> missiles = new(256);
        private static int allocId = 0;
        private static int last;

        /// <summary>
        /// All missiles in the system
        /// </summary>
        public static List<RelativisticMissile> Missiles => missiles;

        /// <summary>
        /// Get the amount of missile currently exists in the system.
        /// </summary>
        public static int Active { get => allocId; }

        /// <summary>
        /// Adds the given <paramref name="missile"/> to the system.
        /// </summary>
        public static void Add(RelativisticMissile missile)
        {
            if (missiles.Count == 0)
            {
                tmr.Start(MISSILE_EXECUTION_INTERVAL, Tick, true);
            }

            missile.Launch();
            missiles[allocId] = missile;
            allocId++;
            missile.Action();
        }

        private static void Tick()
        {
            var i = allocId > RelativisticMissile.SWEET_SPOT ? last : 0;
            var j = 0;

            while (j < RelativisticMissile.SWEET_SPOT && j <= allocId)
            {
                var ms = missiles[i];
                if (ms.Active && !ms.Paused) ms.Action();
                else
                {
                    if (ms.Paused) ms.OnPause();
                    else ms.Dispose();
                    missiles[i] = missiles[allocId];
                    missiles.RemoveAt(allocId);
                    allocId--;
                    RelativisticMissile.UpdateDilation();
                    if (allocId == -1) tmr.Pause();
                    j--;
                }
                i++;
                j++;
                if (i > allocId) i = 0;
            }
            last = i;
        }

        /// <summary>
        /// By default, <see cref="RelativisticMissile.CastingPlayer"/> and <see cref="RelativisticMissile.TargetPlayer"/> are not updated when a unit changes owner.
        /// <para>This adds an event to pass over all missiles and update <see cref="RelativisticMissile.CastingPlayer"/> and <see cref="RelativisticMissile.TargetPlayer"/> on ownership changes.</para>
        /// <para>This will ignore ownership changes of unit type 'xxxx' (WCSharp's dummy unit type).</para>
        /// </summary>
        public static void RegisterForOwnershipChanges()
        {
            PlayerUnitEvents.Register(UnitTypeEvent.ChangesOwner, OnUnitTypeChangesOwner);
        }

        private static void OnUnitTypeChangesOwner()
        {
            var unit = GetTriggerUnit();
            if (GetUnitTypeId(unit) == 2021161080)
                return;

            foreach (var missile in missiles)
            {
                if (missile.Caster == unit)
                {
                    missile.CastingPlayer = GetOwningPlayer(unit);
                }
                if (missile.Target == unit)
                {
                    missile.TargetPlayer = GetOwningPlayer(unit);
                }
            }
        }
    }
}
