namespace KnxNet.Core.DataTypes
{
	public class DataPointType1
	{
		public bool B { get; set; }


		public DataPointPackerResult Pack()
		{
			byte byteValue = B ? (byte)1 : (byte)0;

			return new DataPointPackerResult
			{
				Data = new [] { byteValue },
				BitLength = 1
			};
		}

		public static DataPointType1 Parse(byte[] data)
		{
			DataPointType1 dpt = new DataPointType1
			{
				B = (data[0] & 0x01) == 1
			};


			return dpt;
		}
	}
}