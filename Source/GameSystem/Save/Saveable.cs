// ------------------------------------------------------------------------------
// <copyright file="Saveable.cs" company="Kray Oristine">
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
using War3Api;

/*
 * Ozzzzymaniac Custom Save/Load
 *
 * This is the core function of the maps
 */

namespace Source.GameSystem.Save
{
    public abstract class Saveable
    {
        protected readonly Common.player boundedPlayer;
        protected readonly int boundedSlot;

        protected readonly string hash;

        protected Saveable(Common.player boundPlayer, int boundSlot)
        {
            boundedPlayer = boundPlayer;
            boundedSlot = boundSlot;
            hash = Checksum.Serialize(Common.GetPlayerName(boundPlayer) + boundSlot.ToString());
        }
    }
}
