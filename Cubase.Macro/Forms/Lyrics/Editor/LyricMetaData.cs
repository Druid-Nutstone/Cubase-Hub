using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics.Editor
{
    public class TitleMetaData
    {
        public string Title { get; set; }
        public string FileName { get; set; }
    }

    public class LyricMetaData
    {
        public List<TitleMetaData> SongTitles { get; set; } = new List<TitleMetaData>();

        public List<string> Albums { get; set; } = new List<string>();
    }
}
