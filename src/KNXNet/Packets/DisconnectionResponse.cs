namespace KNXNet.Packets
{
    public class DisconnectionResponse
    {
        public KNXNetIPHeader Header { get; set; }
        public byte ChannelId { get; set; }
        public byte Status { get; set; }
    }
}