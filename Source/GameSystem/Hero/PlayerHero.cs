// ------------------------------------------------------------------------------
// <copyright file="PlayerHero.cs" company="Kray Oristine">
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War3Api.Common;

namespace Source.GameSystem.Hero
{
    public sealed class PlayerHero
    {
        private static readonly Dictionary<unit, PlayerHero> _heroDict = new();
        private readonly unit _handle;
        private int _heroClass;

        public unit Handle { get => _handle; }
        public HeroClass Classes { get => _heroClass; }

        public PlayerHero(player whichPlayer, int uid, HeroClass classType)
        {
            _handle = new(whichPlayer, uid, 0, 0);
            _heroClass = (int)classType;
        }

        public static PlayerHero? FromUnit(unit u) => _heroDict.TryGetValue(u, out var hero) ? hero : null;

        public bool IsClass(HeroClass whichClass) => (_heroClass & (int)whichClass) > 0;
        public bool IsClass(int classBitfield) => (_heroClass & classBitfield) > 0;
    }
}
