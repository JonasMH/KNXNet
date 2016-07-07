using KnxNet.Core.Placeholders;

namespace KnxNet.Core.Packets
{
    public class DescriptionResponse
    {
        private KnxNetIPHeader Header { get; set; }
        public KnxNetIPDIB DeviceHardware { get; set; }
        public KnxNetIPDIB SupportedServiceFamilies { get; set; }
        /// <summary>
        /// Optional
        /// </summary>
        public KnxNetIPDIB OtherDeviceInformation { get; set; }
    }
}