using KnxNet.Core;
using KnxNet.Core.DataTypes;

namespace KnxNet.Tunneling
{
	public static class KnxTunnelingConnectionExtensions
	{

		public static void SendDPT1(this KnxTunnelingConnection con, KnxGroupAddress address, bool value)
		{
			DataPointPackerResult result = new DataPointType1 { B = value }.Pack();
			con.SendValue(address, result.Data, result.BitLength);
		}
		public static void SendDPT9(this KnxTunnelingConnection con, KnxGroupAddress address, float value)
		{
			DataPointPackerResult result = new DataPointType9 { FloatValue = value }.Pack();
			con.SendValue(address, result.Data, result.BitLength);
		}
	}
}
