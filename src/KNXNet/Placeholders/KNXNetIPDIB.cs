namespace KNXNet.Placeholders
{
    /// <summary>
    /// Description Information Block
    /// </summary>
    public class KNXNetIPDIB
    {
        public byte Lenght { get; set; }
        public byte TypeCode { get; set; }
        public byte[] BlockData { get; set; }
    }
}
