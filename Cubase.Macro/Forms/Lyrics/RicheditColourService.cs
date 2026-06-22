using Cubase.Macro.Common.Lyrics.Scrolling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics
{
    public class RicheditColourService : IColourService
    {
        public object GetChordsColour()
        {
            return Color.IndianRed;
        }

        public object GetDefaultColour()
        {
            return DarkTheme.TextColor;
        }

        public object GetSectionColour()
        {
            return Color.Yellow;
        }

        public object GetTitleColour()
        {
            return Color.Cyan;
        }
    }
}
