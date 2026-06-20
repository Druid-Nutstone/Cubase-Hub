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
using Cubase.Macro.Common.Lyrics.Scrolling;
using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Services.Midi;

namespace Cubase.Macro.Forms.Lyrics.Viewer
{
    public class LyricViewer : BaseRichEdit
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<TimeSpan> ScrollUpdateEvent { get; set; }
        
        private string[] sourceCode;

        private double totalDuration = -1;

        private DateTime autoScrollStarted;

        private System.Windows.Forms.Timer autoScrollTimer;

        private readonly ILyricService lyricService;

        private readonly IMidiService midiService;

        public LyricViewer(ILyricService lyricService, IMidiService midiService) : base()
        {
            this.ReadOnly = true;
            this.lyricService = lyricService;
            this.midiService = midiService;
        }

        public void StartAutoScroll()
        {
            StartScrollTimer();
        }

        public void StartScrollTimer()
        {
            this.autoScrollTimer = new System.Windows.Forms.Timer();
            this.autoScrollTimer.Interval = 500;
            this.autoScrollTimer.Tick += AutoScrollTimer_Tick; 
            this.autoScrollStarted = DateTime.Now;
            this.lyricService.StartScrolling();
            this.autoScrollTimer.Start();
        }

        private void AutoScrollTimer_Tick(object? sender, EventArgs e)
        {
            
            var timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - autoScrollStarted.Ticks);
            ScrollUpdateEvent.Invoke(timeSpan);

            var scrollResponse = this.lyricService.Scroll();

            if (scrollResponse != null) 
            { 
                if (scrollResponse.ShouldStop)
                {
                    this.autoScrollTimer.Stop();
                    this.autoScrollTimer.Dispose();
                    return;
                }

                if (scrollResponse.ScrollLine > -1)
                {
                    this.ScrollTolineNumber(scrollResponse.ScrollLine);
                }

            }
        }

        private void ScrollTolineNumber(int lineNumber)
        {
            if (lineNumber < 0 || this.Lines[lineNumber].Length < 2) 
            { 
                return; 
            }

            // 1. Ensure the control has focus so the selection is visible
            this.Focus();

            // 2. Identify the range
            int lineStart = this.GetFirstCharIndexFromLine(lineNumber);
            int lineLength = this.Lines[lineNumber].Length;

            // 3. Scroll to the line first
            var visible = this.GetVisibleLines(); // Assuming this is your custom helper
            if (lineNumber < visible.topLine + 3 || lineNumber > visible.bottomLine - 3)
            {
                this.SelectionStart = lineStart;
                this.ScrollToCaret();
            }

            this.SelectionLength = 0;

            // 4. Apply formatting to the specific line
            this.Select(lineStart, lineLength);

            Debug.WriteLine($"Line number: {lineNumber} Top: {visible.topLine} Bottom: {visible.bottomLine}");
        }

        public (int topLine, int bottomLine) GetVisibleLines()
        {
            // character at the top-left of the control
            System.Drawing.Point topPoint = new System.Drawing.Point(1, 1);
            int topChar = this.GetCharIndexFromPosition(topPoint);


            // character near the bottom-left
            System.Drawing.Point bottomPoint = new System.Drawing.Point(
                1,
                this.ClientSize.Height - 1);

            int bottomChar = this.GetCharIndexFromPosition(bottomPoint);


            int topLine = this.GetLineFromCharIndex(topChar);
            int bottomLine = this.GetLineFromCharIndex(bottomChar);


            return (topLine, bottomLine);
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
            this.sourceCode = sourceCode;
            var lyrics = this.lyricService.ParseLyrics(this.sourceCode);
            this.Text = lyrics.ToText();
            this.ColourIze();
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
                    this.SelectionColor = (Color)this.lyricService.GetTextColour(lineIndex);
                }
            }

            // Restore scroll position 
            SendMessage(this.Handle, EM_SETSCROLLPOS, IntPtr.Zero, ref scrollPoint);

            // Re-enable redraw
            SendMessage(this.Handle, WM_SETREDRAW, true, IntPtr.Zero);

            this.Invalidate();

        }
    }
}


