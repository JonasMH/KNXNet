namespace KnxNet.Core.DataTypes
{
	public class DataPointType20
	{
		public bool this[int key]
		{
			get { return ((Field1 & (1 << key)) >> key) == 1; }
			set
			{
				if (value)
				{
					Field1 |= (byte)(1 << key);
				}
				else
				{
					Field1 &= (byte)~(1 << key);
				}
			}
		}

		public byte Field1 { get; set; }
	}
}