using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Cubase.Macro.Common.Lyrics
{
    public class LyricBuffer : List<LyricViewModel>
    {
        public void AddBlank(int count = 1)
        {
            for (int i=0; i < count; i++)
            {
                this.Add(LyricViewModel.CreateEmptyLine());
            }
        }

        public List<LyricViewModel> GetLyricLines()
        {
            return this.Where(x => x.Lyric != null).ToList();
        }

        public int GetLineIndex(string lineText)
        {
            return this.Select(x => x.Lyric).Where(x => x != null).ToList().IndexOf(lineText);
        }

        public string ToText()
        {
            return string.Join(Environment.NewLine, this.Select(x => x.Lyric)
                                                        .Where(x => x != null).ToArray());
        }
    }

    public class LyricViewModel
    {
        public string? Lyric { get; set; }

        public object ForeColour { get; set; }

        public double TimeLine { get; set; } = -1;

        public bool HasBeenScrolled { get; set; } = false;

        public static LyricViewModel CreateEmptyLine()
        {
            return new LyricViewModel() { Lyric = string.Empty };
        }
    }
}
