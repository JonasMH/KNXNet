using System;

namespace KnxNet.Core
{
    public class KnxNetIPHeader
    {
        public byte HeaderSize { get; set; }
        public byte Version { get; set; }
        public short ServiceType { get; set; }
        public short Size { get; set; }

        public byte[] GetBytes()
        {
            byte[] buffer = new byte[6];

            buffer[0] = HeaderSize;
            buffer[1] = Version;
            
            Array.Copy(KnxBitConverter.GetBytes(ServiceType), 0, buffer, 2, 2);
            Array.Copy(KnxBitConverter.GetBytes(Size), 0, buffer, 4, 2);

            return buffer;
        }

        public static KnxNetIPHeader Parse(byte[] buffer, int index)
        {
            return new KnxNetIPHeader
            {
                HeaderSize = buffer[index],
                Version = buffer[index + 1],
                ServiceType = KnxBitConverter.ToShort(buffer, index + 2),
                Size = KnxBitConverter.ToShort(buffer, index + 4)
            };
        }
    }
}
