using Cubase.Macro.Common.Lyrics.Scrolling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics
{
    public class RicheditScroller : IScroller
    {
        public object GetChordsColour()
        {
            return Color.IndianRed;
        }

        public object GetDefaultColour()
        {
            return Color.White;
        }

        public object GetTitleColour()
        {
            return Color.Yellow;
        }
    }
}
