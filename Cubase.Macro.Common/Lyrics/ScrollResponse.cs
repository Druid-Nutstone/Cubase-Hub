using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Lyrics
{
    public class ScrollResponse
    {
        public int ScrollLine { get; set; } = -1;

        public bool ShouldStop { get; set; } = false;     
    }
}
