using KnxNet.Core;
using System;
using KnxNet.Core.Packets;

namespace KnxNet.Tunneling.Packets
{
	public class TunnelingAck : IKnxPacket
	{
		public KnxNetIPHeader Header { get; set; } = new KnxNetIPHeader() { HeaderSize = 0x06, Version = 0x10, ServiceType = 0x0421 };
		public KnxNetBodyConnectionHeader ConnectionHeader { get; set; } = new KnxNetBodyConnectionHeader();

		public byte[] GetBytes()
		{
			short totalSize = (short)(Header.HeaderSize + ConnectionHeader.StructureLength);
			byte[] buffer = new byte[totalSize];

			Header.Size = totalSize;

			Array.Copy(Header.GetBytes(), 0, buffer, 0, 6);
			Array.Copy(ConnectionHeader.GetBytes(), 0, buffer, Header.HeaderSize, ConnectionHeader.StructureLength);


			return buffer;
		}

		public static TunnelingAck Parse(byte[] buffer, int index)
		{
			TunnelingAck request = new TunnelingAck
			{
				Header = KnxNetIPHeader.Parse(buffer, index),
				ConnectionHeader = KnxNetBodyConnectionHeader.Parse(buffer, index + 6)
			};

			return request;
		}
	}
}
