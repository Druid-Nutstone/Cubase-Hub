using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics
{
    public interface ILyricEditor
    {
        void IncreaseFont();

        void DecreaseFont();

        void SetFontSize(int fontSize);
    }
}
