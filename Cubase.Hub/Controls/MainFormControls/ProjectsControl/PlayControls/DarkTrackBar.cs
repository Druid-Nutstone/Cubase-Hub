using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls
{
    public class DarkTrackBar : Control
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Minimum { get; set; } = 0;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Maximum { get; set; } = 100;

        private int _value = 0;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Clamp(value, Minimum, Maximum);
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string LabelText { get; set; } = "Volume";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color TextColor { get; set; } = Color.White;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font LabelFont { get; set; } =
            new Font("Segoe UI", 8f, FontStyle.Regular);

        public event EventHandler ValueChanged;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BarBackColor { get; set; } = Color.FromArgb(40, 40, 40);
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BarFillColor { get; set; } = Color.FromArgb(0, 160, 255);

        public DarkTrackBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.Opaque,
                true);
            DoubleBuffered = true;
            Height = 14;
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            /*
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float percent = (float)(Value - Minimum) / (Maximum - Minimum);
            int fillWidth = (int)(Width * percent);

            using var backBrush = new SolidBrush(BarBackColor);
            using var fillBrush = new SolidBrush(BarFillColor);

            e.Graphics.FillRectangle(backBrush, 0, 0, Width, Height);
            e.Graphics.FillRectangle(fillBrush, 0, 0, fillWidth, Height);
            */
            var rect = ClientRectangle;

            using var backBrush = new SolidBrush(BarBackColor);
            using var fillBrush = new SolidBrush(BarFillColor);
            using var textBrush = new SolidBrush(TextColor);

            e.Graphics.FillRectangle(backBrush, rect);

            float percent = (float)(Value - Minimum) / (Maximum - Minimum);
            int fillWidth = (int)(rect.Width * percent);

            e.Graphics.FillRectangle(fillBrush, 0, 0, fillWidth, rect.Height);

            // Draw text centered
            var textRect = rect;

            using var sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            e.Graphics.DrawString(LabelText, LabelFont, textBrush, textRect, sf);
        }
        protected override void OnMouseDown(MouseEventArgs e) => SetValueFromMouse(e.X);
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                SetValueFromMouse(e.X);
        }

        private void SetValueFromMouse(int mouseX)
        {
            float percent = Math.Clamp(mouseX / (float)Width, 0f, 1f);
            Value = Minimum + (int)((Maximum - Minimum) * percent);
        }
    }

}
