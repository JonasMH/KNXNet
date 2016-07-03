using KNXNet.Placeholders;

namespace KNXNet.Packets
{
    public class SearchResponse
    {
        public KNXNetIPHeader Header { get; set; }
        public KNXNetIPHPAI ControlEndpoint { get; set; }
        public KNXNetIPDIB DeviceHardware { get; set; }
        public KNXNetIPDIB SupportedServiceFamilies { get; set; }

    }
}