using KnxNet.Core;

namespace KnxNet.Tunneling
{

	public static class TunnellingRequestCEMIExtensions
	{

		public static APCIType GetAPCIType(this CommonExternalMessageInterface cemi)
		{
			int value = ((cemi.ServiceInformation[7] & 0x03) << 2) +
						((cemi.ServiceInformation[8] & 0xC0) >> 6);

			return (APCIType)value;
		}

		public static void SetAPCIType(this CommonExternalMessageInterface cemi)
		{
			
		}


		public static KnxAddress GetSourceAddress(this CommonExternalMessageInterface cemi)
		{
			return new KnxAddress() { Value = new[] { cemi.ServiceInformation[2], cemi.ServiceInformation[3] } };
		}

		public static KnxGroupAddress GetDestinationAddress(this CommonExternalMessageInterface cemi)
		{
			return new KnxGroupAddress() { Value = new[] { cemi.ServiceInformation[4], cemi.ServiceInformation[5] } };
		}

		public static byte[] GetData(this CommonExternalMessageInterface cemi)
		{
			int dataLength = cemi.ServiceInformation[6] & 0x0F;
			byte[] data;

			switch (dataLength)
			{
				case 1:  // 4 bit max
					data = new[] { (byte)(cemi.ServiceInformation[8] & 0x0F) };
					break;
				case 2: // 8 bit
					data = new[] { cemi.ServiceInformation[9] };
					break;
				case 3: // 2 bytes
					data = new[] { cemi.ServiceInformation[9], cemi.ServiceInformation[10] };
					break;
				default:
					data = new byte[1];
					break;
			}

			return data;
		}
	}
}
