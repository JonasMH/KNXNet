using KnxNet.Core;
using NUnit.Framework;

namespace KNXNet.Core.Test.Unit
{
	[TestFixture]
	public class KnxGroupAddressTests
	{
		[TestCase(new byte[] { 0x00, 0x00 }, 00)]
		[TestCase(new byte[] { 0xF8, 0x00 }, 31)]
		[TestCase(new byte[] { 0x07, 0xFF }, 00)]
		[TestCase(new byte[] { 0xFF, 0xFF }, 31)]
		[TestCase(new byte[] { 0x90, 0x00 }, 18)]
		public void SetValue_InputValue_ExpectedMainGroupSet(byte[] input, int expected)
		{
			KnxGroupAddress addr = new KnxGroupAddress() { Value = input };
			Assert.That(addr.MainGroup, Is.EqualTo(expected));
		}

		[TestCase(new byte[] { 0x00, 0x00 }, 00)]
		[TestCase(new byte[] { 0x07, 0x00 }, 7)]
		[TestCase(new byte[] { 0xF8, 0xFF }, 00)]
		[TestCase(new byte[] { 0xFF, 0xFF }, 7)]
		[TestCase(new byte[] { 0x05, 0x00 }, 5)]
		public void SetValue_InputValue_ExpectedMiddleGroupSet(byte[] input, int expected)
		{
			KnxGroupAddress addr = new KnxGroupAddress() { Value = input };
			Assert.That(addr.MiddleGroup, Is.EqualTo(expected));
		}

		[TestCase(new byte[] { 0x00, 0x00 }, 0)]
		[TestCase(new byte[] { 0x00, 0xFF }, 255)]
		[TestCase(new byte[] { 0xFF, 0x00 }, 0)]
		[TestCase(new byte[] { 0xFF, 0xFF }, 255)]
		[TestCase(new byte[] { 0x00, 0x10 }, 16)]
		public void SetValue_InputValue_ExpectedSubGroupSet(byte[] input, int expected)
		{
			KnxGroupAddress addr = new KnxGroupAddress() { Value = input };
			Assert.That(addr.SubGroup, Is.EqualTo(expected));
		}

		[TestCase("0/0/0", 0)]
		[TestCase("31/0/0", 31)]
		[TestCase("31/7/255", 31)]
		[TestCase("0/7/255", 0)]
		public void Parse_InputValue_CorrectMainGroup(string input, int expected)
		{
			KnxGroupAddress addr = KnxGroupAddress.Parse(input);
			Assert.That(addr.MainGroup, Is.EqualTo(expected));
		}

		[TestCase("0/0/0", 0)]
		[TestCase("0/7/0", 7)]
		[TestCase("31/7/255", 7)]
		[TestCase("31/0/255", 0)]
		public void Parse_InputValue_CorrectMiddleGroup(string input, int expected)
		{
			KnxGroupAddress addr = KnxGroupAddress.Parse(input);
			Assert.That(addr.MiddleGroup, Is.EqualTo(expected));
		}

		[TestCase("0/0/0", 0)]
		[TestCase("0/0/255", 255)]
		[TestCase("31/7/255", 255)]
		[TestCase("31/7/0", 0)]
		public void Parse_InputValue_CorrectSubGroup(string input, int expected)
		{
			KnxGroupAddress addr = KnxGroupAddress.Parse(input);
			Assert.That(addr.SubGroup, Is.EqualTo(expected));
		}
	}
}
