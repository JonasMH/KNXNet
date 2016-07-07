using System;
using KnxNet.Core;
using KnxNet.Core.Packets;

namespace KnxNet.Tunneling.Packets
{
    public class TunnelingRequest : IKnxPacket
    {
        public KnxNetIPHeader Header { get; private set; } = new KnxNetIPHeader() {HeaderSize = 0x06, Version = 0x10, ServiceType = 0x0420};
        public KnxNetBodyConnectionHeader ConnectionHeader { get; set; }
        public CommonExternalMessageInterface Message { get; set; } = new CommonExternalMessageInterface();

        public byte[] GetBytes()
        {
            byte[] conHeader = ConnectionHeader.GetBytes();
            byte[] msg = Message.GetBytes();

            Header.Size = (short)(conHeader.Length + msg.Length + 6);
            byte[] header = Header.GetBytes();

            byte[] buffer = new byte[header.Length + conHeader.Length + msg.Length];

            Array.Copy(header, 0, buffer, 0, header.Length);
            Array.Copy(conHeader, 0, buffer, header.Length, conHeader.Length);
            Array.Copy(msg, 0, buffer, header.Length + conHeader.Length, msg.Length);

            return buffer;
        }

        public static TunnelingRequest Parse(byte[] buffer, int index)
        {
            TunnelingRequest request = new TunnelingRequest
            {
                Header = KnxNetIPHeader.Parse(buffer, index),
                ConnectionHeader = KnxNetBodyConnectionHeader.Parse(buffer, index + 6)
            };

            int cemiFrameSize = request.Header.Size - request.ConnectionHeader.StructureLength -
                                request.Header.HeaderSize;

            if (cemiFrameSize == 0)
                return request;

            CommonExternalMessageInterface msg;
            CommonExternalMessageInterface.TryParse(buffer, index + 10, cemiFrameSize, out msg);
            request.Message = msg;

            return request;
        }
    }
}
