// ------------------------------------------------------------------------------
// <copyright file="Packet.cs" company="Kray Oristine">
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source.GameSystem.Sync
{
    /// <summary>
    /// A class that handle all data-management
    /// </summary>
    public sealed class SyncPacket : IDisposable
    {
        private static readonly Stack<SyncPacket> _cache = new Stack<SyncPacket>();
        public string Data { get; private set; }
        public int Owner { get; private set; }

        private SyncPacket(string data, int pid)
        {
            Data = data;
            Owner = pid;
        }

        /// <summary>
        /// Create a new packet with given data
        /// </summary>
        /// <param name="data">Limited to 255 per packet, if exceed data will be cut</param>
        /// <param name="playerId">The owner player id of this packet</param>
        /// <returns>A sync packet</returns>
        public static SyncPacket Create(string data, int playerId)
        {
            if (data.Length > 255) data = data[..255];

            if (_cache.Count == 0) return new(data, playerId);

            var packet = _cache.Pop();
            packet.Data = data;
            packet.Owner = playerId;
            return packet;
        }

        public void Dispose()
        {
            _cache.Push(this);
        }
    }
}
