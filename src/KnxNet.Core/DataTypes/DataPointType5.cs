namespace KnxNet.Core.DataTypes
{
	public class DataPointType5
	{
		public byte UnsignedValue { get; set; }

		public static DataPointType5 Parse(byte[] data)
		{
			return new DataPointType5
			{
				UnsignedValue = data[0]
			};
		}
	}
}