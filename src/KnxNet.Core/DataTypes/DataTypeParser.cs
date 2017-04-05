using System;

namespace KnxNet.Core.DataTypes
{
	public class DataTypeParser
	{
		public DataPointType1 DPT1(byte[] data)
		{
			DataPointType1 dpt = new DataPointType1();

			dpt.B = (data[0] & 0xFE) == 1;

			return dpt;
		}

		public DataPointType2 DTP2(byte[] data)
		{
			DataPointType2 dpt = new DataPointType2();

			dpt.C = (data[0] & 0x02) == 1;
			dpt.V = (data[0] & 0x01) == 1;

			return dpt;
		}

		public DataPointType3 DTP3(byte[] data)
		{
			DataPointType3 dpt = new DataPointType3();

			dpt.C = (data[0] & 0x08) == 1;
			dpt.StepCode = (byte)(data[0] & 0x07);

			return dpt;
		}

		public DataPointType4 DTP4(byte[] data)
		{
			DataPointType4 dpt = new DataPointType4();

			dpt.Character = (char)data[0];

			return dpt;
		}

		public DataPointType5 DTP5(byte[] data)
		{
			return new DataPointType5 { UnsignedValue = data[0] };
		}

		/// <summary>
		/// Parses knx-data with a data-point type of 9.* (Some float-value)
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public DataPointType9 DTP9(byte[] data)
		{
			int e = (data[0] & 0x78) >> 3;

			int m1 = data[1];
			int m2 = data[0] & 0x07;
			uint m = (uint)((m2 << 8) | m1);
			bool signed = (data[0] & 0x80) >> 7 == 1;

			float value = (int)m;

			if (signed)
			{
				m = m - 1;
				m = ~(m);
				m = m & (0 | 0x07FF);
				value = (int)(m * -1);
			}


			return new DataPointType9 { FloatValue = 0.01f * value * (float)Math.Pow(2, e) };
		}
	}

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
		public char Character { get; set; }
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
