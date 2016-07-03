using System;
using KNXNet.Placeholders;

namespace KNXNet.Packets.Core
{
    public class SearchRequest
    {
        private KNXNetIPHeader Header { get; } = new KNXNetIPHeader() {HeaderSize = 0x06, Version = 0x10, ServiceType = 0x0201};
        public KNXNetIPHPAI DiscoveryEndpoint { get; set; } = new KNXNetIPHPAI();

        public byte[] GetBytes()
        {
            byte[] hpai = DiscoveryEndpoint.GetBytes();

            Header.Size = (short)(6 + hpai.Length);

            byte[] buffer = new byte[Header.Size];

            Array.Copy(Header.GetBytes(), 0, buffer, 0, 6);
            Array.Copy(hpai, 0, buffer, 6, hpai.Length);
            
            return buffer;
        }
    }
}
