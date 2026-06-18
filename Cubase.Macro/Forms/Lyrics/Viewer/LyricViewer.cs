using Cubase.Macro.Common.Lyrics;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using static Cubase.Macro.Services.Mouse.NativeMouse;
using System.ComponentModel;
using System.Diagnostics;

namespace Cubase.Macro.Forms.Lyrics.Viewer
{
    public class LyricViewer : BaseRichEdit
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<TimeSpan> ScrollUpdateEvent { get; set; }
        
        private string[] sourceCode;

        private LyricChordCollection lyricCollection;

        private ViewerContent view = new ViewerContent();

        private double totalDuration = -1;

        private DateTime autoScrollStarted;

        private System.Windows.Forms.Timer autoScrollTimer;

        public LyricViewer() : base()
        {
            this.ReadOnly = true;
        }

        public void StartAutoScroll()
        {
            if (this.lyricCollection != null)
            {
                var duration = this.lyricCollection.GetControlValue(ControlLyricKeyword.Duration);
                if (duration != null)
                {
                    totalDuration = duration.GetTimeSeconds();
                    StartScrollTimer();
                }
                else
                {
                    // do based on midi 
                }
            }
        }

        public void StartScrollTimer()
        {
            this.autoScrollTimer = new System.Windows.Forms.Timer();
            this.autoScrollTimer.Interval = 500;
            this.autoScrollTimer.Tick += AutoScrollTimer_Tick; 
            this.autoScrollStarted = DateTime.Now;
            this.autoScrollTimer.Start();
        }

        private void AutoScrollTimer_Tick(object? sender, EventArgs e)
        {
            var timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - autoScrollStarted.Ticks);
            ScrollUpdateEvent.Invoke(timeSpan);
            // now get all defined times for verses ...
            var allTimers = this.view.GetDefinedTimers();
            foreach (var timer in allTimers)
            {
                var timerTimespan = TimeSpan.FromSeconds(timer.TimeLine);
                // if this d_time is is five seconds away - make sure it is scrolled and visible
                if (TimeSpan.FromSeconds(timerTimespan.TotalSeconds-timeSpan.TotalSeconds).TotalSeconds < 5)
                {
                    Debug.WriteLine(timer.LineIndex);
                }
                // if ( timeSpan.Ticks - )
            }
        }

        public void Initialise(string fileName)
        {
            if (File.Exists(fileName))
            {
                this.Initialise(File.ReadAllLines(fileName));
            }
        }

        public void Initialise(string[] sourceCode)
        {
            this.Clear(); // Clear existing content
            this.view.Clear();
            this.sourceCode = sourceCode;
            this.lyricCollection = LyricChordParser.FromLines(sourceCode);

            var lyricBuffer = new LyricBuffer();
            foreach (var line in lyricCollection)
            {
                if (line.HaveControls())
                {
                    ViewModel currentView = null;
                    foreach (var cntrl in line.Controls)
                    {
                        switch (cntrl.Key)
                        {
                            case ControlLyricKeyword.Title:
                                currentView = AppendToView(lyricBuffer, cntrl.Value, () => Color.Cyan);
                                lyricBuffer.AddBlank(1);
                                break;
                            case ControlLyricKeyword.Sov:    
                            case ControlLyricKeyword.Start_Of_Verse:
                                currentView = AppendToView(lyricBuffer, cntrl.Value, () => Color.Yellow);
                                break;
                            case ControlLyricKeyword.Eov:
                            case ControlLyricKeyword.End_Of_Verse:
                                lyricBuffer.AddBlank(2);
                                break;
                            case ControlLyricKeyword.D_Time:
                                if (currentView == null) 
                                {
                                    currentView = AppendToView(lyricBuffer, string.Empty, () => DarkTheme.BackColor);
                                }
                                currentView.TimeLine = cntrl.Value.GetTimeSeconds();
                                break;
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
                    AppendToView(lyricBuffer, cb.ToString(), () => Color.IndianRed);

                }
                if (line.Content.HaveLyric())
                {
                    AppendToView(lyricBuffer, line.Content.Lyric, null);
                }
            }

            this.Text = lyricBuffer.ToText();
            this.ColourIze();
        }

        private ViewModel AppendToView(List<string> lines, string line, Func<Color>? foreGroundColourCallback)
        {
            lines.Add(line);
            var viewModel = new ViewModel()
            {
                ForeColour = foreGroundColourCallback?.Invoke() ?? DarkTheme.TextColor,
                LineIndex = lines.Count - 1
            };
            view.Add(viewModel);
            return viewModel;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.ColourIze();
        }

        private void ColourIze()
        {
            if (!this.IsHandleCreated)
            {
                return;
            }
            
            // Save scroll position
            POINT scrollPoint = new POINT();
            SendMessage(this.Handle, EM_GETSCROLLPOS, IntPtr.Zero, ref scrollPoint);

            // Stop redraw
            SendMessage(this.Handle, WM_SETREDRAW, false, IntPtr.Zero);

            for (int lineIndex = 0; lineIndex < this.Lines.Length; lineIndex++)
            {
                string line = this.Lines[lineIndex];

                int lineStart = this.GetFirstCharIndexFromLine(lineIndex);

                if (lineStart < 0)
                {
                    continue;
                }

                if (line.Length > 0)
                {
                    // colour for whole line
                    this.Select(lineStart, line.Length);
                    this.SelectionColor = view.GetForColour(lineIndex);
                }
            }

            // Restore scroll position 
            SendMessage(this.Handle, EM_SETSCROLLPOS, IntPtr.Zero, ref scrollPoint);

            // Re-enable redraw
            SendMessage(this.Handle, WM_SETREDRAW, true, IntPtr.Zero);

            this.Invalidate();

        }
    }


    public class ViewerContent : List<ViewModel>
    {
        public ViewModel? GetIndex(int lineIndex)
        {
            return this.FirstOrDefault(x => x.LineIndex == lineIndex);
        }


        public List<ViewModel> GetDefinedTimers()
        {
            return this.Where(x => x.TimeLine > -1).ToList();   
        }

        public Color GetForColour(int lineIndex)
        {
            if (this.Any(x => x.LineIndex == lineIndex))
            {
                return this.First(x => x.LineIndex == lineIndex).ForeColour;
            }
            return DarkTheme.TextColor;
        }
    }

    public class ViewModel
    {
        public int LineIndex { get; set; }

        public Color ForeColour { get; set; } = Color.White;

        public Color BackgroundColour { get; set; }

        public double TimeLine { get; set; } = -1;
    }
}


