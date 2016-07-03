using KNXNet.Placeholders;

namespace KNXNet.Packets
{
    public class DescriptionResponse
    {
        public KNXNetIPHeader Header { get; set; }
        public KNXNetIPDIB DeviceHardware { get; set; }
        public KNXNetIPDIB SupportedServiceFamilies { get; set; }
        /// <summary>
        /// Optional
        /// </summary>
        public KNXNetIPDIB OtherDeviceInformation { get; set; }
    }
}