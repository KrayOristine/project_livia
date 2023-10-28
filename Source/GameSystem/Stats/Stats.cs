// ------------------------------------------------------------------------------
// <copyright file="Stats.cs" company="Kray Oristine">
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
using System.Runtime.CompilerServices;
using static War3Api.Common;

// Shorthand for Update, since i lazy as fuck
using UpdateAction = System.Action<War3Api.Common.unit, double, double, Source.GameSystem.Stats.Mode>;

namespace Source.GameSystem.Stats
{

    /// <summary>
    /// Class that _handle stats creation and features
    /// </summary>
    static class ExStats
    {
        internal const int baseId = 1496330289; //"Y001"

        internal static void SetAbility(unit u, int offset, abilityintegerlevelfield field, int value, bool remove = false)
        {
            if (GetUnitAbilityLevel(u, baseId + offset) == 0)
            {
                UnitAddAbility(u, baseId + offset);
                UnitMakeAbilityPermanent(u, true, baseId + offset);
            }
            if (value == 0 && remove)
            {
                UnitRemoveAbility(u, baseId + offset);
                return;
            }
            if (BlzSetAbilityIntegerLevelField(BlzGetUnitAbility(u, baseId + offset), field, 0, value))
            {
                IncUnitAbilityLevel(u, baseId + offset);
                DecUnitAbilityLevel(u, baseId + offset);
            }
        }

        internal static void SetAbility(unit u, int offset, abilityreallevelfield field, float value, bool remove = false)
        {
            if (GetUnitAbilityLevel(u, baseId + offset) == 0)
            {
                UnitAddAbility(u, baseId + offset);
                UnitMakeAbilityPermanent(u, true, baseId + offset);
            }
            if (value == 0 && remove)
            {
                UnitRemoveAbility(u, baseId + offset);
                return;
            }
            if (BlzSetAbilityRealLevelField(BlzGetUnitAbility(u, baseId + offset), field, 0, value))
            {
                IncUnitAbilityLevel(u, baseId + offset);
                DecUnitAbilityLevel(u, baseId + offset);
            }
        }

        //CSharpLua tricks
        public static readonly bool Stat_Initialized = InitStats();

        private static bool InitStats()
        {
            // Base Distribution Stats



            // Bonus stats


            return true;
        }
    }
}
