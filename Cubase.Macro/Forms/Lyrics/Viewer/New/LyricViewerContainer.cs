using Cubase.Macro.Common.Models.Lyrics;
using Cubase.Macro.Forms.Lyrics.Editor.New;
using System.Diagnostics;

namespace Cubase.Macro.Forms.Lyrics.Viewer.New
{
    public class LyricViewerContainer : TableLayoutPanel, ILyricEditor
    {
        private LyricViewer lyricViewer;
        private LyricContainer lyricContainer;
        private SectionHeaderviewer? currentlyScrolledControl;
        private CancellationTokenSource? _scrollCts;

        public LyricViewerContainer() : base()
        {
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;
            this.lyricViewer = new LyricViewer();
            this.Controls.Add(this.lyricViewer);
            this.Margin = new Padding(0);   // Strips external margins around the viewer
            this.Padding = new Padding(0);
        }

        public void Initialise(LyricContainer lyricContainer)
        {
            this.lyricContainer = lyricContainer;
            this.lyricViewer.AddSongTitle(lyricContainer);
            this.lyricViewer.AddLyricSections(lyricContainer);
        }

        public async void GotoBar(int barNumber)
        {
            Control? targetControl = FindControlByBar(this.lyricViewer, barNumber);
            if (targetControl != null)
            {
                if (this.currentlyScrolledControl != null)
                {
                    this.currentlyScrolledControl.SetPlayState(false);
                }

                if (targetControl is SectionHeaderviewer headerViewer)
                {
                    this.currentlyScrolledControl = headerViewer;
                    headerViewer.SetPlayState(true);
                }

                // 1. Calculate target control position relative to the container viewable area
                Point clientPt = this.PointToClient(targetControl.PointToScreen(Point.Empty));
                int currentScrollY = -this.AutoScrollPosition.Y;
                int absoluteControlY = clientPt.Y + currentScrollY;

                // 2. Position the control 1/3 down from the top of the container client height
                int targetY = absoluteControlY - (this.ClientSize.Height / 3);
                targetY = Math.Max(0, targetY); // Prevent negative scrolling bounds

                // 3. Cancel any active smooth scroll animation to avoid overlapping transitions
                _scrollCts?.Cancel();
                _scrollCts = new CancellationTokenSource();

                try
                {
                    // 4. Animate the scroll gradually over 250 milliseconds with cubic ease-out
                    await SmoothScrollToAsync(targetY, 250, _scrollCts.Token);
                }
                catch (OperationCanceledException)
                {
                    // Gracefully handles interruption if a new bar request arrives mid-animation
                }
            }
        }

        private async Task SmoothScrollToAsync(int targetY, int durationMs, CancellationToken cancellationToken)
        {
            int startY = -this.AutoScrollPosition.Y;
            int distance = targetY - startY;
            if (distance == 0) return;

            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < durationMs)
            {
                cancellationToken.ThrowIfCancellationRequested();

                float progress = (float)sw.ElapsedMilliseconds / durationMs;
                // Cubic ease-out formula for a smooth deceleration feel
                float easedProgress = 1f - (float)Math.Pow(1f - progress, 3);

                int currentY = startY + (int)(distance * easedProgress);
                this.AutoScrollPosition = new Point(0, currentY);

                await Task.Delay(15, cancellationToken); // ~60fps frame tick
            }

            cancellationToken.ThrowIfCancellationRequested();
            this.AutoScrollPosition = new Point(0, targetY); // Ensure exact final placement
        }

        private Control? FindControlByBar(Control parent, int targetBar)
        {
            foreach (Control child in parent.Controls)
            {
                // Check if this control's Tag matches the target bar number
                if (child.Tag is int bar && bar == targetBar)
                {
                    return child;
                }

                // Recursively search nested child controls (e.g., inside SectionViewer)
                var found = FindControlByBar(child, targetBar);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        public void IncreaseFont()
        {
            this.lyricContainer.FontSize += 1;
            this.RefreshLyrics();
        }

        public void DecreaseFont()
        {
            this.lyricContainer.FontSize -= 1;
            this.RefreshLyrics();
        }

        private void RefreshLyrics()
        {
            this.lyricContainer.SetSectionFontSize();
            this.lyricViewer.RefreshLyrics(this.lyricContainer);
        }

        public void SetFontSize(int fontSize)
        {
            throw new NotImplementedException();
        }
    }
}