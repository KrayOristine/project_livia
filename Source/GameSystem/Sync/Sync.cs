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
using Source.GameSystem.W3OOP;
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
