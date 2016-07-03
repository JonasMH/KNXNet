using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KNXNet.Packets.Tunnelling
{
    public class TunnelingAck
    {
        public KNXNetIPHeader Header { get; set; } = new KNXNetIPHeader() {HeaderSize = 0x06, Version = 0x10, ServiceType = 0x0421};
        public KNXNetBodyConnectionHeader ConnectionHeader { get; set; } = new KNXNetBodyConnectionHeader();

        public byte[] GetBytes()
        {
            short totalSize = (short)(Header.HeaderSize + ConnectionHeader.StructureLength);
            byte[] buffer = new byte[totalSize];

            Header.Size = totalSize;

            Array.Copy(Header.GetBytes(), 0, buffer, 0, 6);
            Array.Copy(ConnectionHeader.GetBytes(), 0, buffer, Header.HeaderSize, ConnectionHeader.StructureLength);


            return buffer;
        }

        public static TunnelingAck Parse(byte[] buffer, int index)
        {
            TunnelingAck request = new TunnelingAck
            {
                Header = KNXNetIPHeader.Parse(buffer, index),
                ConnectionHeader = KNXNetBodyConnectionHeader.Parse(buffer, index + 6)
            };
            
            return request;
        }
    }
}
