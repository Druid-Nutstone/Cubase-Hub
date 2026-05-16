using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Cues.CueControls
{
    public class CueTrackBar : Control
    {
        private int thumbHeight = 55;
        private int thumbWidth = 42;
        private int topMargin = 10;
        private int bottomMargin = 10;
        private int dragOffsetY;


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<double> OnVolumeChanged { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<double> OnVolumeMoving { get; set; }

        private bool dragging;
        private double volume;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Volume
        {
            get => volume;
            set
            {
                volume = Math.Max(0, Math.Min(1, value));
                Invalidate();
            }
        }

        public CueTrackBar(
            double volume,
            Action<double> onVolumeChanged,
            Action<double> onVolumeMoving)
        {
            OnVolumeChanged = onVolumeChanged;
            OnVolumeMoving = onVolumeMoving;

            DoubleBuffered = true;


            SetStyle(
                ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint,
                true);


            Anchor = AnchorStyles.Top |
                     AnchorStyles.Bottom |
                     AnchorStyles.Left |
                     AnchorStyles.Right;

            Dock = DockStyle.Fill;

            Width = 60;
            Volume = volume;
        }

        public void UpdateVolume(double volume)
        {
            if (Math.Abs(this.volume - volume) < 0.001)
                return;

            Volume = volume;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Prevent thumb being larger than control
            thumbHeight = Math.Min(55, Height / 4);

            // Optional: scale width with height
            thumbWidth = (int)(thumbHeight * 0.75);

            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.Clear(Color.FromArgb(30, 30, 30));

            int usableHeight =
                Height - topMargin - bottomMargin - thumbHeight;

            usableHeight = Math.Max(1, usableHeight);

            // ----------------------------
            // Scale markers
            // ----------------------------

            using var textBrush =
                new SolidBrush(Color.LightGray);

            using var tickPen =
                new Pen(Color.Gray, 1);

            int markerCount = 11;

            for (int i = 0; i < markerCount; i++)
            {
                int value = 100 - (i * 10);

                int markerY =
                    topMargin +
                    (usableHeight * i / (markerCount - 1));

                // Tick mark
                g.DrawLine(
                    tickPen,
                    5,
                    markerY,
                    15,
                    markerY);

                // Text
                g.DrawString(
                    value.ToString(),
                    Font,
                    textBrush,
                    18,
                    markerY - 8);
            }

            // ----------------------------
            // Track
            // ----------------------------

            int trackWidth = 20;
            int trackX = Width / 2 - trackWidth / 2;

            Rectangle trackRect =
                new Rectangle(
                    trackX,
                    topMargin,
                    trackWidth,
                    Height - topMargin - bottomMargin);

            using var trackBrush =
                new SolidBrush(Color.FromArgb(60, 60, 60));

            g.FillRectangle(trackBrush, trackRect);

            // ----------------------------
            // Filled volume section
            // ----------------------------

            int fillHeight =
                (int)((Height - topMargin - bottomMargin)
                * volume);

            int fillY =
                Height - bottomMargin - fillHeight;

            Color levelColor;

            if (volume < .8)
            {
                // Green -> Yellow
                int red = (int)(255 * (volume / .8));

                levelColor =
                    Color.FromArgb(
                        red,
                        255,
                        0);
            }
            else
            {
                // Yellow -> Red
                int green =
                    (int)(255 *
                    (1 - ((volume - .8) / .2)));

                levelColor =
                    Color.FromArgb(
                        255,
                        green,
                        0);
            }

            using var levelBrush =
                new SolidBrush(levelColor);

            g.FillRectangle(
                levelBrush,
                trackX,
                fillY,
                trackWidth,
                fillHeight);

            // Border around track
            using var borderPen =
                new Pen(Color.Black, 2);

            g.DrawRectangle(borderPen, trackRect);

            // ----------------------------
            // Fader thumb
            // ----------------------------

            int y =
                topMargin +
                (int)((1 - volume)
                * usableHeight);

            Rectangle thumbRect =
                new Rectangle(
                    (Width - thumbWidth) / 2,
                    y,
                    thumbWidth,
                    thumbHeight);

            using var thumbBrush =
                new SolidBrush(Color.Silver);

            g.FillRectangle(
                thumbBrush,
                thumbRect);

            g.DrawRectangle(
                borderPen,
                thumbRect);

            // Grip lines

            using var gripPen =
                new Pen(Color.Gray, 2);

            for (int i = 0; i < 5; i++)
            {
                int lineY = y + 10 + (i * 8);

                g.DrawLine(
                    gripPen,
                    thumbRect.Left + 8,
                    lineY,
                    thumbRect.Right - 8,
                    lineY);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int usableHeight =
                Height - topMargin - bottomMargin - thumbHeight;

            usableHeight = Math.Max(1, usableHeight);

            int thumbY =
                topMargin +
                (int)((1 - volume) * usableHeight);

            Rectangle thumbRect =
                new Rectangle(
                    (Width - thumbWidth) / 2,
                    thumbY,
                    thumbWidth,
                    thumbHeight);

            dragging = true;

            if (thumbRect.Contains(e.Location))
            {
                // Store where inside the thumb the user clicked
                dragOffsetY = e.Y - thumbY;
            }
            else
            {
                // Clicking on track: center thumb under mouse
                dragOffsetY = thumbHeight / 2;

                UpdateFromMouse(e.Y - dragOffsetY);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (dragging)
            {
                UpdateFromMouse(e.Y - dragOffsetY);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            dragging = false;

            OnVolumeChanged?.Invoke(volume);

            base.OnMouseUp(e);
        }

        private void UpdateFromMouse(int mouseY)
        {

            int usableHeight =
                Height - topMargin - bottomMargin - thumbHeight;

            usableHeight = Math.Max(1, usableHeight);

            mouseY = Math.Max(
                topMargin,
                Math.Min(
                    topMargin + usableHeight,
                    mouseY));

            double newVolume =
                1.0 -
                ((double)(mouseY - topMargin)
                 / usableHeight);

            volume = Math.Max(
                0,
                Math.Min(1, newVolume));

            OnVolumeMoving?.Invoke(volume);

            Invalidate();
        }
    }
    //public static class DoubleExtensions
    //{
    //    public static int VolumeToInt(this double volume)
    //    {
    //        return (int)(volume * 100);
    //    }

    //    public static double IntToVolume(this int volume)
    //    {
    //        return volume / 100.0;
    //    }
    //}
}