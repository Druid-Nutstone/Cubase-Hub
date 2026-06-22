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
        
        public static bool IsWithinThreshold(this double timeLine, double currentTime, double threshold = 5)
        {
            return (timeLine - currentTime) <= threshold;
        }
        public static string AddLeftPadding(this string line, int padding, char paddingChar)
        {
            if (padding == 0)
            {
                return line;
            }
            var pad = new string(paddingChar, padding);
            return $"{pad}{line}";
        }

        public static double GetTimeSeconds(this string mmss)
        {
            var timeBits = mmss.Split(":");
            if (timeBits.Length < 2)
            {
                return double.Parse(mmss);
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
                if (bits.Length > 1)
                {
                    var value = bits[1];
                    if (bits.Length > 2) // contains more than one ':'
                    {
                        value = string.Join(':', bits.TakeLast(bits.Length - 1));
                    }
                    return (Enum.Parse<ControlLyricKeyword>(keyWord), value);
                }
                else
                {
                    return (Enum.Parse<ControlLyricKeyword>(keyWord), null);
                }
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
