// ------------------------------------------------------------------------------
// <copyright file="SpecialEffect.cs" company="Kray Oristine">
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
using static War3Api.Common;

namespace Source.Shared
{
    public sealed class SpecialEffect
    {
        // instantly vanishes effect are move to this location before they get destroyed
        private const float privX = 0;
        private const float privY = 0;

        // native
        private float _x;
        private float _y;
        private float _z;
        private float _height;
        private float _yaw;
        private float _pitch;
        private float _roll;

        //war3
        private LinkedList<effect> list = new();

        /// <summary>
        /// Create the special effect at the given location
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public SpecialEffect(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;

        }

        /// <summary>
        /// Create the special effect relatively
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="height"></param>
        public SpecialEffect(float x, float y, float height)
        {
        }

    }
}
