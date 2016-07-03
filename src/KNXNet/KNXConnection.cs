﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using KNXNet.Packets.Core;
using KNXNet.Packets.Tunnelling;
using KNXNet.Placeholders;

namespace KNXNet
{
    public class KnxReceivedDataInEventArgs : EventArgs
    {
        public KNXAddress SourceAddress { get; set; }
        public KNXGroupAddress DestinationAddress { get; set; }
        public byte[] Data { get; set; }
    }

    public class KnxConnection : IDisposable
    {
        private Socket _socket;
        private byte _sequenceNumber = 0;

        public byte ChannelId { get; set; }

        public string Host { get; set; }
        public short Port { get; set; }
        public KNXAddress SourceAddress { get; set; } = new KNXAddress(1, 0, 150);

        public void Start()
        {
            InitiateConnection();
            Listen();
        }

        public void StartAsync()
        {
            Task.Factory.StartNew(() =>
            {
                InitiateConnection();
                Listen();
            });
        }

        public void SendMessage(KNXGroupAddress destinationAddress, byte[] data, int lengthInBits)
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
                SourceAddress.Value[0],
                SourceAddress.Value[1],
                destinationAddress.Value[0],
                destinationAddress.Value[1],
                        (byte)(len & 0x0F), 0x00};

            switch (len)
            {
                case 1:
                    buffer.Add((byte) (0x80 | (data[0] & 0x0F)));
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

            TunnelingRequest request = new TunnelingRequest()
            {
                ConnectionHeader = new KNXNetBodyConnectionHeader()
                {
                    ChannelId = ChannelId,
                    SequenceCounter = _sequenceNumber++
                },
                Message = new CommonExternalMessageInterface()
                {
                    MessageCode = CommonExternalMessageInterface.CmeiMessageCode.LDataReq,
                    ServiceInformation = buffer.ToArray()
                }
            };
            
            byte[] buffer1 = request.GetBytes();

            _socket.Send(buffer1);
        }

        private void InitiateConnection()
        {
            if (_socket != null)
                throw new Exception("Allready initate");

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Connect(Host, Port);

            IPEndPoint localEndPoint = (IPEndPoint)_socket.LocalEndPoint;

            ConnectRequest request = new ConnectRequest()
            {
                DataEndpoint = new KNXNetIPHPAI() { Address = localEndPoint.Address, Port = (short)localEndPoint.Port, ProtocolCode = KNXNetIPHPAI.ProtocolCodes.Ipv4Udp },
                ControlEndpoint = new KNXNetIPHPAI() { Address = localEndPoint.Address, Port = (short)localEndPoint.Port, ProtocolCode = KNXNetIPHPAI.ProtocolCodes.Ipv4Udp },
                ConnectionRequest = new KNXNetIPCRI() { ConnectionType = KNXNetIPCRI.ConnectionTypes.Tunnel, IndependantData = new byte[] { 0x02, 0x00 } }
            };

            byte[] buffer = request.GetBytes();

            _socket.Send(buffer);
        }

        private void Listen()
        {
            while (_socket != null)
            {
                if(_socket.Available > 0)
                    ReceiveIncommingPacket();
                
                Task.Delay(1);
            }
        }

        private void ReceiveIncommingPacket()
        {
            byte[] buffer = new byte[1000];
            _socket.Receive(buffer);
            KNXNetIPHeader header = KNXNetIPHeader.Parse(buffer, 0);

            switch (header.ServiceType)
            {
                case 0x0206:
                    ConnectResponse response = ConnectResponse.Parse(buffer, 0);
                    ChannelId = response.ChannelId;
                    Console.WriteLine(ChannelId);
                    break;
                case 0x0420:
                    TunnelingRequest request = TunnelingRequest.Parse(buffer, 0);
                    TunnelingAck ack = new TunnelingAck { ConnectionHeader = request.ConnectionHeader };
                    _socket.Send(ack.GetBytes());

                    byte[] serviceInfo = request.Message.ServiceInformation;

                    KNXAddress sourceAddress = new KNXAddress() { Value = new[] { serviceInfo[2], serviceInfo[3] } };
                    KNXGroupAddress destAddress = new KNXGroupAddress() { Value = new[] { serviceInfo[4], serviceInfo[5] } };

                    int dataLength = serviceInfo[6] & 0x0F;
                    byte[] data;

                    switch (dataLength)
                    {
                        case 1:  // 4 bit max
                            data = new[] {(byte) (serviceInfo[8] & 0x0F)};
                            break;
                        case 2: // 8 bit
                            data = new[] { serviceInfo[9]};
                            break;
                        case 3: // 2 bytes
                            data = new[] { serviceInfo[9], serviceInfo[10]};
                            break;
                        default:
                            data = new byte[1];
                            break;
                    }

                    OnNewDataIn?.Invoke(this, new KnxReceivedDataInEventArgs() { DestinationAddress = destAddress, SourceAddress = sourceAddress, Data = data });

                    break;
                case 0x0421:
                    TunnelingAck ack2 = TunnelingAck.Parse(buffer, 0);//TODO ERROR CHECKING
                    _socket.Receive(buffer);
                    TunnelingRequest request1 = TunnelingRequest.Parse(buffer, 0);

                    TunnelingAck ack3 = new TunnelingAck
                    {
                        ConnectionHeader = request1.ConnectionHeader
                    };

                    ack3.ConnectionHeader.SequenceCounter &= 0x01;

                    byte[] tmep = ack3.GetBytes();
                    _socket.Send(tmep);
                    break;
                default:
                    Console.WriteLine("Unknown packet with service type: " + header.ServiceType.ToString("X"));
                    break;
            }
        }

        public event EventHandler<KnxReceivedDataInEventArgs> OnNewDataIn;

        public void Disconnect()
        {
            IPEndPoint localEndPoint = (IPEndPoint)_socket.LocalEndPoint;

            DisconnectRequest request = new DisconnectRequest()
            {
                ChannelId = ChannelId,
                ControlEndpoint = new KNXNetIPHPAI() { Address = localEndPoint.Address, Port = (short)localEndPoint.Port, ProtocolCode = KNXNetIPHPAI.ProtocolCodes.Ipv4Udp }
            };

            _socket.Send(request.GetBytes());
        }

        public void Dispose()
        {
            
        }
    }
}