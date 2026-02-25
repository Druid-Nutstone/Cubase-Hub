using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Services.Models
{
    public class FlacConfiguration
    {
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Default;

        public BitDepth Depth { get; set; } = BitDepth.Bit32;

        public SampleRate SampleRate { get; set; } = SampleRate.Herts44;
    }
}
