namespace KNXNet.DatapointTypes
{
    public class SingleBoolean : IDatapointType
    {
        public int Id { get; } = 1;
        public bool B { get; set; }
    }
}
