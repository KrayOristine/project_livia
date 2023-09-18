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

/*
    Warcraft 3 C# Damage Engine
    This was made by converting Bribe Damage Engine from Lua and JASS into C#
    This converted version includes minor changes to the engine to optimize it

    Current Version: 1.0
 */

namespace Source.GameSystem.Damage
{
    public sealed class DamageTrigger
    {
        public Action trig;
        public bool isFrozen;
        public bool isInception;
        public int sleepDepth;
        public int weight;
        public int minAOE;
        public bool isEmpty = false;

        private static readonly Stack<DamageTrigger> cache = new();

        private DamageTrigger(Action func, int priority)
        {
            trig = func;
            weight = priority;
            isFrozen = false;
            isInception = false;
            sleepDepth = 0;
            minAOE = 1;
            isEmpty = false;
        }

        private static void Update(DamageTrigger trigger, Action method, int priority)
        {
            trigger.trig = method;
            trigger.weight = priority;
            trigger.isFrozen = false;
            trigger.isInception = false;
            trigger.sleepDepth = 0;
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
