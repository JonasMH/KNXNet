using KnxNet.Core.Placeholders;

namespace KnxNet.Core.Packets
{
	public class ConnectResponse
	{
		private KnxNetIPHeader Header { get; set; }
		public byte ChannelId { get; set; }
		public byte Status { get; set; }
		public KnxNetIPHPAI DataEndpoint { get; set; }
		public KnxNetIPCRD ResponseDataBlock { get; set; }

		public static ConnectResponse Parse(byte[] buffer, int index)
		{
			int progress = 0;
			ConnectResponse output = new ConnectResponse
			{
				Header = KnxNetIPHeader.Parse(buffer, index + progress)
			};

			progress += 6;

			output.ChannelId = buffer[index + progress];
			progress++;
			output.Status = buffer[index + progress];
			progress++;

			if(output.Status == 0)
			{
				output.DataEndpoint = KnxNetIPHPAI.Parse(buffer, index + progress);
				progress += output.DataEndpoint.Lenght;

				output.ResponseDataBlock = KnxNetIPCRD.Parse(buffer, index + progress);
			}

			return output;
		}
	}
}