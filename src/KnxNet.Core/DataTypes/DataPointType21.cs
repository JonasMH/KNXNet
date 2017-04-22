namespace KnxNet.Core.DataTypes
{
	public class DataPointType21
	{
		public bool this[int key]
		{
			get { return ((Attributes & (1 << key)) >> key) == 1; }
			set
			{
				if (value)
				{
					Attributes |= (byte) (1 << key);
				}
				else
				{
					Attributes &= (byte) ~(1 << key);
				}
			}
		}

		public byte Attributes { get; set; }
	}
}