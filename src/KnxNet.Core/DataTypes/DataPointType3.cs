namespace KnxNet.Core.DataTypes
{
	public class DataPointType3
	{
		public bool C { get; set; }
		public byte StepCode { get; set; }


		public static DataPointType3 Parse(byte[] data)
		{
			DataPointType3 dpt = new DataPointType3
			{
				C = (data[0] & 0x08) == 1,
				StepCode = (byte)(data[0] & 0x07)
			};


			return dpt;
		}
	}
}