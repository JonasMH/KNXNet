using KNXNet.Placeholders;

namespace KNXNet.Packets.Core
{
    public class SearchResponse
    {
        private KNXNetIPHeader Header { get; set; }
        public KNXNetIPHPAI ControlEndpoint { get; set; }
        public KNXNetIPDIB DeviceHardware { get; set; }
        public KNXNetIPDIB SupportedServiceFamilies { get; set; }

    }
}