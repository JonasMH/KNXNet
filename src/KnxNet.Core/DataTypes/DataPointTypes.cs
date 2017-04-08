namespace KnxNet.Core.DataTypes
{
	public class DataPointType1
	{
		public bool B { get; set; }
	}

	public class DataPointType2
	{
		public bool C { get; set; }
		public bool V { get; set; }
	}

	public class DataPointType3
	{
		public bool C { get; set; }
		public byte StepCode { get; set; }
	}

	public class DataPointType4
	{
		public byte Character { get; set; }
	}

	public class DataPointType5
	{
		public byte UnsignedValue { get; set; }
	}

	public class DataPointType9
	{
		public float FloatValue { get; set; }
	}
}
