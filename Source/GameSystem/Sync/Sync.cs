using Source.GameSystem.WCObject;
using System;
using System.Collections.Generic;
using System.Text;
using War3Api;

namespace Source.GameSystem.Sync
{
    /// <summary>
    /// Main class that handle all <see cref="SyncPacket"/> and it networking<br/>
    /// </summary>
    /// <remarks>
    /// This system reserve _sh and _sp for it own usage!, nothing else should be using it!
    /// </remarks>
    public static class Sync
    {
        public const int PACKET_SIZE = 255; // 255 default - reduce as needed
        public const string HEADER_PREFIX = "_sh";
        public const string PACKET_PREFIX = "_sp";

        private static Trigger headerTrigger = new Trigger();
        private static Trigger packetTrigger = new Trigger();

        /// <summary>
        /// Use this before you begin to send multiple data!
        /// </summary>
        public static void BeginSync()
        {

        }

        public static void OnHeaderSync()
        {

        }

        public static void OnPacketSync()
        {

        }

        private static bool Init()
        {
            headerTrigger.AddAction(OnHeaderSync);
            packetTrigger.AddAction(OnPacketSync);
            for (int i = 0; i < 24; i++)
            {
                headerTrigger.OnSyncEvent(Common.Player(i), HEADER_PREFIX);
                packetTrigger.OnSyncEvent(Common.Player(i), PACKET_PREFIX);
            }
            return true;
        }
    }
}
