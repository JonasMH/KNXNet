using System;

namespace KnxNet.Core.Placeholders
{
	public class KnxNetIPCRI
	{
		public enum ConnectionTypes
		{
			DeviceManagement = 0x03,
			Tunnel = 0x04,
			RemoteLogging = 0x06,
			RemoteConfiguration = 0x06,
			ObjectServer = 0x08
		}

		public ConnectionTypes ConnectionType { get; set; }
		public byte[] IndependantData { get; set; } = new byte[0];
		public byte[] DependantData { get; set; } = new byte[0];

		public byte[] GetBytes()
		{
			byte[] buffer = new byte[2 + IndependantData.Length + DependantData.Length];

			buffer[0] = (byte)buffer.Length;
			buffer[1] = (byte)ConnectionType;

			Array.Copy(IndependantData, 0, buffer, 2, IndependantData.Length);
			Array.Copy(DependantData, 0, buffer, 2 + IndependantData.Length, DependantData.Length);

			return buffer;
		}
	}
}
