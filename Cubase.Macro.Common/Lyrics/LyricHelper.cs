using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Common.Lyrics
{
    public static class LyricHelper
    {
        public static bool IsControlLine(this string line)
        {
            return line.StartsWith("{");
        }

        public static double GetTimeSeconds(this string mmss)
        {
            var timeBits = mmss.Split(":");
            if (timeBits.Length < 2)
            {
                return -1;
            }

            var min = int.Parse(timeBits[0]);
            var sec = int.Parse(timeBits[1]);
            var asTime = new TimeOnly(0, min, sec);
            return asTime.ToTimeSpan().TotalSeconds;
        } 
        public static string NewLine(this string line)
        {
            return $"{line}{Environment.NewLine}";
        }
        public static (ControlLyricKeyword keyWord, string value)? ControlTypeAndValue(this string line)
        {
            var bits = line.Replace("{", "").Replace("}", "").Trim().Split(":");

            var keyWord = Enum.GetNames<ControlLyricKeyword>()
                              .FirstOrDefault(x => x.Equals(bits[0], StringComparison.OrdinalIgnoreCase));
            if (keyWord != null)
            {
                return (Enum.Parse<ControlLyricKeyword>(keyWord), bits[1]); 
            }
            return null;
        } 

        public static bool HaveChords(this string line)
        {
            return line.Contains("[");
        }

        public static LyricChord GetChords(this string line, LyricChord lyricChord)
        {
            while (lyricChord.Lyric.Contains("["))
            {
                var startIndex = lyricChord.Lyric.IndexOf("[");
                var endIndex = lyricChord.Lyric.IndexOf("]", startIndex);
                var chordWithDelimiters = lyricChord.Lyric.Substring(startIndex, endIndex - startIndex + 1);
                lyricChord.Lyric = lyricChord.Lyric.Remove(startIndex, endIndex - startIndex + 1);
                lyricChord.Chords.Add(new ChordLocation()
                {
                    Chord = chordWithDelimiters.Replace("[", "").Replace("]", "").Trim(),
                    Location = startIndex
                });
            }
            return lyricChord;
        } 
    }
}
