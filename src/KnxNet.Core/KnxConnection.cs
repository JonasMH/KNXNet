using System;
using System.Net;

namespace KnxNet.Core
{
	public class KnxReceivedDataInEventArgs : EventArgs
	{
		public KnxAddress SourceAddress { get; set; }
		public KnxGroupAddress DestinationAddress { get; set; }
		public byte[] Data { get; set; }
	}

	public interface IKnxConnection
	{
		event EventHandler OnConnect;
		event EventHandler OnDisconnect;
		event EventHandler<KnxReceivedDataInEventArgs> OnData;

		string Host { get; }
		int Port { get; }

		IPAddress Address { get; }
		IPEndPoint RemoteEndPoint { get; }

		void Connect();
		void Disconnect();

		void SendValue(KnxGroupAddress address, byte[] data, int dataLength);
		void RequestValue(KnxGroupAddress address);

		ILogger Logger { set; }
	}
}