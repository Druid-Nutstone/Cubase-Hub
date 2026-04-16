using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Models
{
    public class CubaseMidiCommand
    {
        public string Command { get; set; } 

        public int Note { get; set; } = 0;

        public int Channel { get; set; } = 0;
    }
}
