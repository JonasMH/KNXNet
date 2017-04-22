namespace KnxNet.Core.DataTypes
{
	public class DataPointType11
	{
		/// <summary>
		/// 1..31
		/// </summary>
		public byte Day { get; set; }

		/// <summary>
		/// 1..12
		/// </summary>
		public byte Month { get; set; }

		/// <summary>
		/// 0..99
		/// </summary>
		public byte Year { get; set; }
	}
}