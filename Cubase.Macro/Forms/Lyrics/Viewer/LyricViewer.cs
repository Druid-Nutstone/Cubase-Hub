using Cubase.Macro.Common.Lyrics;
using Cubase.Macro.Common.Lyrics.Scrolling;
using Cubase.Macro.Common.Lyrics.Services;
using Cubase.Macro.Services.Midi;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static Cubase.Macro.Services.Mouse.NativeMouse;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cubase.Macro.Forms.Lyrics.Viewer
{
    public class LyricViewer : BaseRichEdit
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<ScrollResponse> ScrollUpdateEvent { get; set; }
        
        private string[] sourceCode;

        private double totalDuration = -1;

        // private DateTime autoScrollStarted;

        private System.Windows.Forms.Timer autoScrollTimer;

        private readonly ILyricService lyricService;

        private LyricBuffer lyricBuffer;

        private int lastHighlightedLine = -1;

        private int linePadding = 2;


        private char indicator = '\u25B6';

        public LyricViewer(ILyricService lyricService) : base()
        {
            this.ReadOnly = true;
            this.lyricService = lyricService;
            this.Font = new Font("Segoe UI", 12);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);  
            if (this.autoScrollTimer != null)
            {
                this.autoScrollTimer.Stop();
                this.autoScrollTimer.Dispose();
            }
        }


        public void StartAutoScroll()
        {
            this.Select(0, 1);
            this.ScrollToCaret();
            this.RefreshContent();
            StartScrollTimer();
        }

        public void EndAutoScroll()
        {
            if (this.autoScrollTimer != null)
            {
                this.autoScrollTimer.Stop();
                this.autoScrollTimer.Dispose();
                this.Select(0, 1);
                this.ScrollToCaret();
            }
        }

        public void StartScrollTimer()
        {
            this.autoScrollTimer = new System.Windows.Forms.Timer();
            this.autoScrollTimer.Interval = 500;
            this.autoScrollTimer.Tick += AutoScrollTimer_Tick; 
            // this.autoScrollStarted = DateTime.Now;
            this.lyricService.StartScrolling();
            this.autoScrollTimer.Start();
        }

        private void AutoScrollTimer_Tick(object? sender, EventArgs e)
        {
            // var timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks - autoScrollStarted.Ticks);
            var scrollResponse = this.lyricService.Scroll();
            ScrollUpdateEvent.Invoke(scrollResponse);
            if (scrollResponse != null) 
            { 
                if (scrollResponse.ScrollType == ScrollResponseType.Stop)
                {
                    this.autoScrollTimer.Stop();
                    this.autoScrollTimer.Dispose();
                    return;
                }

                if (scrollResponse.ScrollLine > -1 && scrollResponse.ScrollType == ScrollResponseType.Scroll)
                {
                    this.ScrollTolineNumber(scrollResponse.ScrollLine);
                }

            }
        }

        private void ScrollTolineNumber(int lineNumber)
        {
            if (lineNumber < 0 || this.Lines[lineNumber].Length < 2 || lineNumber == this.lastHighlightedLine) 
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

            if (lastHighlightedLine > -1)
            {
                this.ApplyHighlight(lastHighlightedLine, this.BackColor);
            }

            this.ApplyHighlight(lineNumber, Color.OrangeRed);

            this.lastHighlightedLine = lineNumber; 

            Debug.WriteLine($"Line number: {lineNumber} Top: {visible.topLine} Bottom: {visible.bottomLine}");
        }

        private void ApplyHighlight(int lineNumber, Color highlightColour)
        {
            int lineStartIndex = this.GetFirstCharIndexFromLine(lineNumber);
            if (lineStartIndex == -1) return; // Line doesn't exist
            // this.Text = this.Text.Remove(lineStartIndex, 1).Insert(lineStartIndex, highLighter.ToString());
            this.Select(lineStartIndex, 1);
            this.SelectionColor = highlightColour;
            this.DeselectAll();
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

        public void Initialise(IEnumerable<string> sourceCode)
        {
            this.Clear(); // Clear existing content
            this.sourceCode = sourceCode.ToArray();
            this.lyricBuffer = this.lyricService.ParseLyrics(this.sourceCode, this.linePadding, indicator);
            this.Text = this.lyricBuffer.ToText();
            this.RefreshContent();
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.RefreshContent();
        }

        protected override void RefreshContent()
        {
            var fontSize = this.lyricService.LyricCollection?.GetControlValue(ControlLyricKeyword.Font_Size);
            if (fontSize != null)
            {
                this.Font = new Font(this.Font.FontFamily, int.Parse(fontSize));
            }

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

                if (line.Length > 0 && line.Trim() != string.Empty)
                {
                    // colour for whole line
                    this.Select(lineStart, line.Length);
                    this.SelectionColor = (Color)this.lyricService.GetTextColour(lineIndex);
                }
                // set padding chars to black 
                this.Select(lineStart, linePadding);
                this.SelectionColor = this.BackColor;
            }

            // Restore scroll position 
            SendMessage(this.Handle, EM_SETSCROLLPOS, IntPtr.Zero, ref scrollPoint);

            // Re-enable redraw
            SendMessage(this.Handle, WM_SETREDRAW, true, IntPtr.Zero);
            this.DeselectAll();
            this.Invalidate();

        }
    }
}


