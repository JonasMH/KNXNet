using KnxNet.Core;
using KnxNet.Core.DataTypes;

namespace KnxNet.Tunneling
{
	public static class KnxTunnelingConnectionExtensions
	{
		public static DataPointPacker Packer { get; set; } = new DataPointPacker();

		public static void SendDPT1(this KnxTunnelingConnection con, KnxGroupAddress address, bool value)
		{
			DataPointPackerResult result = Packer.DPT1(new DataPointType1 {B = value});
			con.SendValue(address, result.Data, result.BitLength);
		}
		public static void SendDPT9(this KnxTunnelingConnection con, KnxGroupAddress address, float value)
		{
			DataPointPackerResult result = Packer.DPT9(new DataPointType9 { FloatValue = value });
			con.SendValue(address, result.Data, result.BitLength);
		}
	}
}
