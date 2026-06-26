using Cubase.Macro.Common.Lyrics.Scrolling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Services
{
    public class ColourService : IColourService
    {
        public object GetChordsColour()
        {
            return Colors.IndianRed;
        }

        public object GetDefaultColour()
        {
            return Colors.White;
        }

        public object GetSectionColour()
        {
            return Colors.Yellow;
        }

        public object GetTitleColour()
        {
            return Colors.Cyan;
        }
    }
}
