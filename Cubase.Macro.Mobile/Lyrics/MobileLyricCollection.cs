using Cubase.Macro.Common.Lyrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Mobile.Lyrics
{
    public class MobileLyricCollection : List<MobileLyric>
    {
        public MobileLyricCollection() { }
    
        public MobileLyricCollection(LyricBuffer lyrics)
        {
            foreach (var lyric in lyrics)
            {
                if (lyric.Lyric != null)
                {
                    this.Add(new MobileLyric()
                    {
                        Lyric = lyric.Lyric,
                        Bar = lyric.Bar,
                        LineType = lyric.LineType,
                        ForegoundColour = (Color)lyric.ForeColour
                    });
                }
            }
        }

        public int MaxBar()
        {
            return this.Max(x => x.Bar);
        }

        public MobileLyric? GetBar(int bar)
        {
           return  this.Where(l => l.Bar <= bar)
                       .OrderByDescending(l => l.Bar)
                       .FirstOrDefault();

            // return this.FirstOrDefault(x => x.Bar == bar);
        }

        public int GetIndex(MobileLyric mobileLyric)
        {
            return this.IndexOf(mobileLyric);
        }
    
    }

    public class MobileLyric
    {
        public string Lyric {  get; set; }

        public int Bar { get; set; } = -1;

        public LyricLineType LineType { get; set; }

        public Color ForegoundColour { get; set; }
    
    }
}
