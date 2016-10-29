namespace KnxNet.Core
{
	public class KnxGroupAddressDescription
	{
		public KnxGroupAddress Address { get; set; }
		public string Name { get; set; }
		public string DataType { get; set; }
		public string MainGroup { get; set; }
		public string MiddleGroup { get; set; }

		public override string ToString()
		{
			return Name + " (" + Address + ")";
		}
	}
}