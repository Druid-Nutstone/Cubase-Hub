using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Models
{
    // TrackName: tracks[channelIndex].Name, CueIndex: cueIndex, CueLevel: tracks[channelIndex].CueLevels[cueIndex]}
    public class CueLevelChange
    {
        public string Id { get; set; }
        
        public string CueName { get; set; }

        public string TrackName { get; set; }

        public int TrackIndex { get; set; }

        public int CueIndex { get; set; } = -1;

        public double CueLevel { get; set; } = -1;

        public int CueEnabled { get; set; } = 0;

        public bool Mute { get; set; }

        public bool Solo { get; set; }

        public bool RecordEnable { get; set; }

        public bool Selected { get; set; }

        public string TrackType { get; set; }

        public MidiCueMessageType CueMessageType { get; set; } = MidiCueMessageType.NotSpecified;
    }

    /*
     var MidiCueMessageType = {
    Enable : 0,
    Volume : 1,
    Solo : 2,
    Mute : 3,
    RecordEnable: 4,
    NotSpecified : 5
} 
     */

    public enum MidiCueMessageType
    {
        Enable = 0,
        Volume = 1,
        Solo = 2,
        Mute = 3,
        RecordEnable = 4,
        Selected = 5,
        TrackType = 6,
        TrackVolume = 7,
        Name = 8,
        NotSpecified = 9
    }
}
