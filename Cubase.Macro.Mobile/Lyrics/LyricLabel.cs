using Cubase.Macro.Common.Lyrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Lyrics
{
    public class LyricLabel : Label
    {
        public LyricLabel() : base() 
        {
            this.FontFamily = "CustomMono";
        }
        
        public LineType Type { get; set; }
    }

    public enum LineType
    {
        Lyric = 0,
        Chord= 1,
    }
}
