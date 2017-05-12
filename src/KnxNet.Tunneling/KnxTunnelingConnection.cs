using System;
using System.Net;
using System.Net.Sockets;
using KnxNet.Core;
using KnxNet.Core.Packets;
using KnxNet.Core.Placeholders;
using System.Threading;
using System.Threading.Tasks;

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

		public bool IsConnected { get; private set; }

		public ILogger Logger { get; set; }

		public event EventHandler OnConnect;
		public event EventHandler<KnxDisconnectEventArg> OnDisconnect;
		public event EventHandler<KnxReceivedDataInEventArgs> OnData;

		private byte _sequenceNumber;
		private UdpClient _udpClient;

		public object SequenceNumberLock = new object();

		internal KnxTunnelingSender Sender;
		internal KnxTunnelingReceiver Receiver;
		internal DateTime LastReceivedHeartBeat;
		private Timer _timer;

		public KnxTunnelingConnection(string host, int port)
		{
			Host = host;
			Port = port;

			RemoteEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
			LocalEndPoint = new IPEndPoint(IPAddress.Any, 0);
		}


		private void SendConnectionStateRequest()
		{
			if (_udpClient == null || !IsConnected)
				return;

			try
			{
				ConnectionStateRequest request = new ConnectionStateRequest()
				{
					ChannelId = ChannelId,
					ControlEndpoint = new KnxNetIPHPAI(_udpClient.LocalIpEndPoint(), KnxNetIPHPAI.ProtocolCodes.Ipv4Udp)
				};

				Sender.SendPacket(request);
				DateTime sentHeartbeatTime = DateTime.Now;
				bool gotResponse = false;

				for (int i = 0; i < 3 && !gotResponse; i++)
				{
					while (true)
					{
						Task.Delay(250).Wait();
						if (!IsConnected)
						{
							return;
						}

						if (LastReceivedHeartBeat > sentHeartbeatTime)
						{
							gotResponse = true;
							break;
						}

						if (DateTime.Now - sentHeartbeatTime > TimeSpan.FromSeconds(10))
						{
							Logger?.WriteLine("Retrying heartbeat");
							Sender.SendPacket(request);
							break;
						}
					}
				}

				if (!gotResponse)
				{
					Disconnected(new KnxDisconnectEventArg
					{
						Reason = KnxDisconnectEventArg.DisconnectReason.ConnectionLost,
						WasClean = false
					});
					Logger?.WriteLine("No heartbeat response", LogType.Error);
				}
				else
				{
					Logger?.WriteLine("Heartbeart successful");
				}
			}
			catch (Exception e)
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

			Sender = new KnxTunnelingSender(this, _udpClient) { Logger = Logger };
			Receiver = new KnxTunnelingReceiver(this, _udpClient) { Logger = Logger };

			Receiver.Start();
			Sender.SendPacket(request);


		}

		internal void Connected(byte channelId)
		{
			_sequenceNumber = 0;
			ChannelId = channelId;
			IsConnected = true;

			Logger?.WriteLine("Ch. #: " + ChannelId);
			_timer = new Timer((e) => SendConnectionStateRequest(), null, 1000, 60000);

			OnConnect?.Invoke(this, EventArgs.Empty);
		}

		internal void Disconnected(KnxDisconnectEventArg args)
		{
			IsConnected = false;
			_timer.Dispose();
			OnDisconnect?.Invoke(this, args);
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

			Sender.SendPacket(request);
			_timer.Dispose();
		}

		public byte GetNextSequenceNumber()
		{
			return _sequenceNumber++;
		}

		/// <summary>
		/// Send a value to a given address
		/// </summary>
		/// <param name="address">The knx group address to send data to</param>
		/// <param name="data">The data</param>
		/// <param name="dataLength">The data-length in bits</param>
		public void SendValue(KnxGroupAddress address, byte[] data, int dataLength)
		{
			Sender.SendMessage(address, data, dataLength);
		}

		/// <summary>
		/// Request a value to be read from the bus. If there's a response it will be readable in the OnData event.
		/// </summary>
		/// <param name="address">The knx group address to read from</param>
		public void RequestValue(KnxGroupAddress address)
		{
			Sender.RequestValue(address);
		}

		public void Dispose()
		{

		}
	}
}
