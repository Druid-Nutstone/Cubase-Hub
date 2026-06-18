using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Lyrics
{
    public class LyricChordCollection : List<Section>
    {
        public LyricChordCollection() 
        { 
            
        }

        public string? GetControlValue(ControlLyricKeyword keyWord)
        {
            var cntrl = this.SelectMany(x => x.Controls).Where(x => x.Key == keyWord);
            if (cntrl.Any())
            {
                return cntrl.First().Value;
            }
            return null;    
        }

        public Section AddSection()
        {
            var sect = new Section();
            this.Add(sect);
            return sect;
        }

        public IEnumerable<string> RenderToText()
        {
            return this.Render((ctrol, value, output) => 
            { 
                switch (ctrol)
                {
                    case ControlLyricKeyword.Sov:
                    case ControlLyricKeyword.Soc:
                    case ControlLyricKeyword.Start_Of_Verse:
                        output.Add(string.Empty);
                        output.Add(value);
                        break;
                    case ControlLyricKeyword.End_of_Chorus:
                    case ControlLyricKeyword.End_Of_Verse:
                        output.Add(string.Empty);    
                        break;
                }
            });
            
        }

        public IEnumerable<string> Render(Action<ControlLyricKeyword, string, List<string>>? callBack)
        {
            var output = new List<string>();
            foreach (var sect in this)
            {
                if (sect.HaveControls())
                {
                    foreach (var item in sect.Controls.ToList())
                    {
                        callBack?.Invoke(item.Key, item.Value, output);
                    }
                }
                if (sect.Content.HaveChords())
                {
                    var sb = new StringBuilder();
                    foreach (var item in sect.Content.Chords)
                    {
                        if (item.Location > sb.Length)
                        {
                            sb.Append(' ', item.Location - sb.Length);
                        }
                        sb.Insert(item.Location, item.Chord);
                    }
                    output.Add(sb.ToString());  
                }
                if (sect.Content.HaveLyric())
                {
                    output.Add(sect.Content.Lyric);
                }
            }
            return output;
        }

    }

    public class Section    
    {
        public Dictionary<ControlLyricKeyword, string> Controls { get; set; } = new Dictionary<ControlLyricKeyword, string>(); 
        
        public LyricChord Content { get; set; } = new LyricChord();
    
        public bool HaveControls()
        {
            return this.Controls.Any();
        }

        public bool HasKey(ControlLyricKeyword controlLyricKeyword)
        {
            return this.Controls.ContainsKey(controlLyricKeyword);
        }
    
        public string? GetControlValue(ControlLyricKeyword controlLyricKeyword)
        {
            if (this.HasKey(controlLyricKeyword))
            {
                return this.Controls[controlLyricKeyword];
            }
            return null;
        }
    }


    public class LyricChord 
    {
        public string? Lyric { get; set; } = null;
    
        public List<ChordLocation> Chords { get; set; } = new List<ChordLocation>(); 
         
        public bool HaveChords()
        {
            return this.Chords.Any();
        }

        public bool HaveLyric()
        {
            return this.Lyric != null;
        }
        public static LyricChord CreateFromLine(string line)
        {
            return new LyricChord() { Lyric = line };
        }
    }

    public class ChordLocation
    {
        public int Location {  get; set; }

        public string Chord { get; set; }
    }

    public enum ControlLyricKeyword
    {
        Title,
        Duration,
        Book,
        Tempo,
        Comment,
        D_Time,
        Soc, // start of chorus
        Sov, // start of verse {sov: Verse 1}
        Eoc, // end of chorus  
        Eov, // end of verse
        Start_Of_Verse,
        End_Of_Verse,
        Start_of_Chorus,
        End_of_Chorus,
    }
}
