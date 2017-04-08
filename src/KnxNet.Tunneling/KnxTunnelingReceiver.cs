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
				case ServiceType.ConnectionStateResponse:

					break;
				case ServiceType.DisconnectRequest:
					_connection.Disconnected();
					break;
				case ServiceType.DisconnectResponse:
					_connection.Disconnected();
					break;
				default:
					Logger?.WriteLine("Unknown packet with service type: " + (ServiceType)header.ServiceType, LogType.Warning);
					break;
			}
		}

		private void HandleConnectResponse(byte[] buffer, KnxNetIPHeader header)
		{
			ConnectResponse response = ConnectResponse.Parse(buffer, 0);

			if (response.Status == 0)
			{
				_connection.Connected(response.ChannelId);
			}
			else
			{
				Logger?.WriteLine("Connection failed with status:" + response.Status.ToString("X"), LogType.Error);
			}
		}

		private void HandleTunnelingRequest(byte[] buffer, KnxNetIPHeader header)
		{
			TunnelingRequest request = TunnelingRequest.Parse(buffer, 0);
			TunnelingAck ack = new TunnelingAck { ConnectionHeader = request.ConnectionHeader };

			_connection._sender.SendPacket(ack);

			_receivedPackets.Add(new KnxReceivedDataInEventArgs()
			{
				DestinationAddress = request.Message.GetDestinationAddress(),
				SourceAddress = request.Message.GetSourceAddress(),
				Data = request.Message.GetData(),
				RequestType = request.Message.GetAPCIType()
			});
		}
	}
}
