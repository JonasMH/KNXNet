using KnxNet.Tunneling;
using System;
using System.Net;

namespace KnxNet.Core
{
	public class KnxReceivedDataInEventArgs : EventArgs
	{
		public KnxAddress SourceAddress { get; set; }
		public KnxGroupAddress DestinationAddress { get; set; }
		public byte[] Data { get; set; }
		public APCIType RequestType { get; set; }
	}


	public class KnxDisconnectEventArg : EventArgs
	{
		public enum DisconnectReason
		{
			Unknown,
			EndpointRequest,
			LocalRequest,
			ConnectionLost,
			ConnectionStateStatusCode,
			TunnelingAckStatusCode
		}

		public bool WasClean { get; set; }
		public DisconnectReason Reason { get; set; }
	}


	public interface IKnxConnection
	{
		event EventHandler OnConnect;
		event EventHandler<KnxDisconnectEventArg> OnDisconnect;
		event EventHandler<KnxReceivedDataInEventArgs> OnData;

		string Host { get; }
		int Port { get; }
		
		IPEndPoint LocalEndPoint { get; }
		IPEndPoint RemoteEndPoint { get; }

		void Connect();
		void Disconnect();

		void SendValue(KnxGroupAddress address, byte[] data, int dataLength);
		void RequestValue(KnxGroupAddress address);

		ILogger Logger { set; }
	}
}