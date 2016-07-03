using System;

namespace KNXNet
{
    public class CommonExternalMessageInterface
    {
        public enum CmeiMessageCode
        {
            LDataInd = 0x29
        }

        public CmeiMessageCode MessageCode => (CmeiMessageCode) MessageCodeRaw;

        public byte MessageCodeRaw { get; set; }
        public byte[] AdditionalInformation { get; set; } = new byte[0];
        public byte[] ServiceInformation { get; set; } = new byte[0];

        public static bool TryParse(byte[] input, int index, int length, out CommonExternalMessageInterface output)
        {
            output = new CommonExternalMessageInterface();

            if (input == null || input.Length < 2)
                return false;

            output.MessageCodeRaw = input[index + 0];
            int additionalInfoLength = input[index + 1];
            int infoLength = length - (2 + additionalInfoLength);

            output.AdditionalInformation = new byte[additionalInfoLength];
            output.ServiceInformation = new byte[infoLength];

            Array.Copy(input, index + 2, output.AdditionalInformation, 0, additionalInfoLength);
            Array.Copy(input, index + 2 + additionalInfoLength, output.ServiceInformation, 0, infoLength);

            return true;
        }

        public byte[] GetBytes()
        {
            byte[] buffer = new byte[2 + AdditionalInformation.Length + ServiceInformation.Length];

            buffer[0] = MessageCodeRaw;
            buffer[1] = (byte) AdditionalInformation.Length;

            if (AdditionalInformation.Length != 0)
            {
                Array.Copy(AdditionalInformation, 0, buffer, 2, AdditionalInformation.Length);
                Array.Copy(ServiceInformation, 0, buffer, 2 + AdditionalInformation.Length, ServiceInformation.Length);
            }
            else
            {
                Array.Copy(ServiceInformation, 0, buffer, 2, ServiceInformation.Length);
            }

            return buffer;
        }
    }
}
