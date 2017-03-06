using KnxNet.Core;
using Xunit;

namespace KNXNet.Core.Test.Unit
{
	public class KnxGroupAddressTests
	{
		[Theory]
		[InlineData(new byte[] { 0x00, 0x00 }, 00)]
		[InlineData(new byte[] { 0xF8, 0x00 }, 31)]
		[InlineData(new byte[] { 0x07, 0xFF }, 00)]
		[InlineData(new byte[] { 0xFF, 0xFF }, 31)]
		[InlineData(new byte[] { 0x90, 0x00 }, 18)]
		public void SetValue_InputValue_ExpectedMainGroupSet(byte[] input, int expected)
		{
			KnxGroupAddress addr = new KnxGroupAddress() { Value = input };
			Assert.Equal(expected, addr.MainGroup);
		}

		[Theory]
		[InlineData(new byte[] { 0x00, 0x00 }, 00)]
		[InlineData(new byte[] { 0x07, 0x00 }, 7)]
		[InlineData(new byte[] { 0xF8, 0xFF }, 00)]
		[InlineData(new byte[] { 0xFF, 0xFF }, 7)]
		[InlineData(new byte[] { 0x05, 0x00 }, 5)]
		public void SetValue_InputValue_ExpectedMiddleGroupSet(byte[] input, int expected)
		{
			KnxGroupAddress addr = new KnxGroupAddress() { Value = input };
			Assert.Equal(expected, addr.MiddleGroup);
		}

		[Theory]
		[InlineData(new byte[] { 0x00, 0x00 }, 0)]
		[InlineData(new byte[] { 0x00, 0xFF }, 255)]
		[InlineData(new byte[] { 0xFF, 0x00 }, 0)]
		[InlineData(new byte[] { 0xFF, 0xFF }, 255)]
		[InlineData(new byte[] { 0x00, 0x10 }, 16)]
		public void SetValue_InputValue_ExpectedSubGroupSet(byte[] input, int expected)
		{
			KnxGroupAddress addr = new KnxGroupAddress() { Value = input };
			Assert.Equal(expected, addr.SubGroup);
		}

		[Theory]
		[InlineData("0/0/0", 0)]
		[InlineData("31/0/0", 31)]
		[InlineData("31/7/255", 31)]
		[InlineData("0/7/255", 0)]
		public void Parse_InputValue_CorrectMainGroup(string input, int expected)
		{
			KnxGroupAddress addr = KnxGroupAddress.Parse(input);
			Assert.Equal(expected, addr.MainGroup);
		}

		[Theory]
		[InlineData("0/0/0", 0)]
		[InlineData("0/7/0", 7)]
		[InlineData("31/7/255", 7)]
		[InlineData("31/0/255", 0)]
		public void Parse_InputValue_CorrectMiddleGroup(string input, int expected)
		{
			KnxGroupAddress addr = KnxGroupAddress.Parse(input);
			Assert.Equal(expected, addr.MiddleGroup);
		}

		[Theory]
		[InlineData("0/0/0", 0)]
		[InlineData("0/0/255", 255)]
		[InlineData("31/7/255", 255)]
		[InlineData("31/7/0", 0)]
		public void Parse_InputValue_CorrectSubGroup(string input, int expected)
		{
			KnxGroupAddress addr = KnxGroupAddress.Parse(input);
			Assert.Equal(expected, addr.SubGroup);
		}
	}
}
