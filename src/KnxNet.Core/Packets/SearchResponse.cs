using KnxNet.Core.Placeholders;

namespace KnxNet.Core.Packets
{
	public class SearchResponse
	{
		private KnxNetIPHeader Header { get; set; }
		public KnxNetIPHPAI ControlEndpoint { get; set; }
		public KnxNetIPDIB DeviceHardware { get; set; }
		public KnxNetIPDIB SupportedServiceFamilies { get; set; }
	}
}