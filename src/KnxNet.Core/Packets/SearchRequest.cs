using System;
using KnxNet.Core.Placeholders;

namespace KnxNet.Core.Packets
{
    public class SearchRequest
    {
        private KnxNetIPHeader Header { get; } = new KnxNetIPHeader() {HeaderSize = 0x06, Version = 0x10, ServiceType = 0x0201};
        public KnxNetIPHPAI DiscoveryEndpoint { get; set; } = new KnxNetIPHPAI();

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
