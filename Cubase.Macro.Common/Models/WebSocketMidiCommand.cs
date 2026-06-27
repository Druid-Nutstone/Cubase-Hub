using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Models
{
    public enum WebSocketMidiCommand
    {
        MidiCommandList = 0,
        MidiCommand = 1,
        MidiTransportLocation = 2,
        MidiLyricCurrentProject = 3,
        MidiLyricStartTransportMonitoring = 4,
        MidiLyricStopTransportMonitoring = 5,
        MidiLyricIndex = 6,
        MidiLyricContent = 7,
        Error = 6
    }
}
