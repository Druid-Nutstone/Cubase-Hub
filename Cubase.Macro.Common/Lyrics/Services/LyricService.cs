using Cubase.Macro.Common.Lyrics.Scrolling;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Cubase.Macro.Common.Lyrics.Services
{
    public class LyricService : ILyricService
    {
        private readonly IScroller scroller;

        private LyricChordCollection lyricCollection;

        private LyricBuffer lyricBuffer;

        private Dictionary<ControlLyricKeyword, Func<Section, LyricBuffer, LyricChordCollection, LyricViewModel, LyricViewModel>> commandParsers;

        private Dictionary<ScrollingStrategy, Func<ScrollResponse>> scrollProcessors;

        private Dictionary<ScrollingStrategy, Action> scrollStartProcessors;
        
        private ScrollingStrategy scrollingStrategy;

        private double duration;

        private DateTime scrollingStartedAt;

        public LyricService(IScroller scroller) 
        { 
            this.scroller = scroller;
            this.commandParsers = new Dictionary<ControlLyricKeyword, Func<Section, LyricBuffer, LyricChordCollection, LyricViewModel, LyricViewModel>>()
            {
                { ControlLyricKeyword.Title, ProcessTitle },
                { ControlLyricKeyword.Sov, ProcessVerse },
                { ControlLyricKeyword.Start_Of_Verse, ProcessVerse },
                { ControlLyricKeyword.Eov, ProcessEndVerse },
                { ControlLyricKeyword.End_Of_Verse, ProcessEndVerse },
                { ControlLyricKeyword.Duration, ProcessDuration }
            };

            this.scrollStartProcessors = new Dictionary<ScrollingStrategy, Action>() 
            {
                { ScrollingStrategy.Duration, StartDurationProcessor }
            };

            this.scrollProcessors = new Dictionary<ScrollingStrategy, Func<ScrollResponse>>()
            {
                { ScrollingStrategy.Duration, DurationProcessor }
            }; 
        }

        public object GetTextColour(int lineIndex)
        {
            var bufferIndex = this.lyricBuffer.GetLyricLines()[lineIndex];
            if (bufferIndex != null)
            {
                return bufferIndex.ForeColour;
            }
            return this.scroller.GetDefaultColour();
        }

        public LyricBuffer ParseLyrics(IEnumerable<string> lyricSource)
        {
            this.lyricCollection = LyricChordParser.FromLines(lyricSource);
            this.lyricBuffer = new LyricBuffer();
            LyricViewModel lyricViewModel = null;
            foreach (var line in lyricCollection)
            {
                if (line.HaveControls())
                {
                    foreach (var cntrl in line.Controls)
                    {
                        if (this.commandParsers.ContainsKey(cntrl.Key))
                        {
                            lyricViewModel = this.commandParsers[cntrl.Key](line, this.lyricBuffer, this.lyricCollection, lyricViewModel);
                        }
                    }
                }
                if (line.Content.HaveChords())
                {
                    var cb = new StringBuilder();
                    foreach (var item in line.Content.Chords)
                    {
                        if (item.Location > cb.Length)
                        {
                            cb.Append(' ', item.Location - cb.Length);
                        }
                        cb.Insert(item.Location, item.Chord);
                    }

                    lyricBuffer.Add(new LyricViewModel()
                    {
                        Lyric = cb.ToString(),
                        ForeColour = this.scroller.GetChordsColour()
                    });


                }
                if (line.Content.HaveLyric())
                {
                    lyricBuffer.Add(new LyricViewModel()
                    {
                        Lyric = line.Content.Lyric,
                        ForeColour = this.scroller.GetDefaultColour()
                    });
                }
            }
            return this.lyricBuffer;
        }

        private LyricViewModel ProcessTitle(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var newViewModel = new LyricViewModel()
            {
                Lyric = source.GetControlValue(ControlLyricKeyword.Title),
                ForeColour = this.scroller.GetTitleColour()
            };
            buffer.Add(newViewModel);
            buffer.AddBlank(2);
            return newViewModel;
        }

        private LyricViewModel ProcessVerse(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var verseName = source.GetControlValue(ControlLyricKeyword.Start_Of_Verse);
            if (verseName == null)
            {
                verseName = source.GetControlValue(ControlLyricKeyword.Sov);
            }
            
            var newViewModel = new LyricViewModel()
            {
                Lyric = verseName,
                ForeColour = this.scroller.GetTitleColour()
            };
            buffer.Add(newViewModel);
            return newViewModel;
        }

        private LyricViewModel ProcessEndVerse(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var newViewModel = new LyricViewModel();
            buffer.Add(newViewModel);
            buffer.AddBlank(2);
            return newViewModel;
        }

        private LyricViewModel ProcessDuration(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            this.scrollingStrategy = ScrollingStrategy.Duration;
            this.duration = source.GetControlValue(ControlLyricKeyword.Duration).GetTimeSeconds(); 
            return currentView;
        }

        public void StartScrolling()
        {
            this.scrollingStartedAt = DateTime.Now;
            this.scrollStartProcessors[this.scrollingStrategy]();
        }
         
        public ScrollResponse Scroll()
        {
            return this.scrollProcessors[this.scrollingStrategy]();
        }

        #region scroller methods.

        #region Duration 
        private void StartDurationProcessor()
        {
          
        }

        private ScrollResponse DurationProcessor()
        {
            var totalLines = this.lyricBuffer.GetLyricLines().Count-1;
            var timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - this.scrollingStartedAt.Ticks);
            double secondsPerLine = duration / totalLines;
            int targetLine = (int)(timeSpan.TotalSeconds / secondsPerLine);

            var scrollLine = this.lyricBuffer[targetLine];

            var scrolllineIndex = scrollLine.HasBeenScrolled ? -1 : targetLine; 

            if (scrollLine != null)
            {
                scrollLine.HasBeenScrolled = true;
            }

            return new ScrollResponse()
            {
                 ScrollLine = scrolllineIndex,
                 ShouldStop = targetLine >= totalLines 
            };
        }
        #endregion
        #endregion
    }
}