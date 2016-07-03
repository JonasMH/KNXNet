using NUnit.Framework;
using KNXNet;

namespace KNXNet.Test.Unit
{
    [TestFixture]
    public class KNXAddressTests
    {
        [Test]
        public void Area_ValueZero_Zero()
        {
            KNXAddress addr = new KNXAddress {Value = new byte[] {0x00, 0x00}};
            Assert.That(addr.Area, Is.EqualTo(0));
        }

        [Test]
        public void Line_ValueZero_Zero()
        {
            KNXAddress addr = new KNXAddress {Value = new byte[] {0x00, 0x00}};
            Assert.That(addr.Line, Is.EqualTo(0));
        }

        [Test]
        public void BusDevice_ValueZero_Zero()
        {
            KNXAddress addr = new KNXAddress {Value = new byte[] {0x00, 0x00}};
            Assert.That(addr.BusDevice, Is.EqualTo(0));
        }

        [Test]
        public void Area_ValueMax_MaxAddress()
        {
            KNXAddress addr = new KNXAddress {Value = new byte[] {0xFF, 0xFF}};
            Assert.That(addr.Area, Is.EqualTo(15));
        }

        [Test]
        public void Line_ValueMax_MaxAddress()
        {
            KNXAddress addr = new KNXAddress { Value = new byte[] { 0xFF, 0xFF } };
            Assert.That(addr.Line, Is.EqualTo(15));
        }

        [Test]
        public void BusDevice_ValueMax_MaxAddress()
        {
            KNXAddress addr = new KNXAddress { Value = new byte[] { 0xFF, 0xFF } };
            Assert.That(addr.BusDevice, Is.EqualTo(255));
        }
    }
}
