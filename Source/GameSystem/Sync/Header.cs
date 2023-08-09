using System;
using System.Collections.Generic;
using System.Text;

namespace Source.GameSystem.Sync
{
    public sealed class Header : IDisposable
    {
        public int SyncOwner { get; private set; }
        public int IncomingPacketAmount { get; private set; }
        public int[] IncomingDataType { get; private set; }

        private static readonly Stack<Header> _cache = new Stack<Header>();

        private Header(int owner, int incomePacket, int[] incomeData)
        {
            SyncOwner = owner;
            IncomingPacketAmount = incomePacket;
            IncomingDataType = incomeData;
        }

        private Header()
        {
            SyncOwner = -1;
            IncomingPacketAmount = 0;
            IncomingDataType = Array.Empty<int>();
        }

        /// <summary>
        /// Create a new sync header to be used
        /// </summary>
        /// <param name="ownerPlayerId"></param>
        /// <param name="packetAmount"></param>
        /// <param name="dataTypeList"></param>
        /// <returns></returns>
        public static Header Create(int ownerPlayerId, int packetAmount, int[] dataTypeList)
        {
            if (_cache.Count == 0) return new Header(ownerPlayerId, packetAmount, dataTypeList);

            var h = _cache.Pop();
            h.SyncOwner = ownerPlayerId;
            h.IncomingPacketAmount = packetAmount;
            h.IncomingDataType = dataTypeList;
            return h;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(SyncOwner);
            result.Append('-');
            result.Append(IncomingPacketAmount);
            result.Append('-');
            foreach (var dtype in IncomingDataType) result.Append(dtype);

            return result.ToString();
        }

        public static Header FromString(string headerData)
        {
            var data = headerData.Split('-');
            var header = (_cache.Count == 0 ? new Header() : _cache.Pop());
            header.SyncOwner = int.Parse(data[0]);
            header.IncomingPacketAmount = int.Parse(data[1]);
            var dataList = new List<int>();
            for (int i = 0; i < data[2].Length; i++)
            {
                dataList.Add(int.Parse(data[2].Substring(i,i+1)));
            }
            header.IncomingDataType = dataList.ToArray();

            return header;
        }

        public void Dispose()
        {
            _cache.Push(this);
        }
    }
}
