using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using KNXNet.Packets;
using KNXNet.Placeholders;

namespace KNXNet
{
    public class Search
    {
        public static IPAddress GetLocalAddress()
        {
            return IPAddress.Parse("192.168.1.119");
            //IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            //return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public void SearchNetwork()
        {
            string host = "224.0.23.12";
            int timeToLive = 2;
            short port = 3671;
            int localPort = 3671;
            int waitTimeInSeconds = 2;

            IPAddress localIpAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(localIpAddress, localPort);
            IPEndPoint multicastEndPoint = new IPEndPoint(IPAddress.Parse(host), port);


            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            socket.Bind(localEndPoint);

            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(multicastEndPoint.Address, localIpAddress));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, timeToLive);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);

            SearchRequest request = new SearchRequest
            {
                DiscoveryEndpoint =
                {
                    Address = GetLocalAddress(),
                    Port = (short) localEndPoint.Port,
                    ProtocolCode = KNXNetIPHPAI.ProtocolCodes.Ipv4Udp
                }
            };

            byte[] buffer = request.GetBytes();


            socket.SendTo(buffer, SocketFlags.None, multicastEndPoint);
            buffer = new byte[10000];

            Stopwatch watch = Stopwatch.StartNew();

            while (watch.ElapsedMilliseconds < waitTimeInSeconds*2000)
            {
                if (socket.Available <= 0)
                    continue;

                int received = socket.Receive(buffer);
                Console.WriteLine(received);
            }

            socket.Shutdown(SocketShutdown.Both);
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
