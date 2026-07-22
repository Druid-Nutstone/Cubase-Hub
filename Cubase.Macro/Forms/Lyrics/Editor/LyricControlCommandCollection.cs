using Cubase.Macro.Common.Lyrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Lyrics.Editor
{
    public class LyricControlCommandCollection : List<LyricControlCommand>
    {
        public LyricControlCommandCollection()
        {
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Title, "{title:xxx}", "Song Title", null));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Font_Size, "{font_size:24}", "Font Size", "24"));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Duration, "{duration:00:00}", "Total duraction of track in mm:ss", null));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Beats_Per_Bar, "{beats_per_bar:4", "Time signature (4/4 = 4 , 3/4 = 3)", ""));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Tempo, "{tempo:120}", "BPM", "120"));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Sov, "{sov:xxx}", "Verse Start", null));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Bar, "{bar:0}", "Bar number to move to", "0"));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Eov, "{eov}", "Verse End", null));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Soc, "{soc:xxx}", "Chorus Start", null));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Eoc, "{eoc}", "Chorus End", null));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.SoS, "{sos:xxx}", "Section Start (Bridge, Solo etc)", null));
            this.Add(LyricControlCommand.Create(ControlLyricKeyword.Eos, "{eos}", "Section End", null));
        }
    }

    public class LyricControlCommand
    {
        public ControlLyricKeyword Keyword { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public string? DefaultValue { get; set; }

        public static LyricControlCommand Create(ControlLyricKeyword keyword, string text, string description, string? defaultValue)
        {
            return new LyricControlCommand()
            {
                 Keyword = keyword,
                 Text = text,
                 Description = description,
                 DefaultValue = defaultValue
            };
        }
    }
}
