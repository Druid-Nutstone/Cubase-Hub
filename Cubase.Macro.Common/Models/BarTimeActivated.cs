using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Models
{
    public class BarTimeActivated
    {
        public int Bar { get; set; }

        public TimeSpan BarTime { get; set; }
    }
}
