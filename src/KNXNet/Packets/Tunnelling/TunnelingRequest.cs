using System;

namespace KNXNet.Packets.Tunnelling
{
    public class TunnelingRequest
    {
        public KNXNetIPHeader Header { get; private set; } = new KNXNetIPHeader() {HeaderSize = 0x06, Version = 0x10, ServiceType = 0x0420};
        public KNXNetBodyConnectionHeader ConnectionHeader { get; set; }
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
                Header = KNXNetIPHeader.Parse(buffer, index),
                ConnectionHeader = KNXNetBodyConnectionHeader.Parse(buffer, index + 6)
            };

            CommonExternalMessageInterface msg;
            CommonExternalMessageInterface.TryParse(buffer, index + 10, request.Header.Size - request.ConnectionHeader.StructureLength - request.Header.HeaderSize, out msg);
            request.Message = msg;

            return request;
        }
    }
}
