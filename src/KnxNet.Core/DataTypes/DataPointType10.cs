namespace KnxNet.Core.DataTypes
{
	public class DataPointType10
	{
		public enum DayEnum
		{
			NoDay = 0,
			Monday = 1,
			Tuesday = 2,
			Wednesday = 3,
			Thursday = 4,
			Friday = 5,
			Saturday = 6,
			Sunday = 7
		}

		/// <summary>
		/// 0..7
		/// </summary>
		public DayEnum Day { get; set; }

		/// <summary>
		/// 0..23
		/// </summary>
		public byte Hour { get; set; }

		/// <summary>
		/// 0..59
		/// </summary>
		public byte Minutes { get; set; }

		/// <summary>
		/// 0..59
		/// </summary>
		public byte Seconds { get; set; }
	}
}