// ------------------------------------------------------------------------------
// <copyright file="NewSave.cs" company="Kray Oristine">
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
using Source.GameSystem.Save;
using Source.GameSystem.W3OOP;
using System.Collections.Generic;
using static War3Api.Common;

namespace Source.MapScript.NewPlayer
{
    public static class NewSave
    {
        private static readonly Frame textFrame = new("EscMenuEditBoxTemplate", "newsave_secretcode", Frame.GetOrigin(Frame.OriginType.GAME_UI, 0), "", 1);
        private static readonly Trigger frameTrigger = new();
        private static readonly Queue<player> queue = new();

        public static void TriggerFor(player whichPlayer)
        {
            queue.Enqueue(whichPlayer);
            DisplayTimedTextToPlayer(whichPlayer, 0, 0, 3, "Creating your profile.....");
            DisplayTimedTextToPlayer(whichPlayer, 0.45f, 0.5f, 10, "Enter |c00ff00006 character|r 'secret' codes");
            if (GetLocalPlayer() == whichPlayer)
            {
                textFrame.Visible = true;
            }
        }

        private static void On_Enter()
        {
            player p = GetTriggerPlayer();
            if (!queue.Contains(p)) return;
            string codes = BlzGetTriggerFrameText();
            if (GetLocalPlayer() == p)
            {
                textFrame.Visible = false;
            }
            DisplayTimedTextToPlayer(p, 0, 0, 20, "Please remember the codes you entered as it will be used to decrypt your save");
            DisplayTimedTextToPlayer(p, 0, 0, 20, "Forgot it will result in a complete save reset");
            PlayerBank.GetPlayerBank(GetPlayerId(p)).KEY = codes;
        }

        public static bool Init()
        {
            textFrame.RegisterEvent(frameTrigger, Frame.Event.EDITBOX_TEXT_CHANGED);
            frameTrigger.AddAction(On_Enter);
            textFrame.Visible = false;
            textFrame.SetAbsolutePoint(Frame.FramePoint.CENTER, 0, 0);
            textFrame.SetSize(0.35f, 0.15f);
            textFrame.TextSizeLimit = 6;

            return true;
        }
    }
}
