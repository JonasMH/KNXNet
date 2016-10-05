using System.Collections.Generic;
using System.Net.Sockets;
using KnxNet.Core;
using KnxNet.Tunneling.Packets;
using System.Linq;
using KnxNet.Core.Packets;

namespace KnxNet.Tunneling
{
	public class KnxTunnelingSender
	{
		public ILogger Logger { private get; set; }
		public UdpClient UdpClient { private get; set; }
		private readonly KnxTunnelingConnection _connection;

		public KnxTunnelingSender(KnxTunnelingConnection connection, UdpClient client)
		{
			_connection = connection;
			UdpClient = client;
		}

		public void SendMessage(KnxGroupAddress destinationAddress, byte[] data, int lengthInBits)
		{
			byte len;

			if (lengthInBits <= 4)
				len = 1;
			else if (lengthInBits <= 8)
				len = 2;
			else if (lengthInBits <= 16)
				len = 3;
			else
				len = 4;

			IList<byte> buffer = new List<byte> {
				0xBC,
				0xE0,
				_connection.SourceAddress.Value[0],
				_connection.SourceAddress.Value[1],
				destinationAddress.Value[0],
				destinationAddress.Value[1],
						(byte)(len & 0x0F), 0x00};

			switch (len)
			{
				case 1:
					buffer.Add((byte)(0x80 | (data[0] & 0x0F)));
					break;
				case 2:
					buffer.Add(0x80);
					buffer.Add(data[0]);
					break;
				case 3:
					buffer.Add(0x80);
					buffer.Add(data[1]);
					buffer.Add(data[2]);
					break;
			}

			lock (_connection.SequenceNumberLock)
			{
				TunnelingRequest request = new TunnelingRequest()
				{
					ConnectionHeader = new KnxNetBodyConnectionHeader()
					{
						ChannelId = _connection.ChannelId,
						SequenceCounter = _connection.GetNextSequenceNumber()
					},
					Message = new CommonExternalMessageInterface()
					{
						MessageCode = CommonExternalMessageInterface.CmeiMessageCode.LDataReq,
						ServiceInformation = buffer.ToArray()
					}
				};

				UdpClient.SendAsync(request.GetBytes(), _connection.RemoteEndPoint);
			}

		}


		public void SendPacket(IKnxPacket packet)
		{
			Send(packet.GetBytes());
		}

		private void Send(byte[] buffer)
		{
			UdpClient.SendAsync(buffer, _connection.RemoteEndPoint).Wait();
		}
	}
}
