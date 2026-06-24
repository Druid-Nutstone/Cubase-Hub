using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text;

namespace Cubase.Macro.Common.Lyrics
{
    public class LyricBuffer : List<LyricViewModel>
    {

        private int padding;
        
        private char paddingChar;

        public LyricBuffer(int padding, char paddingChar) 
        { 
           this.padding = padding;
           this.paddingChar = paddingChar;
        }
        
        public void Reset()
        {
            this.ForEach(x => x.HasBeenScrolled = false);
        } 

        public LyricViewModel AddLyric(string lyric, object foreColour, double timeLine = -1, bool hasBeenScrolled = false)
        {
            var lyricModel = new LyricViewModel()
            {
                Lyric = lyric.AddLeftPadding(padding, paddingChar),
                ForeColour = foreColour,
                TimeLine = timeLine,
                HasBeenScrolled = hasBeenScrolled
            }; 
            this.Add(lyricModel);
            return lyricModel;
        }

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

        public int GetIndex(LyricViewModel lyricViewModel)
        {
            return this.GetLineIndex(lyricViewModel.Lyric);
        } 

        public LyricViewModel? GetTimelineGreateOrEqualTo(double targetTimeline)
        {
            return this.FirstOrDefault(x => x.TimeLine >= targetTimeline);
        }

        public LyricViewModel? GetLyricBar(int targetBar)
        {
            return this.FirstOrDefault(x => x.Bar == targetBar);
        }

        public LyricViewModel? GetTimeLineWithinThreshold(double targetTimeLineSeconds, int threshold = 1)
        {
            return this.FirstOrDefault(x => Math.Abs(x.TimeLine - targetTimeLineSeconds) <= threshold);
        }
        public string ToText()
        {
            return string.Join(Environment.NewLine, this.Select(x => x.Lyric)
                                                        .Where(x => x != null).ToArray());
        }
    }

    public class LyricViewModel    {
        public string? Lyric { get; set; }

        public object ForeColour { get; set; }

        public double TimeLine { get; set; } = -1;

        public int Bar { get; set; } = -1;

        public bool HasBeenScrolled { get; set; } = false;

        public static LyricViewModel CreateEmptyLine()
        {
            return new LyricViewModel() { Lyric = string.Empty };
        }
    }
}
