using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KNXNet
{
    public class KNXNetBodyConnectionHeader
    {
        public byte StructureLength { get; private set; } = 4;
        public byte ChannelId { get; set; }
        public byte SequenceCounter { get; set; }
        public byte Reserved { get; set; }

        public static KNXNetBodyConnectionHeader Parse(byte[] buffer, int index)
        {
            KNXNetBodyConnectionHeader header = new KNXNetBodyConnectionHeader
            {
                StructureLength = buffer[index],
                ChannelId = buffer[index + 1],
                SequenceCounter = buffer[index + 2],
                Reserved = buffer[index + 3]
            };
            
            return header;
        }

        public byte[] GetBytes()
        {
            return new byte[] {StructureLength, ChannelId, SequenceCounter, Reserved};
        }
    }
}
