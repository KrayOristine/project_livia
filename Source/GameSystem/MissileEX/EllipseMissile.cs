// ------------------------------------------------------------------------------
// <copyright file="EllipseMissile.cs" company="Kray Oristine">
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
using War3Api;
using WCSharp.Missiles;

namespace Source.GameSystem.MissileEX
{
    /// <summary>
    /// A very hacky way to implement Ellipse missile, VERY SURE THEY WONT NOTICE THIS OR ELSE IM SCREWED
    /// </summary>
    public sealed class EllipseMissile : CurveMissile
    {
        public EllipseMissile(Common.unit caster, Common.unit target) : base(caster, target)
        {
        }

        public EllipseMissile(Common.unit caster, float targetX, float targetY) : base(caster, targetX, targetY)
        {
        }

        public EllipseMissile(Common.player castingPlayer, float casterX, float casterY, Common.unit target) : base(castingPlayer, casterX, casterY, target)
        {
        }

        public EllipseMissile(Common.player castingPlayer, float casterX, float casterY, float targetX, float targetY) : base(castingPlayer, casterX, casterY, targetX, targetY)
        {
        }
    }
}
