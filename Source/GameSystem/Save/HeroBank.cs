// ------------------------------------------------------------------------------
// <copyright file="HeroBank.cs" company="Kray Oristine">
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
using War3Api;

namespace Source.GameSystem.Save
{
    public sealed class HeroBank : Saveable
    {
        public int ID { get; private set; } = 0;
        public int Level { get; set; } = 0;
        /// <summary>
        /// No use, but leave it here for future case
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>
        /// Only modify the list, do not create a new list
        /// </summary>
        public float[] Attribute { get; set; } = new float[(int)Stats.StatType.OneshotChance+1];
;        /// <summary>
        /// Only modify the list, do not create a new list
        /// </summary>
        public int Flags { get; set; } = 0;
        /// <summary>
        /// The id of the item at the specific slot
        /// </summary>
        public InventoryBank Inventory { get; set; }
        /// <summary>
        /// The save code of this bank
        /// </summary>
        public string SaveCode { get; private set; } = string.Empty;


        public HeroBank(Common.player player, int slot) : base(player, slot)
        {
            Inventory = new InventoryBank(player, slot);
        }
    }
}
