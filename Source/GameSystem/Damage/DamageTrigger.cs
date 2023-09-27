// ------------------------------------------------------------------------------
// <copyright file="DamageTrigger.cs" company="Kray Oristine">
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

namespace Source.GameSystem.Damage
{
    public sealed class DamageTrigger
    {
        public Action trig;
        /// <summary>
        /// Indicate this trigger is frozen, will not executed anymore
        /// </summary>
        public bool isFrozen;
        /// <summary>
        /// Set this to true to allow this trigger instance to dream
        /// </summary>
        public bool allowInception;
        /// <summary>
        /// How deep is this trigger instance has slept for
        /// </summary>
        public int InceptionFloor;
        public int weight;
        public int minAOE;
        public bool isEmpty = false;

        private static readonly Stack<DamageTrigger> cache = new();

        private DamageTrigger(Action func, int priority)
        {
            trig = func;
            weight = priority;
            isFrozen = false;
            allowInception = false;
            InceptionFloor = 0;
            minAOE = 1;
            isEmpty = false;
        }

        private static void Update(DamageTrigger trigger, Action method, int priority)
        {
            trigger.trig = method;
            trigger.weight = priority;
            trigger.isFrozen = false;
            trigger.allowInception = false;
            trigger.InceptionFloor = 0;
            trigger.minAOE = 1;
            trigger.isEmpty = false;
        }

        public static DamageTrigger Create(Action method, int priority)
        {
            if (cache.Count > 0)
            {
                var dg = cache.Pop();
                Update(dg, method, priority);
                return dg;
            }

            return new(method, priority);
        }

        public static void Recycle(DamageTrigger trigger)
        {
            trigger.isEmpty = true;
            cache.Push(trigger);
        }

        public void Recycle()
        {
            cache.Push(this);
        }
    }
}
