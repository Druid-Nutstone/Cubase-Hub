using Cubase.Macro.Common.Lyrics.Scrolling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Cubase.Macro.Common.Lyrics.Services
{
    public class LyricService : ILyricService
    {
        private readonly IColourService colourService;

        private LyricChordCollection lyricCollection;

        private LyricBuffer lyricBuffer;

        private Dictionary<ControlLyricKeyword, Func<Section, LyricBuffer, LyricChordCollection, LyricViewModel, LyricViewModel>> commandParsers;

        private Dictionary<ScrollingStrategy, Func<ScrollResponse>> scrollProcessors;

        private Dictionary<ScrollingStrategy, Action> scrollStartProcessors;
        
        private ScrollingStrategy scrollingStrategy;

        private double duration;

        private double start = 0;

        private DateTime scrollingStartedAt;

        public LyricService(IColourService colourService) 
        {
            this.colourService = colourService;
            this.commandParsers = new Dictionary<ControlLyricKeyword, Func<Section, LyricBuffer, LyricChordCollection, LyricViewModel, LyricViewModel>>()
            {
                { ControlLyricKeyword.Title, ProcessTitle },
                { ControlLyricKeyword.Sov, ProcessVerse },
                { ControlLyricKeyword.Start_Of_Verse, ProcessVerse },
                { ControlLyricKeyword.Eov, ProcessEndVerse },
                { ControlLyricKeyword.End_Of_Verse, ProcessEndVerse },
                { ControlLyricKeyword.Duration, ProcessDuration },
                { ControlLyricKeyword.D_Time, ProcessTime },
                { ControlLyricKeyword.SoS, ProcessSection },
                { ControlLyricKeyword.Start_of_Section, ProcessSection },
                { ControlLyricKeyword.End_of_Section, ProcessEndSection },
                { ControlLyricKeyword.Eos, ProcessEndSection },
                { ControlLyricKeyword.Start_of_Chorus, ProcessChorus },
                { ControlLyricKeyword.Soc, ProcessChorus },
                { ControlLyricKeyword.Eoc, ProcessEndChorus },
                { ControlLyricKeyword.End_of_Chorus, ProcessEndChorus },
                { ControlLyricKeyword.Start, ProcessStart },
            };

            this.scrollStartProcessors = new Dictionary<ScrollingStrategy, Action>() 
            {
                { ScrollingStrategy.Duration, StartDurationProcessor },
                { ScrollingStrategy.Time, StartDurationProcessor } // starts as duration does 
            };

            this.scrollProcessors = new Dictionary<ScrollingStrategy, Func<ScrollResponse>>()
            {
                { ScrollingStrategy.Duration, DurationProcessor },
                { ScrollingStrategy.Time, TimeProcessor }
            }; 
        }

        public object GetTextColour(int lineIndex)
        {
            var bufferIndex = this.lyricBuffer.GetLyricLines()[lineIndex];
            if (bufferIndex != null)
            {
                return bufferIndex.ForeColour == null ? colourService.GetDefaultColour() : bufferIndex.ForeColour;
            }
            return this.colourService.GetDefaultColour();
        }

        public LyricBuffer ParseLyrics(IEnumerable<string> lyricSource, int padding, char paddingChar)
        {
            this.lyricCollection = LyricChordParser.FromLines(lyricSource);
            this.lyricBuffer = new LyricBuffer(padding, paddingChar);
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

                    lyricViewModel = this.lyricBuffer.AddLyric(cb.ToString(), this.colourService.GetChordsColour()); 
                }
                if (line.Content.HaveLyric())
                {
                    lyricViewModel = this.lyricBuffer.AddLyric(line.Content.Lyric, this.colourService.GetDefaultColour());
                }
            }
            return this.lyricBuffer;
        }

        private LyricViewModel ProcessTitle(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var newViewModel = buffer.AddLyric(source.GetControlValue(ControlLyricKeyword.Title), this.colourService.GetTitleColour());
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

            var newViewModel = buffer.AddLyric(verseName, this.colourService.GetSectionColour());
            return newViewModel;
        }

        private LyricViewModel ProcessSection(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var sectionName = source.GetControlValue(ControlLyricKeyword.Start_of_Section);
            if (sectionName == null)
            {
                sectionName = source.GetControlValue(ControlLyricKeyword.SoS);
            }
            var newViewModel = buffer.AddLyric(sectionName, this.colourService.GetSectionColour());
            return newViewModel;
        }

        private LyricViewModel ProcessEndSection(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var newViewModel = new LyricViewModel();
            buffer.Add(newViewModel);
            buffer.AddBlank(2);
            return newViewModel;
        }

        private LyricViewModel ProcessChorus(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var chorusName = source.GetControlValue(ControlLyricKeyword.Start_of_Chorus);
            if (chorusName == null)
            {
                chorusName = source.GetControlValue(ControlLyricKeyword.Soc);
            }
            var newViewModel = buffer.AddLyric(chorusName, this.colourService.GetSectionColour());
            return newViewModel;
        }

        private LyricViewModel ProcessEndChorus(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var newViewModel = new LyricViewModel();
            buffer.Add(newViewModel);
            buffer.AddBlank(2);
            return newViewModel;
        }

        private LyricViewModel ProcessEndVerse(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var newViewModel = new LyricViewModel();
            buffer.Add(newViewModel);
            buffer.AddBlank(2);
            return newViewModel;
        }

        private LyricViewModel ProcessTime(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            this.scrollingStrategy = ScrollingStrategy.Time;
            if (currentView != null)
            {
                var timeLine = source.GetControlValue(ControlLyricKeyword.D_Time);
                if (timeLine != null)
                {
                    currentView.TimeLine = timeLine.GetTimeSeconds();
                }
            }
            return currentView;
        }

        private LyricViewModel ProcessDuration(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            this.scrollingStrategy = ScrollingStrategy.Duration;
            this.duration = source.GetControlValue(ControlLyricKeyword.Duration).GetTimeSeconds(); 
            return currentView;
        }

        private LyricViewModel ProcessStart(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            this.start = source.GetControlValue(ControlLyricKeyword.Start).GetTimeSeconds();
            return currentView;
        }

        public void StartScrolling()
        {
            // if start is specified - amend the started at to the left locator 
            this.scrollingStartedAt = DateTime.Now.AddSeconds(this.start > 0 ? this.start * -1 : 0);
            this.scrollStartProcessors[this.scrollingStrategy]();
        }
         
        public ScrollResponse Scroll()
        {
            this.lyricBuffer.Reset();
            return this.scrollProcessors[this.scrollingStrategy]();  
        }

        private bool ShouldStop(int targetLine = -1)
        {
            var totalLines = this.lyricBuffer.GetLyricLines().Count - 1;
            return targetLine > 0 
                ? targetLine >= totalLines 
                : TimeSpan.FromTicks(DateTime.Now.Ticks - this.scrollingStartedAt.Ticks).TotalSeconds > this.duration;
        }

        #region scroller methods.

        #region Duration 
        private void StartDurationProcessor()
        {
            var totalLines = this.lyricBuffer.GetLyricLines().Count - 1;
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
                TransportLocation = timeSpan,
                ScrollType = ShouldStop(targetLine) ? ScrollResponseType.Stop : ScrollResponseType.Scroll 
            };
        }
        #endregion
        #region Time processor
        private ScrollResponse TimeProcessor()
        {
            // get elapsed time so far 
            var timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - this.scrollingStartedAt.Ticks);
            // get first lyricline/verse/chorus etc that is =>
            var timelineTarget = this.lyricBuffer.GetTimelineGreateOrEqualTo(timeSpan.TotalSeconds);
            Debug.WriteLine($"Current Time: {timeSpan.TotalSeconds} Target Time: {timelineTarget?.TimeLine}");
            if (timelineTarget != null && !timelineTarget.HasBeenScrolled)
            {
                if (timelineTarget.TimeLine.IsWithinThreshold(timeSpan.TotalSeconds))
                {
                    var lineNumber = this.lyricBuffer.GetIndex(timelineTarget);
                    timelineTarget.HasBeenScrolled = true;
                    return new ScrollResponse()
                    {
                        ScrollLine = this.lyricBuffer.GetIndex(timelineTarget),
                        TransportLocation = timeSpan,
                        ScrollType = ShouldStop(lineNumber) ? ScrollResponseType.Stop : ScrollResponseType.Scroll,
                    };
                }
            }
            return new ScrollResponse() 
            { 
                TransportLocation = timeSpan,
                ScrollType = timeSpan.TotalSeconds > this.duration ? ScrollResponseType.Stop : ScrollResponseType.Nop
            };
        }
        #endregion 
        #endregion
    }
}