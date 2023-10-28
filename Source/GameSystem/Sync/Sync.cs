// ------------------------------------------------------------------------------
// <copyright file="Sync.cs" company="Kray Oristine">
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
using System.Net.Security;
using System.Text;
using War3Api;
using static War3Api.Common;

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
        public const int PACKET_SIZE = 256; // 255 default - reduce as needed
        public const string HEADER_PREFIX = "_sh";
        public const string PACKET_PREFIX = "_sp";

        private static readonly trigger headerTrigger = new();
        private static readonly trigger packetTrigger = new();
        private static readonly StringBuilder[] buffer = new StringBuilder[28];
        private static bool initialized = false;

        /// <summary>
        /// Use this before you begin to send multiple data with unknown amount of time you are going to send
        /// </summary>
        public static void Begin()
        {
            BlzSendSyncData(HEADER_PREFIX, "await");
        }

        /// <summary>
        /// Send the data
        /// </summary>
        /// <param name="data"></param>
        public static void Send(string data)
        {

        }

        /// <summary>
        /// Sync the given data with known amount of data being sent so that sync can pre-initialize the amount of storage that is needed
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public static void SendEx(string data, int length)
        {

        }

        /// <summary>
        /// Use this after you finishes sending data
        /// </summary>
        public static void End()
        {
            BlzSendSyncData(HEADER_PREFIX, "end");
        }

        public static void InitBuffer()
        {
            for (int i = 0; i < 28; i++)
            {
                var buff = buffer[i];
                if (buff.Length > 0) buff.Clear();
            }
        }

        private static void OnHeaderSync()
        {
            var source = @event.Player;
            var data = @event.SyncData;
            if (data == "await") return;
            if (int.TryParse(data, out var len))
            {

            }
        }

        public static void OnPacketSync()
        {

        }

        public static void Init()
        {
            if (initialized) return;
            for (int i = 0; i < 28; i++)
            {
                buffer[i] = new();
            }
            headerTrigger.AddAction(OnHeaderSync);
            packetTrigger.AddAction(OnPacketSync);
            for (int i = 0; i < 28; i++)
            {
                var p = new player(i);
                headerTrigger.RegisterSyncEvent(p, HEADER_PREFIX);
                packetTrigger.RegisterSyncEvent(p, PACKET_PREFIX);
            }
            initialized = true;
        }
    }
}
