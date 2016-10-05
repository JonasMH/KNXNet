using KnxNet.Core;
using NUnit.Framework;

namespace KNXNet.Core.Test.Unit
{
	[TestFixture]
	public class KnxAddressTests
	{
		[TestCase(new byte[] { 0x00, 0x00 }, 00)]
		[TestCase(new byte[] { 0xF0, 0x00 }, 15)]
		[TestCase(new byte[] { 0x0F, 0xFF }, 00)]
		[TestCase(new byte[] { 0xFF, 0xFF }, 15)]
		[TestCase(new byte[] { 0x90, 0x00 }, 9)]
		public void SetValue_InputValue_ExpectedAreaSet(byte[] input, int expected)
		{
			KnxAddress addr = new KnxAddress() { Value = input };
			Assert.That(addr.Area, Is.EqualTo(expected));
		}

		[TestCase(new byte[] { 0x00, 0x00 }, 00)]
		[TestCase(new byte[] { 0x0F, 0x00 }, 15)]
		[TestCase(new byte[] { 0xF0, 0xFF }, 00)]
		[TestCase(new byte[] { 0xFF, 0xFF }, 15)]
		[TestCase(new byte[] { 0x09, 0x00 }, 9)]
		public void SetValue_InputValue_ExpectedLineSet(byte[] input, int expected)
		{
			KnxAddress addr = new KnxAddress() { Value = input };
			Assert.That(addr.Line, Is.EqualTo(expected));
		}

		[TestCase(new byte[] { 0x00, 0x00 }, 0)]
		[TestCase(new byte[] { 0x00, 0xFF }, 255)]
		[TestCase(new byte[] { 0xFF, 0x00 }, 0)]
		[TestCase(new byte[] { 0xFF, 0xFF }, 255)]
		[TestCase(new byte[] { 0x00, 0x10 }, 16)]
		public void SetValue_InputValue_ExpectedBusDeviceSet(byte[] input, int expected)
		{
			KnxAddress addr = new KnxAddress() { Value = input };
			Assert.That(addr.BusDevice, Is.EqualTo(expected));
		}
	}
}
