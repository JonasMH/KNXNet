namespace KnxNet.Core.Placeholders
{
	/// <summary>
	/// Description Information Block
	/// </summary>
	public class KnxNetIPDIB
	{
		public byte Lenght { get; set; }
		public byte TypeCode { get; set; }
		public byte[] BlockData { get; set; }
	}
}
