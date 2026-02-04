using Cubase.Hub.Forms.BaseForm;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.PlayControls
{
    public class DarkProgressBar : Control
    {
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Minimum
        {
            get => _minimum;
            set
            {
                if (value >= _maximum)
                    throw new ArgumentException("Minimum must be less than Maximum");

                _minimum = value;
                if (_value < _minimum) _value = _minimum;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Maximum
        {
            get => _maximum;
            set
            {
                if (value <= _minimum)
                    throw new ArgumentException("Maximum must be greater than Minimum");

                _maximum = value;
                if (_value > _maximum) _value = _maximum;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Max(_minimum, Math.Min(_maximum, value));
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DisplayText { get; set; } = "";

        public DarkProgressBar()
        {
            DoubleBuffered = true;
            Size = new Size(200, 18);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.Clear(DarkTheme.PanelColor);

            float range = _maximum - _minimum;
            float percent = range > 0
                ? (_value - _minimum) / range
                : 0f;

            int fillWidth = (int)(Width * percent);

            using (var fill = new SolidBrush(DarkTheme.MutedText))
                g.FillRectangle(fill, 0, 0, fillWidth, Height);

            using (var border = new Pen(DarkTheme.BorderColor))
                g.DrawRectangle(border, 0, 0, Width - 1, Height - 1);

            if (!string.IsNullOrEmpty(DisplayText))
            {
                TextRenderer.DrawText(
                    g,
                    DisplayText,
                    Font,
                    ClientRectangle,
                    DarkTheme.TextColor,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPadding
                );
            }
        }
    }


}
