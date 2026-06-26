using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Models
{
    /*
var sysexCommand = {
    ClearChannels: "ClearChannels",
    ChannelChange: "ChannelChange",
    Message: "Message",
    Ready: "Ready",
    CommandValueChanged: "CommandValueChanged",
    Tracks: "Tracks",
    TrackUpdate: "TrackUpdate",
    TrackComplete: "TrackComplete",
    TrackSelectionChanged: "TrackSelectionChanged",
    CommandComplete: "CommandComplete",
    CueLevelChange: "CueLevelChange",
    UpdateCueLevel: "UpdateCueLevel",
    UpdateCueLevelComplete: "UpdateCueLevelComplete",
    GetCueLevels: "GetCueLevels",
    GetCueLevelsComplete : "GetCueLevelsComplete",
};     
     */

    public enum MidiCommand
    {
       Ready,
       ClearChannels,
       CueLevelChange,
       ChannelChange, 
       UpdateCueLevel,
       UpdateCueLevelComplete,
       GetCueLevels,
       GetCueLevelsComplete,
       CommandValueChanged,
       CommandComplete,
       Failed,
       AckUpdate,
       TrackDeleted,
       Reload,
       UpdateMute,
       UpdateSolo,
       LocationChanged,
       StartTransportEventMonitoring,
       StopTransportEventMonitoring
    }
}
