using Cubase.Macro.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Cubase.Macro.Forms.Lyrics.Editor.New
{
    public class LyricTextEditor : BaseRichEdit
    {
        private LyricSection lyricSection;

        public LyricTextEditor(LyricSection lyricSection) : base()
        {
            this.lyricSection = lyricSection;
            this.BorderStyle = BorderStyle.None;
            this.BackColor = System.Drawing.Color.FromArgb(55, 55, 55);
            this.ForeColor = System.Drawing.Color.White; // Ensure you can see your text on dark theme!

            // 1. DOCK SEPARATION: Do NOT use DockStyle.Fill if you want AutoSize parents to grow.
            // Instead, anchor it to fill the horizontal layout space completely.
            this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // 2. THE EMPTY RESCUE: Give it a baseline size so it's clickable when empty
            // Width will auto-stretch, but Height needs a comfortable 3-line minimum drop target.
            this.MinimumSize = new System.Drawing.Size(100, 80);

            // 3. TEXT EVENT LISTENER: Watch for typing to dynamically recalculate layout heights
            this.ContentsResized += LyricTextEditor_ContentsResized;

            this.Lines = this.lyricSection.GetLinesWithChords();
            this.RefreshContent();
        }

        private void LyricTextEditor_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            // Fix the control height strictly to the size of your rich text block contents.
            // Add a little 10px buffer room so the bottom line doesn't feel cramped.
            int contentHeight = e.NewRectangle.Height + 10;

            // Respect your minimum bounding scale rules
            if (contentHeight < this.MinimumSize.Height)
            {
                this.Height = this.MinimumSize.Height;
            }
            else
            {
                this.Height = contentHeight;
            }
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.lyricSection.AddLyrics(this.Lines);
            this.RefreshContent();
        }

        protected override void RefreshContent()
        {
            int originalSelectionStart = this.SelectionStart;
            int originalSelectionLength = this.SelectionLength;

            // Save scroll position
            POINT scrollPoint = new POINT();
            SendMessage(this.Handle, EM_GETSCROLLPOS, IntPtr.Zero, ref scrollPoint);

            // Stop redraw to prevent screen flickering
            SendMessage(this.Handle, WM_SETREDRAW, false, IntPtr.Zero);

            // 1. Reset the entire text canvas color back to default in one single pass
            this.Select(0, this.TextLength);
            this.SelectionColor = Color.White; // Change to your preferred default text color

            // 2. Scan the global text block. This matches the exact index mapping of this.Select()
            string currentText = this.Text;
            var chordMatches = Regex.Matches(currentText, @"\[[^\]]*\]");

            foreach (Match match in chordMatches)
            {
                this.Select(match.Index, match.Length);
                this.SelectionColor = Color.Red;
            }

            // 3. FIX FOR CARETE BLEED: Explicitly reset the typing attribute color 
            // at the current user cursor insertion point so next typed letters aren't red.
            this.Select(originalSelectionStart, 0);
            this.SelectionColor = Color.White;

            // Restore original caret position/selection width
            this.Select(originalSelectionStart, originalSelectionLength);

            // Restore scroll viewport position
            SendMessage(this.Handle, EM_SETSCROLLPOS, IntPtr.Zero, ref scrollPoint);

            // Re-enable rendering redraw
            SendMessage(this.Handle, WM_SETREDRAW, true, IntPtr.Zero);

            this.Invalidate();
        }



    }
}
