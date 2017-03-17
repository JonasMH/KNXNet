
using KnxNet.Core.DataTypes;
using Xunit;

namespace KnxNet.Core.Test.Unit
{
	public class DataTypeParserTests
	{
		[Theory]
		[InlineData(new byte[] { 0x0C, 0xED }, 25.22)]
		[InlineData(new byte[] { 0x03, 0xE8 }, 10.00)]
		public void DPT9_ShouldParse(byte[] bytes, float expected)
		{
			var parser = new DataTypeParser();
			var result = parser.DTP9(bytes).FloatValue;

			Assert.Equal(expected, result);

		}
	}
}
