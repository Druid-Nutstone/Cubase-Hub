using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Models
{
    public class CueUpdateRequest
    {
        // index of the cue in the channel (0-3)
        public int CueSlotIndex { get; set; }

        // index of the track 
        public int TrackIndex { get; set; }

        // new level for the cue (0.0 - 1.0)
        public double CueLevel { get; set; }
    }
}
