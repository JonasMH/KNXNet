using System;
using System.Net;
using System.Net.Sockets;
using KnxNet.Core;
using KnxNet.Core.Packets;
using KnxNet.Core.Placeholders;
using System.Threading;

namespace KnxNet.Tunneling
{
	public class KnxTunnelingConnection : IKnxConnection
	{
		public byte ChannelId { get; private set; }
		public string Host { get; }
		public int Port { get; }

		public IPEndPoint LocalEndPoint { get; set; }
		public IPEndPoint RemoteEndPoint { get; }
		public KnxAddress SourceAddress { get; set; } = new KnxAddress(1, 0, 150);

		public bool IsConnected { get; private set; } = false;

		public ILogger Logger { get; set; }

		public event EventHandler OnConnect;
		public event EventHandler OnDisconnect;
		public event EventHandler<KnxReceivedDataInEventArgs> OnData;

		private byte _sequenceNumber;
		private UdpClient _udpClient;

		public object SequenceNumberLock = new object();

		internal KnxTunnelingSender _sender;
		internal KnxTunnelingReceiver _receiver;
		private Timer _timer;

		public KnxTunnelingConnection(string host, int port)
		{
			_timer = new Timer((e) => SendConnectionStateRequest(), null, 1000, 60000);

			Host = host;
			Port = port;

			RemoteEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
			LocalEndPoint = new IPEndPoint(IPAddress.Any, 0);
		}


		private void SendConnectionStateRequest()
		{
			if (_udpClient == null && IsConnected)
				return;

			try
			{
				ConnectionStateRequest request = new ConnectionStateRequest()
				{
					ChannelId = ChannelId,
					ControlEndpoint = new KnxNetIPHPAI(_udpClient.LocalIpEndPoint(), KnxNetIPHPAI.ProtocolCodes.Ipv4Udp)
				};

				_sender.SendPacket(request);
				Logger?.WriteLine("Sent heartbeat");
			}
			catch(Exception e)
			{
				Logger?.WriteLine(e.Message, LogType.Error);
			}

		}

		public void Connect()
		{
			_udpClient = new UdpClient(LocalEndPoint);

			ConnectRequest request = new ConnectRequest()
			{
				DataEndpoint = new KnxNetIPHPAI(_udpClient.LocalIpEndPoint(), KnxNetIPHPAI.ProtocolCodes.Ipv4Udp),
				ControlEndpoint = new KnxNetIPHPAI(_udpClient.LocalIpEndPoint(), KnxNetIPHPAI.ProtocolCodes.Ipv4Udp),
				ConnectionRequest =
					new KnxNetIPCRI()
					{
						ConnectionType = KnxNetIPCRI.ConnectionTypes.Tunnel,
						IndependantData = new byte[] { 0x02, 0x00 }
					}
			};

			_sender = new KnxTunnelingSender(this, _udpClient) { Logger = Logger };
			_receiver = new KnxTunnelingReceiver(this, _udpClient) { Logger = Logger };

			_receiver.Start();

			_sender.SendPacket(request);
		}

		internal void Connected(byte channelId)
		{
			_sequenceNumber = 0;
			ChannelId = channelId;
			Logger?.WriteLine("Ch. #: " + ChannelId);
			IsConnected = true;
			OnConnect?.Invoke(this, EventArgs.Empty);
		}

		internal void Disconnected()
		{
			IsConnected = false;
			OnDisconnect?.Invoke(this, EventArgs.Empty);
		}

		internal void NewData(KnxReceivedDataInEventArgs data)
		{
			OnData?.Invoke(this, data);
		}

		public void Disconnect()
		{
			DisconnectRequest request = new DisconnectRequest()
			{
				ChannelId = ChannelId,
				ControlEndpoint = new KnxNetIPHPAI(_udpClient.LocalIpEndPoint(), KnxNetIPHPAI.ProtocolCodes.Ipv4Udp)
			};

			_sender.SendPacket(request);
		}

		public byte GetNextSequenceNumber()
		{
			return _sequenceNumber++;
		}

		public void SendValue(KnxGroupAddress address, byte[] data, int dataLength)
		{
			_sender.SendMessage(address, data, dataLength);
		}

		public void RequestValue(KnxGroupAddress address)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{

		}
	}
}
