using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Lyrics
{
    public class ScrollResponse
    {
        public int ScrollLine { get; set; } = -1;

        public ScrollResponseType ScrollType { get; set; } = ScrollResponseType.Nop;   
        
        public TimeSpan TransportLocation {  get; set; }
    }

    public enum ScrollResponseType
    {
        Scroll = 0,
        Nop = 1,
        Stop = 2
    }
}
