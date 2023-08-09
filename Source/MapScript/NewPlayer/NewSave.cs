using Source.GameSystem.Save;
using Source.GameSystem.WCObject;
using System.Collections.Generic;
using static War3Api.Common;

namespace Source.MapScript.NewPlayer
{
    public static class NewSave
    {
        private static readonly Frame textFrame = new("EscMenuEditBoxTemplate", "newsave_secretcode", Frame.GetOriginFrame(Frame.OriginType.GAME_UI, 0), "", 1);
        private static readonly Trigger frameTrigger = new();
        private static readonly Queue<player> queue = new();

        public static void TriggerFor(player whichPlayer)
        {
            queue.Enqueue(whichPlayer);
            DisplayTimedTextToPlayer(whichPlayer, 0, 0, 3, "Creating your profile.....");
            DisplayTimedTextToPlayer(whichPlayer, 0.45f, 0.5f, 10, "Enter |c00ff00006 character|r 'secret' codes");
            if (GetLocalPlayer() == whichPlayer)
            {
                textFrame.SetVisible(true);
            }
        }

        private static void On_Enter()
        {
            player p = GetTriggerPlayer();
            if (!queue.Contains(p)) return;
            string codes = BlzGetTriggerFrameText();
            if (GetLocalPlayer() == p)
            {
                textFrame.SetVisible(false);
            }
            DisplayTimedTextToPlayer(p, 0, 0, 20, "Please remember the codes you entered as it will be used to decrypt your save");
            DisplayTimedTextToPlayer(p, 0, 0, 20, "Forgot it will result in a complete save reset");
            PlayerBank.GetPlayerBank(GetPlayerId(p)).KEY = codes;
        }

        public static bool Init()
        {
            textFrame.RegisterEvent(frameTrigger, Frame.Event.EDITBOX_TEXT_CHANGED);
            frameTrigger.AddAction(On_Enter);
            textFrame.SetVisible(false);
            textFrame.SetAbsolutePoint(Frame.FramePoint.CENTER, 0, 0);
            textFrame.SetSize(0.35f, 0.15f);
            textFrame.SetTextSizeLimit(6);

            return true;
        }
    }
}
