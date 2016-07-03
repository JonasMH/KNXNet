namespace KNXNet.Placeholders
{
    public class KNXNetIPCRI
    {
        public byte Lenght { get; set; }
        public byte ConnectionType { get; set; }
        public byte[] IndependantData { get; set; }
        public byte[] DependantData { get; set; }
    }
}
