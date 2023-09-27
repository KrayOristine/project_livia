// ------------------------------------------------------------------------------
// <copyright file="ArcingItem.cs" company="Kray Oristine">
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
using System.Collections.Generic;
using WCSharp.Events;
using static War3Api.Common;

namespace Source.Shared
{
    public sealed class ArcingItem : IPeriodicAction
    {
        private const float ARC = 65; // Modify the arc shape
        private const float TRAVEL_TIME = 1.2f; // Time in second that the arc to reach destination
        private const float CONVERT_ARC = ARC / 100;

        //! MODIFY BELOW CODE AT YOUR OWN RISK

        private const float SPEED_CONVERT = TRAVEL_TIME / PeriodicEvents.SYSTEM_INTERVAL;

        private static readonly PeriodicTrigger<ArcingItem> trigger = new(PeriodicEvents.SYSTEM_INTERVAL);
        private static readonly Stack<ArcingItem> cache = new();

        private int id;
        private effect? sfx;
        private float angle;
        private float distance;
        private float maxDistance;
        private float arc;
        private float speed;
        private float x1;
        private float y1;
        private float x2;
        private float y2;

        public bool Active { get; set; }

        public void Action()
        {
            x1 += speed * Cos(angle);
            y1 += speed * Sin(angle);
            distance = WCSharp.Shared.FastUtil.DistanceBetweenPoints(x1, y1, x2, y2);

            float newHeight = (((4 * arc) / maxDistance) * (maxDistance - distance) * (distance / maxDistance)) + WarEX.GetLocZ(x1, y1);
            BlzSetSpecialEffectPosition(sfx, x1, y1, newHeight);

            if (newHeight <= 0)
            {
                DestroyEffect(sfx);
                CreateItem(id, x1, y1);
                Active = false;
                cache.Push(this);
            }
        }

        private ArcingItem()
        { }

        /// <summary>
        /// Prepare the needed data
        /// </summary>
        /// <returns></returns>
        public ArcingItem Prepare(int itemId, string modelPath, float x1, float y1, float x2, float y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            id = itemId;
            sfx = AddSpecialEffect(modelPath, x1, y1);
            angle = WCSharp.Shared.Util.AngleBetweenPointsRad(x1, y1, x2, y2);
            distance = WCSharp.Shared.FastUtil.DistanceBetweenPoints(x1, y1, x2, y2);
            maxDistance = distance;
            speed = distance / SPEED_CONVERT;
            arc = distance * CONVERT_ARC;

            return this;
        }

        /// <summary>
        /// Create a arcing item
        /// </summary>
        /// <param name="itemId">The item id</param>
        /// <param name="modelPath">The model for the fancy arcing</param>
        /// <param name="x1">Start X</param>
        /// <param name="y1">Start Y</param>
        /// <param name="x2">Destination X</param>
        /// <param name="y2">Destination Y</param>
        /// <returns>An ArcingItem instance</returns>
        public static ArcingItem Create(int itemId, string modelPath, float x1, float y1, float x2, float y2)
        {
            ArcingItem temp;
            if (cache.Count > 0)
            {
                temp = cache.Pop();
            }
            else
            {
                temp = new ArcingItem();
            }

            temp.Prepare(itemId, modelPath, x1, y1, x2, y2);
            trigger.Add(temp);
            return temp;
        }
    }
}
