using KnxNet.Core.Placeholders;

namespace KnxNet.Core.Packets
{
	public class DescriptionRequest
	{
		private KnxNetIPHeader Header { get; set; }
		public KnxNetIPHPAI ControlEndport { get; set; }
	}
}