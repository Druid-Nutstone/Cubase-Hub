using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Lyrics.Services
{
    public interface ILyricService
    {
        LyricBuffer ParseLyrics(IEnumerable<string> lyricSource, int padding, char paddingChar);

        object GetTextColour(int lineIndex);

        void StartScrolling();

        ScrollResponse Scroll();
    }
}
