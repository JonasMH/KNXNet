using System;
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
					HandleConnectResponse(buffer);
					break;
				case ServiceType.TunnelingRequest:
					HandleTunnelingRequest(buffer);
					break;
				case ServiceType.TunnelingAcknowledge:
					HandleTunnelingAcknowledge(buffer);
					break;
				case ServiceType.ConnectionStateResponse:
					HandleConnectionStateResponse(buffer);
					break;
				case ServiceType.DisconnectRequest:
					_connection.Disconnected(new KnxDisconnectEventArg
					{
						Reason = KnxDisconnectEventArg.DisconnectReason.EndpointRequest,
						WasClean = true
					});
					//TODO Should respond to request
					break;
				case ServiceType.DisconnectResponse:
					_connection.Disconnected(new KnxDisconnectEventArg
					{
						Reason = KnxDisconnectEventArg.DisconnectReason.LocalRequest,
						WasClean = true
					});
					break;
				default:
					Logger?.WriteLine("Unknown packet with service type: " + (ServiceType)header.ServiceType, LogType.Warning);
					break;
			}
		}

		private void HandleConnectionStateResponse(byte[] buffer)
		{
			ConnectionStateResponse response = ConnectionStateResponse.Parse(buffer, 0);

			if (response.StatusCode != ConnectionStateResponse.StatusCodes.NoError)
			{
				_connection.Disconnected(new KnxDisconnectEventArg
				{
					Reason = KnxDisconnectEventArg.DisconnectReason.ConnectionStateStatusCode,
					WasClean = false
				});
			}

			_connection.LastReceivedHeartBeat = DateTime.Now;
		}

		private void HandleTunnelingAcknowledge(byte[] buffer)
		{
			TunnelingAck ack = TunnelingAck.Parse(buffer, 0);

			if (ack.ConnectionHeader.TunnelingAckStatus != 0)
			{
				_connection.Disconnected(new KnxDisconnectEventArg
				{
					Reason = KnxDisconnectEventArg.DisconnectReason.TunnelingAckStatusCode,
					WasClean = false
				});
			}
		}

		private void HandleConnectResponse(byte[] buffer)
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

		private void HandleTunnelingRequest(byte[] buffer)
		{
			TunnelingRequest request = TunnelingRequest.Parse(buffer, 0);
			TunnelingAck ack = new TunnelingAck { ConnectionHeader = request.ConnectionHeader };

			_connection.Sender.SendPacket(ack);

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
