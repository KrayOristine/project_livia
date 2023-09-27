// ------------------------------------------------------------------------------
// <copyright file="DamageEvent.cs" company="Kray Oristine">
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
namespace Source.GameSystem.Damage
{
    public enum DamageEvent
    {
        /// <summary>
        /// Once the damage is initialized
        /// </summary>
        DAMAGE,

        /// <summary>
        /// After Warcraft 3 engine armor calculation
        /// </summary>
        ARMOR,

        /// <summary>
        /// Before that damage is finalized (is dealt)
        /// </summary>
        DAMAGED,

        /// <summary>
        /// After the damage is dealt
        /// </summary>
        AFTER,

        /// <summary>
        /// On specific target that damage multiple times
        /// </summary>
        SOURCE,

        /// <summary>
        /// On damage that deal lethal blow!
        /// </summary>
        LETHAL
    }
}
