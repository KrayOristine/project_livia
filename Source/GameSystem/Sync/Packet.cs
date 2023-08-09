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
