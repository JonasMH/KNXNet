using System;

namespace Core
{
    public class CommonExternalMessageInterface
    {
        public enum CmeiMessageCode
        {
            LDataInd = 0x29
        }

        public CmeiMessageCode MessageCode => (CmeiMessageCode) MessageCodeRaw;

        public byte MessageCodeRaw { get; set; }
        public byte[] AdditionalInformation { get; set; }
        public byte[] ServiceInformation { get; set; }

        public static bool TryParse(byte[] input, out CommonExternalMessageInterface output)
        {
            output = new CommonExternalMessageInterface();

            if (input == null || input.Length < 2)
                return false;

            output.MessageCodeRaw = input[0];
            int additionalInfoLength = input[1];
            int infoLength = input.Length - (2 + additionalInfoLength);

            output.AdditionalInformation = new byte[additionalInfoLength];
            output.ServiceInformation = new byte[infoLength];

            Array.Copy(input, 2, output.AdditionalInformation, 0, additionalInfoLength);
            Array.Copy(input, 2 + additionalInfoLength, output.ServiceInformation, 0, infoLength);

            return true;
        }
    }
}
