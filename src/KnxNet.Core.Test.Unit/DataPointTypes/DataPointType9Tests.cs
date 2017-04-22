using KnxNet.Core.DataTypes;
using Xunit;

namespace KnxNet.Core.Test.Unit.DataPointTypes
{
    public class DataPointType9Tests
	{
		[Theory]
		[InlineData(new byte[] { 0x0C, 0xED }, 25.22)]
		[InlineData(new byte[] { 0x03, 0xE8 }, 10.00)]
		public void DPT9_ShouldPack(byte[] expected, float value)
		{
			var result = new DataPointType9 {FloatValue = value}.Pack();

			Assert.Equal(result.Data, expected);
		}


		[Theory]
		[InlineData(new byte[] { 0x0C, 0xED }, 25.22)]
		[InlineData(new byte[] { 0x03, 0xE8 }, 10.00)]
		public void DPT9_ShouldParse(byte[] bytes, float expected)
		{
			var result = DataPointType9.Parse(bytes).FloatValue;

			Assert.Equal(expected, result);

		}
	}
}
