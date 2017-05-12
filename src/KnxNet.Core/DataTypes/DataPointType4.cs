namespace KnxNet.Core.DataTypes
{
	public class DataPointType4
	{
		public byte Character { get; set; }

		public DataPointPackerResult Pack()
		{
			return new DataPointPackerResult
			{
				Data = new[] {Character},
				BitLength = 1
			};
		}

		public static DataPointType4 Parse(byte[] data)
		{
			DataPointType4 dpt = new DataPointType4
			{
				Character = data[0]
			};


			return dpt;
		}
	}
}