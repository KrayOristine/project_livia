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
using static War3Api.Common;

namespace Source.Shared
{
    public static class Mouse
    {
        private static readonly Dictionary<player, float> x = new();
        private static readonly Dictionary<player, float> y = new();
        private static readonly trigger trigger = CreateTrigger();

        public static float GetX(player p)
        {
            return x[p];
        }

        public static float GetY(player p)
        {
            return y[p];
        }

        public static void OnMouseMoveEvent()
        {
            var p = @event.Player;

            x[p] = @event.PlayerMouseX;
            y[p] = @event.PlayerMouseY;
        }

        public static void Init()
        {
            for (int i = 0; i <= 27; i++)
            {
                var player = new player(i);
                x[player] = 0;
                y[player] = 0;

                if (player.Controller == MAP_CONTROL_USER && player.SlotState == PLAYER_SLOT_STATE_PLAYING)
                {
                    trigger.RegisterPlayerEvent(player, EVENT_PLAYER_MOUSE_MOVE);
                }
            }
            trigger.AddAction(OnMouseMoveEvent);
        }
    }
}
