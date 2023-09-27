// ------------------------------------------------------------------------------
// <copyright file="BaseItem.cs" company="Kray Oristine">
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
using System.Text;
using static War3Api.Common;
using Source.GameSystem.Hero;

namespace Source.GameSystem.Item
{
    /// <summary>
    /// Base class for items, that contain field that always exists in all items object
    /// </summary>
    public abstract class GameItem
    {
        /// <summary>
        /// Item id, in script database, not in-game
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Item name
        /// </summary>
        public string Name { get; private set; } = string.Empty;
        /// <summary>
        /// Item description
        /// </summary>
        public string Description { get; private set; } = string.Empty;
        /// <summary>
        /// Level of this item
        /// </summary>
        public uint Level { get; private set; } = 0;
        /// <summary>
        /// Level requirement to wield this item
        /// </summary>
        public uint LevelRequirement { get; private set; } = 0;
        /// <summary>
        /// Class limitation, like it name suggest..., limit this item to only be wield-able by all of the class it defined <br/>
        /// This is a bit-field, for more information please see <see cref="HeroClass.ClassType"/>
        /// </summary>
        public uint ClassLimitation { get; private set; } = 0;
        /// <summary>
        /// Rarity of the item<br/>
        /// This is a bit-field but only higher bit is used, other lower one are ignored<br/>
        /// for more information please see <see cref="ItemBitField.Rarities"/>
        /// </summary>
        public uint Rarity { get; private set; } = 0;
        /// <summary>
        /// Item type, a single item can have multiple types<br/>
        /// This is a bit-field, for more information please see <see cref="ItemBitField.Types"/>
        /// </summary>
        public uint Type { get; private set; } = 0;
        /// <summary>
        /// Item prefix, a single item could have more one prefix<br/>
        /// This is a bit-field, for more information please see <see cref="ItemBitField.Prefixes"/>
        /// </summary>
        public uint Prefix { get; private set; } = 0;

        /// <summary>
        /// Item suffix, a single item could have more than one suffix<br/>
        /// This is a bit-field, for more information please see <see cref="ItemBitField.Suffixes"/>
        /// </summary>
        public uint Suffix { get; private set; } = 0;
        /// <summary>
        /// Item flags, a single can have multiple flag at once<br/>
        /// This is a bit-field, for more information please see <see cref="ItemBitField.Flags"/>
        /// </summary>
        public uint Flags { get; private set; } = 0;

        /// <summary>
        /// Check if a target is qualified to wield this item.
        /// </summary>
        /// <param name="wieldTarget">Wielding target</param>
        /// <returns>Whether the target is qualified or not</returns>
        public bool IsMatchRequirement(unit wieldTarget)
        {
            if (wieldTarget == null) return false;
            if (ClassLimitation != (uint)HeroClass.ClassType.None && (ClassLimitation & HeroClass.GetClassType(wieldTarget)) > 0) return false;
            if (GetUnitLevel(wieldTarget) < LevelRequirement) return false;
            if ((Flags & (uint)ItemBitField.Flags.Disabled) > 0) return false;
            return true;
        }

        /// <summary>
        /// Everyone want this ain't they?
        /// </summary>
        public void UpdateItem()
        {

        }
    }
}
