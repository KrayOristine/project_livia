// ------------------------------------------------------------------------------
// <copyright file="Mouse.cs" company="Kray Oristine">
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

namespace Source.Shared
{
    public static class Mouse
    {
        private static readonly Dictionary<Common.player, float> x = new();
        private static readonly Dictionary<Common.player, float> y = new();
        private static readonly Common.trigger trigger = Common.CreateTrigger();

        public static float GetX(Common.player p)
        {
            return x[p];
        }

        public static float GetY(Common.player p)
        {
            return y[p];
        }

        public static void OnMouseMoveEvent()
        {
            var p = Common.GetTriggerPlayer();

            x[p] = Common.BlzGetTriggerPlayerMouseX();
            y[p] = Common.BlzGetTriggerPlayerMouseY();
        }

        public static void Init()
        {
            for (int i = 0; i <= Blizzard.bj_MAX_PLAYERS; i++)
            {
                var player = Common.Player(i);
                x[player] = 0;
                y[player] = 0;

                if (Common.GetPlayerController(player) == Common.MAP_CONTROL_USER && Common.GetPlayerSlotState(player) == Common.PLAYER_SLOT_STATE_PLAYING)
                {
                    Common.TriggerRegisterPlayerEvent(trigger, player, Common.EVENT_PLAYER_MOUSE_MOVE);
                }
            }
            Common.TriggerAddAction(trigger, OnMouseMoveEvent);
        }
    }
}
