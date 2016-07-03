using KNXNet.Placeholders;

namespace KNXNet.Packets.Core
{
    public class DescriptionRequest
    {
        private KNXNetIPHeader Header { get; set; }
        public KNXNetIPHPAI ControlEndport { get; set; }
    }
}