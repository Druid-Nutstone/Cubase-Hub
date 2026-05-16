using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Models
{
    // TrackName: tracks[channelIndex].Name, CueIndex: cueIndex, CueLevel: tracks[channelIndex].CueLevels[cueIndex]}
    public class CueLevelChange
    {
        public string TrackName { get; set; }

        public int TrackIndex { get; set; } = -1;

        public int CueIndex { get; set; } = -1;

        public double CueLevel { get; set; } = -1;

        public int CueEnabled { get; set; } = 0;

        public MidiCueMessageType CueMessageType { get; set; } = MidiCueMessageType.NotSpecified;
    }

    public enum MidiCueMessageType
    {
        Enable = 0,
        Volume = 1,
        NotSpecified = 2
    }
}
