namespace Cubase.Macro.Common.Models
{
    public class UpdateTransport
    {
        public int Value { get; set; }

        public TransportUpdateValueType TransportValueType { get; set; }
    }

    public enum TransportUpdateValueType
    {
        Play = 0,
        Stop = 1,
        Record = 2,
        Cycle = 3,
        PunchIn = 4,
        PunchOut = 5,
    }
}
