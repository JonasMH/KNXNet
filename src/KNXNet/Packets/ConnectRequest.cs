using KNXNet.Placeholders;

namespace KNXNet.Packets
{
    public class ConnectRequest
    {
        public enum ConnectionType
        {
            DeviceManagement = 0x03,
            Tunnel = 0x04,
            RemoteLogging = 0x06,
            RemoteConfiguration = 0x06,
            ObjectServer = 0x08
        }

        public KNXNetIPHeader Header { get; set; }
        public KNXNetIPHPAI ControlEndpoint { get; set; }
        public KNXNetIPHPAI DataEndpoint { get; set; }
        public KNXNetIPCRI ConnectionRequest { get; set; }
    }
}