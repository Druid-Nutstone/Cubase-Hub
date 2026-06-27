using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Lyrics
{
    public static class LyricExtentions
    {
        public static string LyricFullPath(this string fileName)
        {
            return Path.Combine(CubaseMacroMobileConstants.LyricSourceFolder, fileName);
        }
    }
}
