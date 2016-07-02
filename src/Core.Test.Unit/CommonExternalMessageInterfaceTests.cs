using NUnit.Framework;

namespace Core.Test.Unit
{
    [TestFixture]
    public class CommonExternalMessageInterfaceTests
    {
        [Test]
        public void TryParse_NullInput_ReturnsFalse()
        {
            CommonExternalMessageInterface msg;
            byte[] input = null;

            bool result = CommonExternalMessageInterface.TryParse(input, out msg);
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryParse_EmptyInput_ReturnsFalse()
        {
            CommonExternalMessageInterface msg;
            byte[] input = new byte[0];

            bool result = CommonExternalMessageInterface.TryParse(input, out msg);
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryParse_SingleByte_ReturnsFalse()
        {
            CommonExternalMessageInterface msg;
            byte[] input = new byte[] {0x00};

            bool result = CommonExternalMessageInterface.TryParse(input, out msg);
            Assert.That(result, Is.False);
        }
    }
}
