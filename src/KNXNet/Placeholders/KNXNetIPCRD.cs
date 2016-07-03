namespace KNXNet.Placeholders
{
    /// <summary>
    /// KNXnet/IP COnnection Response Data Block
    /// </summary>
    public class KNXNetIPCRD
    {
        /// <summary>
        /// Structure lenght
        /// </summary>
        public byte Lenght { get; set; }
        /// <summary>
        /// Connection type code
        /// </summary>
        public byte ConnectionTypeCode { get; set; }
        /// <summary>
        /// Optional
        /// </summary>
        public byte[] IndependantData { get; set; }
        /// <summary>
        /// Optional
        /// </summary>
        public byte[] DependantData { get; set; }
    }
}
