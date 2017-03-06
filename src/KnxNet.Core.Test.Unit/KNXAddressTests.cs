using KnxNet.Core;
using Xunit;

namespace KNXNet.Core.Test.Unit
{
	public class KnxAddressTests
	{
		[Theory]
		[InlineData(new byte[] { 0x00, 0x00 }, 00)]
		[InlineData(new byte[] { 0xF0, 0x00 }, 15)]
		[InlineData(new byte[] { 0x0F, 0xFF }, 00)]
		[InlineData(new byte[] { 0xFF, 0xFF }, 15)]
		[InlineData(new byte[] { 0x90, 0x00 }, 9)]
		public void SetValue_InputValue_ExpectedAreaSet(byte[] input, int expected)
		{
			KnxAddress addr = new KnxAddress() { Value = input };
			Assert.Equal(expected, addr.Area);
		}

		[Theory]
		[InlineData(new byte[] { 0x00, 0x00 }, 00)]
		[InlineData(new byte[] { 0x0F, 0x00 }, 15)]
		[InlineData(new byte[] { 0xF0, 0xFF }, 00)]
		[InlineData(new byte[] { 0xFF, 0xFF }, 15)]
		[InlineData(new byte[] { 0x09, 0x00 }, 9)]
		public void SetValue_InputValue_ExpectedLineSet(byte[] input, int expected)
		{
			KnxAddress addr = new KnxAddress() { Value = input };
			Assert.Equal(expected, addr.Line);
		}

		[Theory]
		[InlineData(new byte[] { 0x00, 0x00 }, 0)]
		[InlineData(new byte[] { 0x00, 0xFF }, 255)]
		[InlineData(new byte[] { 0xFF, 0x00 }, 0)]
		[InlineData(new byte[] { 0xFF, 0xFF }, 255)]
		[InlineData(new byte[] { 0x00, 0x10 }, 16)]
		public void SetValue_InputValue_ExpectedBusDeviceSet(byte[] input, int expected)
		{
			KnxAddress addr = new KnxAddress() { Value = input };
			Assert.Equal(expected, addr.BusDevice);
		}
	}
}
