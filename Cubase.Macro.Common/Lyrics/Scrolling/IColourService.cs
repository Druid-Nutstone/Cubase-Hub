using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Lyrics.Scrolling
{
    public interface IColourService
    {
        object GetTitleColour();

        object GetChordsColour();

        object GetDefaultColour();

        object GetSectionColour();
    
    }
}
