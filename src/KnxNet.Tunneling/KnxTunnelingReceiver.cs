using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Tasks;
using KnxNet.Core;
using KnxNet.Core.Packets;
using KnxNet.Tunneling.Packets;

namespace KnxNet.Tunneling
{
	public class KnxTunnelingReceiver
	{
		public ILogger Logger { private get; set; }
		public UdpClient UdpClient { private get; set; }
		private readonly KnxTunnelingConnection _connection;

		private readonly BlockingCollection<KnxReceivedDataInEventArgs> _receivedPackets = new BlockingCollection<KnxReceivedDataInEventArgs>();

		public KnxTunnelingReceiver(KnxTunnelingConnection connection, UdpClient client)
		{
			_connection = connection;
			UdpClient = client;
		}

		public void Start()
		{
			Task.Factory.StartNew(EventTask);
			Task.Factory.StartNew(ListenTask);
		}

		private void EventTask()
		{
			while (true)
			{
				_connection.NewData(_receivedPackets.Take());
			}
			// ReSharper disable once FunctionNeverReturns
		}

		private void ListenTask()
		{
			while (true)
			{
				byte[] buffer = UdpClient.ReceiveAsync().Result.Buffer;
				KnxNetIPHeader header = KnxNetIPHeader.Parse(buffer, 0);
				ProcessIncomming(buffer, header);
			}
			// ReSharper disable once FunctionNeverReturns
		}

		private void ProcessIncomming(byte[] buffer, KnxNetIPHeader header)
		{
			switch ((ServiceType)header.ServiceType)
			{
				case ServiceType.ConnectResponse:
					HandleConnectResponse(buffer, header);
					break;
				case ServiceType.TunnelingRequest:
					HandleTunnelingRequest(buffer, header);
					break;
				case ServiceType.TunnelingAcknowledge:
					/*TunnelingAck ack2 = TunnelingAck.Parse(buffer, 0);//TODO ERROR CHECKING
					_socket.Receive(buffer);
					TunnelingRequest request1 = TunnelingRequest.Parse(buffer, 0);

					TunnelingAck ack3 = new TunnelingAck
					{
						ConnectionHeader = request1.ConnectionHeader
					};

					ack3.ConnectionHeader.SequenceCounter &= 0x01;

					byte[] tmep = ack3.GetBytes();
					_socket.Send(tmep);*/
					break;
				default:
					Logger?.WriteLine("Unknown packet with service type: " + header.ServiceType.ToString("X"), LogType.Warn);
					break;
			}
		}

		private void HandleConnectResponse(byte[] buffer, KnxNetIPHeader header)
		{
			ConnectResponse response = ConnectResponse.Parse(buffer, 0);
			_connection.Connected(response.ChannelId);
		}

		private void HandleTunnelingRequest(byte[] buffer, KnxNetIPHeader header)
		{
			TunnelingRequest request = TunnelingRequest.Parse(buffer, 0);
			TunnelingAck ack = new TunnelingAck { ConnectionHeader = request.ConnectionHeader };

			_connection._sender.SendPacket(ack);

			byte[] serviceInfo = request.Message.ServiceInformation;

			KnxAddress sourceAddress = new KnxAddress() { Value = new[] { serviceInfo[2], serviceInfo[3] } };
			KnxGroupAddress destAddress = new KnxGroupAddress() { Value = new[] { serviceInfo[4], serviceInfo[5] } };

			int dataLength = serviceInfo[6] & 0x0F;
			byte[] data;

			switch (dataLength)
			{
				case 1:  // 4 bit max
					data = new[] { (byte)(serviceInfo[8] & 0x0F) };
					break;
				case 2: // 8 bit
					data = new[] { serviceInfo[9] };
					break;
				case 3: // 2 bytes
					data = new[] { serviceInfo[9], serviceInfo[10] };
					break;
				default:
					data = new byte[1];
					break;
			}

			_receivedPackets.Add(new KnxReceivedDataInEventArgs() { DestinationAddress = destAddress, SourceAddress = sourceAddress, Data = data });
		}
	}
}
