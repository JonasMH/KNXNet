using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using KnxNet.Core;
using KnxNet.Core.Packets;
using KnxNet.Core.Placeholders;

namespace KnxNet.Tunneling
{
    public class KnxTunnelingConnection : IKnxConnection
    {
        public byte ChannelId { get; private set; }
        public string Host { get; }
        public int Port { get; }

        public IPAddress Address { get; }
        public IPEndPoint RemoteEndPoint { get; }
        public KnxAddress SourceAddress { get; set; } = new KnxAddress(1, 0, 150);

        public ILogger Logger { get; set; }
        
        public event EventHandler OnConnect;
        public event EventHandler OnDisconnect;
        public event EventHandler<KnxReceivedDataInEventArgs> OnData;

        public TimeSpan ConnectionStateRequestInterval { get; set; } = TimeSpan.FromSeconds(60);
        private Timer _connectionStateRequestTimer;

        private byte _sequenceNumber = 0;
        private UdpClient _udpClient;

        public object SequenceNumberLock = new object();

        internal KnxTunnelingSender _sender;
        internal KnxTunnelingReceiver _receiver;


        public KnxTunnelingConnection(string host, int port)
        {
            _connectionStateRequestTimer = new Timer(state => SendConnectionStateRequest(), null, TimeSpan.Zero,
                ConnectionStateRequestInterval);

            Host = host;
            Port = port;
            RemoteEndPoint = new IPEndPoint(Dns.GetHostAddressesAsync(Host).Result.FirstOrDefault(), port);

            Address = IPAddress.Any;
        }


        private void SendConnectionStateRequest()
        {
            if(_udpClient == null)
                return;

            ConnectionStateRequest request = new ConnectionStateRequest()
            {
                ChannelId = ChannelId,
                ControlEndpoint = new KnxNetIPHPAI(_udpClient.LocalIpEndPoint(), KnxNetIPHPAI.ProtocolCodes.Ipv4Udp)
            };

            _sender.SendPacket(request);
        }

        public void Connect()
        {
            _udpClient = new UdpClient(0);

            ConnectRequest request = new ConnectRequest()
            {
                DataEndpoint = new KnxNetIPHPAI(_udpClient.LocalIpEndPoint(), KnxNetIPHPAI.ProtocolCodes.Ipv4Udp),
                ControlEndpoint = new KnxNetIPHPAI(_udpClient.LocalIpEndPoint(), KnxNetIPHPAI.ProtocolCodes.Ipv4Udp),
                ConnectionRequest =
                    new KnxNetIPCRI()
                    {
                        ConnectionType = KnxNetIPCRI.ConnectionTypes.Tunnel,
                        IndependantData = new byte[] {0x02, 0x00}
                    }
            };

            _sender = new KnxTunnelingSender(this, _udpClient) {Logger = Logger};
            _receiver = new KnxTunnelingReceiver(this, _udpClient) {Logger = Logger};

            _receiver.Start();

            _sender.SendPacket(request);
        }

        internal void Connected(byte channelId)
        {
            _sequenceNumber = 0;
            ChannelId = channelId;
            Logger?.WriteLine("Ch. #: " + ChannelId);
            OnConnect?.Invoke(this, EventArgs.Empty);
        }

        internal void Disconnected()
        {
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
