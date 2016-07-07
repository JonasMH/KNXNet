using System;
using KnxNet.Core.Placeholders;

namespace KnxNet.Core.Packets
{
    public class ConnectionStateRequest : IKnxPacket
    {
        private KnxNetIPHeader Header { get; } = new KnxNetIPHeader() {HeaderSize = 0x06, ServiceType = (short)ServiceType.ConnectionStateRequest, Version = 0x10};
        public byte ChannelId { get; set; }
        public byte Reserved { get; set; }
        public KnxNetIPHPAI ControlEndpoint { get; set; }

        public byte[] GetBytes()
        {
            byte[] hpai = ControlEndpoint.GetBytes();
            byte[] buffer = new byte[8 + hpai.Length];

            Header.Size = (short)(8 + hpai.Length);

            Array.Copy(Header.GetBytes(), 0, buffer, 0, 6);

            buffer[6] = ChannelId;
            buffer[7] = Reserved;

            Array.Copy(hpai, 0, buffer, 8, hpai.Length);

            return buffer;
        }
    }
}