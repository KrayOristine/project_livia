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
