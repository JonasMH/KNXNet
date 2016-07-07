using KnxNet.Core.Placeholders;

namespace KnxNet.Core.Packets
{
    public class ConnectionStateRequest
    {
        private KnxNetIPHeader Header { get; set; }
        public byte ChannelId { get; set; }
        public byte Reserved { get; set; }
        public KnxNetIPHPAI ControlEndpoint { get; set; }
    }
}