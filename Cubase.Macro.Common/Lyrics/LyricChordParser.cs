using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Cubase.Macro.Common.Lyrics
{
    public class LyricChordParser
    {
        public LyricChordCollection LyricsChords { get; set; } = new LyricChordCollection();

        public LyricChordParser() 
        { 
        
        }

        public LyricChordParser(string[] lines)
        {
            this.Parse(lines);
        }

        public LyricChordParser(string fileName)
        {
            this.LyricsChords = this.Parse(fileName);
        }

        public LyricChordCollection? Parse(string[] lines)
        {
            return this.ParseLines(lines);  
        }

        public LyricChordCollection? Parse(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            var lines = File.ReadAllLines(fileName);
            return this.ParseLines(lines);
        }

        private LyricChordCollection ParseLines(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                if (line.Trim().Length > 0)
                {
                    var section = this.LyricsChords.AddSection();
                    if (line.IsControlLine())
                    {
                        var valuePair = line.ControlTypeAndValue();
                        if (valuePair != null)
                        {
                            section.Controls[valuePair.Value.keyWord] = valuePair.Value.value;
                        }
                    }
                    else // must be a lyric line with potential chords 
                    {
                        if (line.HaveChords())
                        {
                            section.Content = line.GetChords(LyricChord.CreateFromLine(line));
                        }
                        else
                        {
                            section.Content.Lyric = line;
                        }
                    }
                }
            }
            return this.LyricsChords;
        }

        public static LyricChordCollection FromLines(IEnumerable<string> lines)
        {
            var parser = new LyricChordParser();
            return parser.ParseLines(lines);
        }
    }
}
