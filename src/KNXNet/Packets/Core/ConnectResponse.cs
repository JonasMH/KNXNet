using KNXNet.Placeholders;

namespace KNXNet.Packets.Core
{
    public class ConnectResponse
    {
        private KNXNetIPHeader Header { get; set; }
        public byte ChannelId { get; set; }
        public byte Status { get; set; }
        public KNXNetIPHPAI DataEndpoint { get; set; }
        public KNXNetIPCRD ResponseDataBlock { get; set; }

        public static ConnectResponse Parse(byte[] buffer, int index)
        {
            int progress = 0;
            ConnectResponse output = new ConnectResponse();

            output.Header = KNXNetIPHeader.Parse(buffer, index + progress);
            progress += 6;

            output.ChannelId = buffer[index + progress];
            output.Status = buffer[index + progress];
            progress += 2;

            output.DataEndpoint = KNXNetIPHPAI.Parse(buffer, index + progress);
            progress += output.DataEndpoint.Lenght;

            output.ResponseDataBlock = KNXNetIPCRD.Parse(buffer, index + progress);

            return output;
        }
    }
}