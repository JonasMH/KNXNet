namespace KnxNet.Core.DataTypes
{
	public class DataPointType19
	{
		public byte Year { get; set; }
		public byte Month { get; set; }
		public byte DayOfMonth { get; set; }
		public byte HourOfDay { get; set; }
		public byte Minutes { get; set; }
		public byte Seconds { get; set; }
		public bool Fault { get; set; }
		public bool WorkingDay { get; set; }
		public bool NoWorkingDay { get; set; }
		public bool NoYear { get; set; }
		public bool NoDate { get; set; }
		public bool NoDayOfWeek { get; set; }
		public bool NoTime { get; set; }
		public bool StandardSummerTime { get; set; }
		public bool QualityOfClock { get; set; }
	}
}