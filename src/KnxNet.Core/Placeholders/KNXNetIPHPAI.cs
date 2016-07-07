using System;
using System.Net;

namespace KnxNet.Core.Placeholders
{
    public class KnxNetIPHPAI
    {
        public enum ProtocolCodes
        {
            Ipv4Udp = 0x01,
            Ipv4Tcp = 0x02
        }

        public byte Lenght {get; set;} = 8;
        public ProtocolCodes ProtocolCode { get; set; }
        public IPAddress Address { get; set; }
        public short Port { get; set; }

        public byte[] GetBytes()
        {
            byte[] buffer = new byte[8];

            buffer[0] = Lenght;
            buffer[1] = (byte)ProtocolCode;

            Array.Copy(Address.GetAddressBytes(), 0, buffer, 2, 4);
            Array.Copy(KnxBitConverter.GetBytes(Port), 0, buffer, 6, 2);

            return buffer;
        }

        public static KnxNetIPHPAI Parse(byte[] buffer, int index)
        {
            byte[] temp = new byte[4];
            Array.Copy(buffer, index + 2, temp, 0, 4);

            KnxNetIPHPAI output = new KnxNetIPHPAI
            {
                Lenght = buffer[index],
                ProtocolCode = (ProtocolCodes) buffer[index + 1],
                Address = new IPAddress(temp),
                Port = KnxBitConverter.ToShort(buffer, index + 6)
            };

            return output;
        }
    }
}
