using KNXNet.Placeholders;

namespace KNXNet.Packets.Core
{
    public class ConnectionStateRequest
    {
        private KNXNetIPHeader Header { get; set; }
        public byte ChannelId { get; set; }
        public byte Reserved { get; set; }
        public KNXNetIPHPAI ControlEndpoint { get; set; }
    }
}