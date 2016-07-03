using KNXNet.Placeholders;

namespace KNXNet.Packets
{
    public class ConnectResponse
    {
        public KNXNetIPHeader Header { get; set; }
        public byte ChannelId { get; set; }
        public byte Status { get; set; }
        public KNXNetIPHPAI DataEndpoint { get; set; }
        public KNXNetIPCRD ResponseDataBlock { get; set; }
    }
}