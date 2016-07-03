namespace KNXNet.Packets.Core
{
    public class ConnectionStateResponse
    {
        private KNXNetIPHeader Header { get; set; }
        public byte ChannelId { get; set; }
        public byte Status { get; set; }
    }
}