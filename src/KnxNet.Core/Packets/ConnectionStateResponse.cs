namespace KnxNet.Core.Packets
{
    public class ConnectionStateResponse
    {
        private KnxNetIPHeader Header { get; set; }
        public byte ChannelId { get; set; }
        public byte Status { get; set; }
    }
}