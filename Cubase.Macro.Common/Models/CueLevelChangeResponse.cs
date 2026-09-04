namespace Cubase.Macro.Common.Models
{
    // var payLoad = JSON.stringify({ChannelIndex: channelIndex, CueSlot: cueIdx, Volume: value});

    public class CueLevelChangeResponse
    {
        public int ChannelIndex { get; set; }

        public int CueSlot { get; set; }

        public double Volume { get; set; }
    }
}
