using Cubase.Macro.Common.Lyrics.Scrolling;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Cubase.Macro.Common.Lyrics.Services
{
    public class LyricService : ILyricService
    {
        private readonly IColourService colourService;

        private readonly IlyricMidiService lyricMidiService;
        
        public LyricChordCollection LyricCollection { get; set; }

        private LyricBuffer lyricBuffer;

        private Dictionary<ControlLyricKeyword, Func<Section, LyricBuffer, LyricChordCollection, LyricViewModel, LyricViewModel>> commandParsers;

        private Dictionary<ScrollingStrategy, Func<ScrollResponse>> scrollProcessors;

        private Dictionary<ScrollingStrategy, Action> scrollStartProcessors;
        
        private ScrollingStrategy scrollingStrategy;

        private double duration = -1;

        private double start = 0;

        private bool haveTime = false;

        private bool haveBar = true;

        private bool useMidi = false;

        private int bpm = -1;

        private int bpb = -1; // beats per bar 

        private DateTime scrollingStartedAt;

        public LyricService(IColourService colourService, 
                            IlyricMidiService lyricMidiService) 
        {
            this.colourService = colourService;
            this.lyricMidiService = lyricMidiService;
            this.commandParsers = new Dictionary<ControlLyricKeyword, Func<Section, LyricBuffer, LyricChordCollection, LyricViewModel, LyricViewModel>>()
            {
                { ControlLyricKeyword.Title, ProcessTitle },
                { ControlLyricKeyword.Tempo, ProcessBPM },
                { ControlLyricKeyword.Beats_Per_Bar, ProcessBPB },
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
                { ControlLyricKeyword.Bar, ProcessBar },
            };

            this.scrollStartProcessors = new Dictionary<ScrollingStrategy, Action>() 
            {
                { ScrollingStrategy.Duration, StartDurationProcessor },
                { ScrollingStrategy.Time, StartDurationProcessor }, // starts as duration does 
                { ScrollingStrategy.Bar, StartDurationProcessor } // starts as duration does             
            };

            this.scrollProcessors = new Dictionary<ScrollingStrategy, Func<ScrollResponse>>()
            {
                { ScrollingStrategy.Duration, DurationProcessor },
                { ScrollingStrategy.Time, TimeProcessor },
                { ScrollingStrategy.Bar, BarProcessor }
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
            this.LyricCollection = LyricChordParser.FromLines(lyricSource);
            this.lyricBuffer = new LyricBuffer(padding, paddingChar);
            LyricViewModel lyricViewModel = null;
            foreach (var line in LyricCollection)
            {
                if (line.HaveControls())
                {
                    foreach (var cntrl in line.Controls)
                    {
                        if (this.commandParsers.ContainsKey(cntrl.Key))
                        {
                            lyricViewModel = this.commandParsers[cntrl.Key](line, this.lyricBuffer, this.LyricCollection, lyricViewModel);
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
                    lyricViewModel.LineType = LyricLineType.Chord;
                }
                if (line.Content.HaveLyric())
                {
                    lyricViewModel = this.lyricBuffer.AddLyric(line.Content.Lyric, this.colourService.GetDefaultColour());
                    lyricViewModel.LineType = LyricLineType.Lyric;
                }
            }
            return this.lyricBuffer;
        }

        public void UseMidi(bool useMidi)
        {
            if (useMidi && this.lyricMidiService.IsMidiAvailable()) 
            {
                this.useMidi = true;
            }
            this.useMidi = useMidi;
        }

        private LyricViewModel ProcessTitle(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            var newViewModel = buffer.AddLyric(source.GetControlValue(ControlLyricKeyword.Title), this.colourService.GetTitleColour());
            buffer.AddBlank(2);
            return newViewModel;
        }

        private LyricViewModel ProcessBPM(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            this.bpm = int.Parse(source.GetControlValue(ControlLyricKeyword.Tempo));
            return currentView;
        }

        private LyricViewModel ProcessBPB(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            this.bpb = int.Parse(source.GetControlValue(ControlLyricKeyword.Beats_Per_Bar));
            return currentView;
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
            this.haveTime = true;
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

        private LyricViewModel ProcessBar(Section source, LyricBuffer buffer, LyricChordCollection originalSource, LyricViewModel currentView)
        {
            this.haveBar = true;
            if (currentView != null)
            {
                var timeLine = source.GetControlValue(ControlLyricKeyword.Bar);
                if (timeLine != null)
                {
                    currentView.Bar = int.Parse(timeLine);
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
            if (duration > 0) 
            { 
                if (this.bpm > 0)
                {
                    this.scrollingStrategy = ScrollingStrategy.Bar;
                }
                else
                {
                    if (this.haveTime)
                    {
                        this.scrollingStrategy = ScrollingStrategy.Time;
                    }
                    else
                    {
                        this.scrollingStrategy = ScrollingStrategy.Duration;
                    }
                }
            }
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
                LocationType = TransportLocationType.Time,
                ScrollType = ShouldStop(targetLine) ? ScrollResponseType.Stop : ScrollResponseType.Scroll 
            };
        }
        #endregion
        #region Time processor
        private ScrollResponse TimeProcessor()
        {
            TimeSpan timeSpan = TimeSpan.Zero;
            LyricViewModel? timeLineTarget = null;
            if (this.useMidi)
            {
                var midiTransportLocation = this.lyricMidiService.GetTransportLocation();
                timeSpan = TimeSpan.FromSeconds(midiTransportLocation.SecondsTime.GetTimeSeconds());
                timeLineTarget = this.lyricBuffer.GetTimeLineWithinThreshold(timeSpan.TotalSeconds);
            }
            else
            {
                // get elapsed time so far 
                timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - this.scrollingStartedAt.Ticks);
                // get first lyricline/verse/chorus etc that is =>
                timeLineTarget = this.lyricBuffer.GetTimelineGreateOrEqualTo(timeSpan.TotalSeconds);
            }
            if (timeLineTarget != null)
            {
                if (timeLineTarget.TimeLine.IsWithinThreshold(timeSpan.TotalSeconds))
                {
                    var lineNumber = this.lyricBuffer.GetIndex(timeLineTarget);
                    timeLineTarget.HasBeenScrolled = true;
                    return new ScrollResponse()
                    {
                        ScrollLine = lineNumber,
                        TransportLocation = timeSpan,
                        LocationType = TransportLocationType.Time,
                        ScrollType = ShouldStop(lineNumber) ? ScrollResponseType.Stop : ScrollResponseType.Scroll,
                    };
                }
            }
            return new ScrollResponse() 
            { 
                TransportLocation = timeSpan,
                LocationType = TransportLocationType.Time,
                ScrollType = timeSpan.TotalSeconds > this.duration ? ScrollResponseType.Stop : ScrollResponseType.Nop
            };
        }
        #endregion
        #region Bar processing
        private ScrollResponse BarProcessor()
        {
            var timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - this.scrollingStartedAt.Ticks);
            if (this.useMidi)
            {
                return BarWithMidi();
            }
            else
            {
                return Bar();   
            }

            ScrollResponse Bar()
            {
                // 1. Seconds per beat
                double secondsPerBeat = 60.0 / bpm;
                // 2. Seconds per full bar
                double secondsPerBar = secondsPerBeat * bpb;
                // 3. Calculate current bar (adding 1 because we start counting at Bar 1)
                double currentBar = (timeSpan.TotalSeconds / secondsPerBar) + 1.0;
                var lyricBar = this.lyricBuffer.GetLyricBar((int)currentBar);
                Debug.WriteLine($"Seconds: {timeSpan.TotalSeconds} Bpm: {bpm} Bpb: {bpb} Current Bar: {currentBar} Lyric bar: {lyricBar?.Lyric}");
                if (lyricBar != null)
                {
                    return new ScrollResponse()
                    {
                         ScrollLine = this.lyricBuffer.GetIndex(lyricBar),
                         ScrollType = ScrollResponseType.Scroll,
                         LocationType = TransportLocationType.Bar,
                         Bar = lyricBar.Bar
                    };
                }
                else
                {
                    return new ScrollResponse()
                    {
                        TransportLocation = timeSpan,
                        ScrollType = ScrollResponseType.Nop
                    };
                }
            }

            ScrollResponse BarWithMidi()
            {
                var midiTransportLocation = this.lyricMidiService.GetTransportLocation();
                var barLineTarget = this.lyricBuffer.GetLyricBar(midiTransportLocation.BarBeatTime);
                if (barLineTarget != null)
                {
                    var barLyricLine = this.lyricBuffer.GetIndex(barLineTarget);
                    return new ScrollResponse()
                    {
                         ScrollLine = barLyricLine,
                         ScrollType= ScrollResponseType.Scroll,
                         LocationType = TransportLocationType.Bar,
                         Bar = midiTransportLocation.BarBeatTime
                    };
                }
                else
                {
                    return new ScrollResponse()
                    {
                        TransportLocation = timeSpan,
                        ScrollType = ScrollResponseType.Nop
                    };
                }
            }
        }
#endregion
#endregion
    }
}