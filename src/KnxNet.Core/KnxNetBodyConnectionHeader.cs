namespace KnxNet.Core
{
	public class KnxNetBodyConnectionHeader
	{
		public byte StructureLength { get; private set; } = 4;
		public byte ChannelId { get; set; }
		public byte SequenceCounter { get; set; }
		public byte Reserved { get; set; }
		public byte TunnelingAckStatus => Reserved;

		public static KnxNetBodyConnectionHeader Parse(byte[] buffer, int index)
		{
			KnxNetBodyConnectionHeader header = new KnxNetBodyConnectionHeader
			{
				StructureLength = buffer[index],
				ChannelId = buffer[index + 1],
				SequenceCounter = buffer[index + 2],
				Reserved = buffer[index + 3]
			};

			return header;
		}

		public byte[] GetBytes()
		{
			return new[] {StructureLength, ChannelId, SequenceCounter, Reserved};
		}
	}
}