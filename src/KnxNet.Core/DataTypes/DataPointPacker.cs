using System;
using System.Collections.Generic;
using System.Text;

namespace KnxNet.Core.DataTypes
{
	public class DataPointPacker
	{
		public DataPointPackerResult DPT1(DataPointType1 dpt)
		{
			byte byteValue = dpt.B ? (byte)1 : (byte)2;

			return new DataPointPackerResult
			{
				Data = new byte[] {byteValue},
				BitLength = 1
			};
		}
		
		public DataPointPackerResult DPT4(DataPointType4 dpt)
		{
			return new DataPointPackerResult
			{
				Data = new byte[] { dpt.Character },
				BitLength = 1
			};
		}

		public DataPointPackerResult DPT9(DataPointType9 dpt)
		{
			var result = new DataPointPackerResult
			{
				Data = new byte[2],
				BitLength = 16
			};

			//Borrowed from the other KNX.net lib because lazyness
			float v = (float)Math.Round(dpt.FloatValue * 100);
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
	}

	public class DataPointPackerResult
	{
		public byte[] Data { get; set; }
		public int BitLength { get; set; }
	}
}
