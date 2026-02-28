namespace Cubase.Hub.Controls.DarkPanel
{

    using System;
    using System.Drawing;
    using System.Windows.Forms;

    [Obsolete("Does not work")]
    public class DarkScrollablePanel : Panel
    {
        private const int ScrollBarWidth = 12;

        private int scrollValue;
        private bool dragging;
        private int dragOffset;

        public DarkScrollablePanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            DoubleBuffered = true;
            AutoScroll = false;
            BackColor = Color.FromArgb(37, 37, 38);
        }

        private Control Content =>
            Controls.Count > 0 ? Controls[0] : null;

        private int ContentHeight =>
            Content?.Height ?? 0;

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (Controls.Count > 1)
                throw new InvalidOperationException(
                    "Only one child control supported.");

            e.Control.Dock = DockStyle.Top;
            e.Control.Width = Width - ScrollBarWidth;
            e.Control.Location = new Point(0, 0);

            e.Control.SizeChanged += (s, _) =>
            {
                Invalidate();
            };
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (Content != null)
                Content.Width = Width - ScrollBarWidth;

            ClampScroll();
            UpdateScrollPosition();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            scrollValue -= e.Delta / 2;
            ClampScroll();
            UpdateScrollPosition();
        }

        private void ClampScroll()
        {
            int max = Math.Max(0, ContentHeight - Height);
            scrollValue = Math.Max(0, Math.Min(max, scrollValue));
        }

        private void UpdateScrollPosition()
        {
            if (Content != null)
                Content.Top = -scrollValue;

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (ContentHeight <= Height)
                return;

            var g = e.Graphics;

            // Scrollbar track
            Rectangle track = new Rectangle(
                Width - ScrollBarWidth,
                0,
                ScrollBarWidth,
                Height);

            using (Brush b = new SolidBrush(Color.FromArgb(45, 45, 48)))
                g.FillRectangle(b, track);

            // Thumb size
            float visibleRatio = (float)Height / ContentHeight;
            int thumbHeight = Math.Max(30, (int)(Height * visibleRatio));

            float scrollRatio =
                (float)scrollValue / (ContentHeight - Height);

            int thumbY =
                (int)((Height - thumbHeight) * scrollRatio);

            Rectangle thumb = new Rectangle(
                Width - ScrollBarWidth + 2,
                thumbY,
                ScrollBarWidth - 4,
                thumbHeight);

            using (Brush b = new SolidBrush(Color.FromArgb(90, 90, 95)))
                g.FillRectangle(b, thumb);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Rectangle thumb = GetThumbRect();

            if (thumb.Contains(e.Location))
            {
                dragging = true;
                dragOffset = e.Y - thumb.Y;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!dragging)
                return;

            int thumbHeight = GetThumbRect().Height;
            int trackHeight = Height - thumbHeight;

            int newThumbY = e.Y - dragOffset;
            newThumbY = Math.Max(0, Math.Min(trackHeight, newThumbY));

            float percent = (float)newThumbY / trackHeight;

            scrollValue =
                (int)((ContentHeight - Height) * percent);

            ClampScroll();
            UpdateScrollPosition();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            dragging = false;
        }

        private Rectangle GetThumbRect()
        {
            if (ContentHeight <= Height)
                return Rectangle.Empty;

            float visibleRatio = (float)Height / ContentHeight;
            int thumbHeight = Math.Max(30, (int)(Height * visibleRatio));

            float scrollRatio =
                (float)scrollValue / (ContentHeight - Height);

            int thumbY =
                (int)((Height - thumbHeight) * scrollRatio);

            return new Rectangle(
                Width - ScrollBarWidth + 2,
                thumbY,
                ScrollBarWidth - 4,
                thumbHeight);
        }
    }

}