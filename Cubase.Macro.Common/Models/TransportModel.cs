namespace Cubase.Macro.Common.Models
{
    /*
     var transportModel = {
    Play: false,
    Stop: false,
    Cycle: false,
    PunchIn: false,
    PunchOut: false
}
     */


    public class TransportModel
    {
        public bool Play { get; set; } = false;

        public bool Stop { get; set; } = false;

        public bool Cycle { get; set; } = false;

        public bool PunchIn { get; set; } = false;

        public bool PunchOut { get; set; } = false;

        public bool Record { get; set; } = false;

    }
}
