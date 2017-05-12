using System;

namespace KnxNet.Core
{
	public enum ServiceType
	{
		SearchRequest = 0x0201,
		SearchResponse = 0x0202,
		DescriptionRequest = 0x0203,
		DescriptionResponse = 0x0204,
		ConnectRequest = 0x0205,
		ConnectResponse = 0x0206,
		ConnectionStateRequest = 0x0207,
		ConnectionStateResponse = 0x0208,
		DisconnectRequest = 0x0209,
		DisconnectResponse = 0x020A,

		DeviceConfigurationRequest = 0x0310,
		DeviceConfigurationAcknowledge = 0x0311,

		TunnelingRequest = 0x0420,
		TunnelingAcknowledge = 0x0421,

		RoutingIndication = 0x0530,
		RoutingLostMessage = 0x0531
	}

	public class KnxNetIPHeader
	{
		public byte HeaderSize { get; set; }
		public byte Version { get; set; }
		public short ServiceType { get; set; }
		public short Size { get; set; }

		public byte[] GetBytes()
		{
			byte[] buffer = new byte[6];

			buffer[0] = HeaderSize;
			buffer[1] = Version;

			Array.Copy(KnxBitConverter.GetBytes(ServiceType), 0, buffer, 2, 2);
			Array.Copy(KnxBitConverter.GetBytes(Size), 0, buffer, 4, 2);

			return buffer;
		}

		public static KnxNetIPHeader Parse(byte[] buffer, int index)
		{
			return new KnxNetIPHeader
			{
				HeaderSize = buffer[index],
				Version = buffer[index + 1],
				ServiceType = KnxBitConverter.ToShort(buffer, index + 2),
				Size = KnxBitConverter.ToShort(buffer, index + 4)
			};
		}
	}
}