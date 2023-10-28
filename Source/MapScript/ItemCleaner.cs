// ------------------------------------------------------------------------------
// <copyright file="ItemCleaner.cs" company="Kray Oristine">
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
using Source.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static War3Api.Common;
using War3Api;
using System.Net.Security;

namespace Source.MapScript
{
    public static class ItemCleaner
    {
        private static readonly Lua.Table deadStack = new();

        private static void AddCleaning()
        {
            var i = GetEnumItem();
            if (i != null) deadStack.Insert(i);
        }

        private static void Cleanup()
        {
            if (deadStack.Length > 0)
            {
                for (int i = deadStack.Length; i > 0; i--)
                {
                    var itm = deadStack.Remove(i);
                    itm.Life = 1;
                    itm.Dispose();
                }
            }

            EnumItemsInRect(Blizzard.bj_mapInitialPlayableArea, null, AddCleaning);
        }

        public static void Init()
        {
            var t = new timer();
            t.Start(60f, Cleanup, true);
        }
    }
}
