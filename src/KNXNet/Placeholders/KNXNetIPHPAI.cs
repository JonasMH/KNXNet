using System;
using System.Net;

namespace KNXNet.Placeholders
{
    public class KNXNetIPHPAI
    {
        public enum ProtocolCodes
        {
            Ipv4Udp = 0x01,
            Ipv4Tcp = 0x02
        }

        public byte Lenght => 8;
        public ProtocolCodes ProtocolCode { get; set; }
        public IPAddress Address { get; set; }
        public short Port { get; set; }

        public byte[] GetBytes()
        {
            byte[] buffer = new byte[8];

            buffer[0] = Lenght;
            buffer[1] = (byte)ProtocolCode;

            Array.Copy(Address.GetAddressBytes(), 0, buffer, 2, 4);
            Array.Copy(KNXBitConverter.GetBytes(Port), 0, buffer, 6, 2);

            return buffer;
        }
    }
}
