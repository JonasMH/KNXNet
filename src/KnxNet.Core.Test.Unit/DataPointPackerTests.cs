using System;
using System.Collections.Generic;
using System.Text;
using KnxNet.Core.DataTypes;
using Xunit;

namespace KnxNet.Core.Test.Unit
{
    public class DataPointPackerTests
	{
		[Theory]
		[InlineData(new byte[] { 0x0C, 0xED }, 25.22)]
		[InlineData(new byte[] { 0x03, 0xE8 }, 10.00)]
		public void DPT9_ShouldPack(byte[] expected, float value)
		{
			var parser = new DataPointPacker();
			var result = parser.DPT9(new DataPointType9{FloatValue = value});

			Assert.Equal(result.Data, expected);
		}
	}
}
