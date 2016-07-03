using KNXNet.Placeholders;

namespace KNXNet.Packets.Core
{
    public class DescriptionResponse
    {
        private KNXNetIPHeader Header { get; set; }
        public KNXNetIPDIB DeviceHardware { get; set; }
        public KNXNetIPDIB SupportedServiceFamilies { get; set; }
        /// <summary>
        /// Optional
        /// </summary>
        public KNXNetIPDIB OtherDeviceInformation { get; set; }
    }
}