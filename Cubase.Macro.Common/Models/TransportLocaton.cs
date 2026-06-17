using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Models
{
    public class TransportLocaton
    {
        public string Location { get; set; }

        public TransportType LocationType { get; set; }
    }

    public enum TransportType 
    {
        Seconds = 0,
        BarsBeats = 1,
        Timecode = 2,
        Samples = 3,
        NotSpecified = 4
    }
}
