namespace KnxNet.Core.Placeholders
{
	/// <summary>
	/// KNXnet/IP COnnection Response Data Block
	/// </summary>
	public class KnxNetIPCRD
	{
		/// <summary>
		/// Structure lenght
		/// </summary>
		public byte Lenght => (byte)(2 + IndependantData.Length + DependantData.Length);
		/// <summary>
		/// Connection type code
		/// </summary>
		public byte ConnectionTypeCode { get; set; }
		/// <summary>
		/// Optional
		/// </summary>
		public byte[] IndependantData { get; set; }
		/// <summary>
		/// Optional
		/// </summary>
		public byte[] DependantData { get; set; }

		public static KnxNetIPCRD Parse(byte[] buffer, int index)
		{
			return new KnxNetIPCRD();
		}
	}
}
