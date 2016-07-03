using System;
using KNXNet.Placeholders;

namespace KNXNet.Packets.Core
{
    public class ConnectRequest
    {
        private KNXNetIPHeader Header { get; set; } = new KNXNetIPHeader() {HeaderSize = 0x06, Version = 0x10, ServiceType = 0x0205};
        
        public KNXNetIPHPAI ControlEndpoint { get; set; }
        public KNXNetIPHPAI DataEndpoint { get; set; }
        public KNXNetIPCRI ConnectionRequest { get; set; }


        public byte[] GetBytes()
        {
            byte[] hpai1 = ControlEndpoint.GetBytes();
            byte[] hpai2 = DataEndpoint.GetBytes();
            byte[] cri = ConnectionRequest.GetBytes();

            Header.Size = (short)(6 + hpai1.Length + hpai2.Length + cri.Length);

            byte[] buffer = new byte[Header.Size];

            Array.Copy(Header.GetBytes(), 0, buffer, 0, 6);
            Array.Copy(hpai1, 0, buffer, 6, hpai1.Length);
            Array.Copy(hpai2, 0, buffer, 6 + hpai1.Length, hpai2.Length);
            Array.Copy(cri, 0, buffer, 6 + hpai1.Length + hpai2.Length, cri.Length);

            return buffer;
        }
    }
}