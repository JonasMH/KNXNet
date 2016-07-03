using System;
using KNXNet.Placeholders;

namespace KNXNet.Packets.Core
{
    public class DisconnectRequest
    {
        private KNXNetIPHeader Header { get; } = new KNXNetIPHeader() {HeaderSize = 0x06, Version = 0x10, ServiceType = 0x0209, Size = 0x10};
        public byte ChannelId { get; set; }
        public byte Reserved { get; set; }
        public KNXNetIPHPAI ControlEndpoint { get; set; }

        public byte[] GetBytes()
        {
            byte[] buffer = new byte[Header.Size];

            Array.Copy(Header.GetBytes(), 0, buffer, 0, 6);
            buffer[6] = ChannelId;
            buffer[7] = Reserved;
            Array.Copy(ControlEndpoint.GetBytes(), 0, buffer, 8, 8);

            return buffer;
        }
    }
}