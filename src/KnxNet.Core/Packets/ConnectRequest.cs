using System;
using KnxNet.Core.Placeholders;

namespace KnxNet.Core.Packets
{
	public class ConnectRequest : IKnxPacket
	{
		private KnxNetIPHeader Header { get; set; } = new KnxNetIPHeader() { HeaderSize = 0x06, Version = 0x10, ServiceType = 0x0205 };

		public KnxNetIPHPAI ControlEndpoint { get; set; }
		public KnxNetIPHPAI DataEndpoint { get; set; }
		public KnxNetIPCRI ConnectionRequest { get; set; }


		public byte[] GetBytes()
		{
			byte[] hpai1 = ControlEndpoint.GetBytes();
			byte[] hpai2 = DataEndpoint.GetBytes();
			byte[] cri = ConnectionRequest.GetBytes();

			Header.Size = (short)(6 + hpai1.Length + hpai2.Length + cri.Length);

			byte[] buffer = new byte[Header.Size];

			Array.Copy(Header.GetBytes(), 0, buffer, 0, 6);
			Array.Copy(hpai1, 0, buffer, 6, hpai1.Length);
			Array.Copy(hpai2, 0, buffer, 6 + hpai1.Length, hpai2.Length);
			Array.Copy(cri, 0, buffer, 6 + hpai1.Length + hpai2.Length, cri.Length);

			return buffer;
		}
	}
}