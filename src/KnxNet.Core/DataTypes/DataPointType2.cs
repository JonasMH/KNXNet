namespace KnxNet.Core.DataTypes
{
	public class DataPointType2
	{
		public bool C { get; set; }
		public bool V { get; set; }


		public static DataPointType2 Parse(byte[] data)
		{
			DataPointType2 dpt = new DataPointType2
			{
				C = (data[0] & 0x02) == 1,
				V = (data[0] & 0x01) == 1
			};


			return dpt;
		}
	}
}