using KNXNet.Placeholders;

namespace KNXNet.Packets
{
    public class DescriptionRequest
    {
        public KNXNetIPHeader Header { get; set; }
        public KNXNetIPHPAI ControlEndport { get; set; }
    }
}