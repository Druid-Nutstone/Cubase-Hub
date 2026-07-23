using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Services.Mswin
{
    public interface IMsWinService
    {
        string LyricFile { get; }
        
        bool HaveParameters();

        bool HaveLyricFile();
    }
}
