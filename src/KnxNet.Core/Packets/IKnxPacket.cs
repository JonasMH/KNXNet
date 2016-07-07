using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnxNet.Core.Packets
{
    public interface IKnxPacket
    {
        byte[] GetBytes();
    }
}
