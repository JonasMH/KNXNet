using System;

namespace KnxNet.Core.DataTypes
{
	public class DataPointType9
	{
		public float FloatValue { get; set; }


		public DataPointPackerResult Pack()
		{
			DataPointPackerResult result = new DataPointPackerResult
			{
				Data = new byte[2],
				BitLength = 16
			};

			//Borrowed from the other KNX.net lib because lazyness
			float v = (float)Math.Round(FloatValue * 100);
			int e = 0;
			while (v < -2048)
			{
				v = v / 2;
				e++;
			}
			while (v > 2047)
			{
				v = v / 2;
				e++;
			}

			int mantissa;
			bool signed;
			if (v < 0)
			{
				signed = true;
				mantissa = ((int)v * -1);
				mantissa = ~mantissa;
				mantissa = mantissa + 1;
			}
			else
			{
				signed = false;
				mantissa = (int)v;
			}

			if (signed)
				result.Data[0] = 0x80;

			result.Data[0] = ((byte)(result.Data[0] | ((e & 0x0F) << 3)));
			result.Data[0] = ((byte)(result.Data[0] | ((mantissa >> 8) & 0x07)));
			result.Data[1] = ((byte)mantissa);

			return result;
		}



		/// <summary>
		/// Parses knx-data with a data-point type of 9.* (Some float-value)
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static DataPointType9 Parse(byte[] data)
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
}