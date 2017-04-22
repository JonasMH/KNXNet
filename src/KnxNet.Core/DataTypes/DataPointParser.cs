namespace KnxNet.Core.DataTypes
{
	public class DataPointParser
	{
		public DataPointType1 DPT1(byte[] data) => DataPointType1.Parse(data);
		public DataPointType2 DPT2(byte[] data) => DataPointType2.Parse(data);
		public DataPointType3 DPT3(byte[] data) => DataPointType3.Parse(data);
		public DataPointType5 DPT5(byte[] data) => DataPointType5.Parse(data);
		public DataPointType9 DPT9(byte[] data) => DataPointType9.Parse(data);
	}
}
