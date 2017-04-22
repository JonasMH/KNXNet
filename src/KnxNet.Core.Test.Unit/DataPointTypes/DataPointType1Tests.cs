using KnxNet.Core.DataTypes;
using Xunit;

namespace KnxNet.Core.Test.Unit.DataPointTypes
{
	public class DataPointType1Tests
	{
		[Theory]
		[InlineData(new byte[] { 0x01 }, true)]
		[InlineData(new byte[] { 0x00 }, false)]
		public void DPT1_ShouldPack(byte[] expected, bool value)
		{
			var result = new DataPointType1 { B = value }.Pack();

			Assert.Equal(result.Data, expected);
		}


		[Theory]
		[InlineData(new byte[] { 0x81 }, true)]
		[InlineData(new byte[] { 0x80 }, false)]
		public void DPT1_ShouldParse(byte[] bytes, bool expected)
		{
			var result = DataPointType1.Parse(bytes).B;

			Assert.Equal(expected, result);

		}
	}
}
