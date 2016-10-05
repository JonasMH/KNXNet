using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace KnxNet.Tunneling
{
	public static class UdpClientExtentions
	{
		public static Task<int> SendAsync(this UdpClient client, byte[] buffer, IPEndPoint endpoint)
		{
			return client.SendAsync(buffer, buffer.Length, endpoint);
		}

		public static IPEndPoint LocalIpEndPoint(this UdpClient client)
		{
			return (IPEndPoint)client.Client.LocalEndPoint;
		}
	}
}
