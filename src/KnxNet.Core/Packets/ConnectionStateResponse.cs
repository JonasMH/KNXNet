namespace KnxNet.Core.Packets
{
	public class ConnectionStateResponse
	{
		public enum StatusCodes
		{
			NoError = 0x00,
			ErrorConnectionID = 0x21,
			ErrorDataConnection = 0x26,
			ErrorKnxConnection = 0x27
		}

		private KnxNetIPHeader Header { get; set; }
		public byte ChannelId { get; set; }
		public byte Status { get; set; }
		public StatusCodes StatusCode => (StatusCodes) Status;


		public static ConnectionStateResponse Parse(byte[] buffer, int index)
		{
			int progress = 0;
			ConnectionStateResponse output = new ConnectionStateResponse
			{
				Header = KnxNetIPHeader.Parse(buffer, index + progress)
			};

			progress += 6;

			output.ChannelId = buffer[index + progress];
			progress++;
			output.Status = buffer[index + progress];

			return output;
		}
	}
}