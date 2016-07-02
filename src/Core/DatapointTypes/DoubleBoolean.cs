namespace Core.DatapointTypes
{
    public class DoubleBoolean : IDatapointType
    {
        public int Id { get; } = 2;
        public bool C { get; set; }
        public bool V { get; set; }
    }
}
