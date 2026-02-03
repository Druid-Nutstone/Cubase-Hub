using Cubase.Hub.Forms.BaseForm;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.MainFormControls
{
    public class DarkScrollPanel : Panel
    {
        private VScrollBar vScroll;

        public DarkScrollPanel()
        {
            // Panel itself
            DoubleBuffered = true;
            AutoScroll = false; // we will handle scrolling manually
            BackColor = DarkTheme.PanelColor;

            // Vertical scrollbar
            vScroll = new VScrollBar
            {
                Dock = DockStyle.Right,
                Width = 12,
                Minimum = 0,
                Maximum = 100,
                SmallChange = 10,
                LargeChange = 20,
                BackColor = DarkTheme.ControlColor,
                ForeColor = DarkTheme.TextColor,
                Visible = false
            };
            vScroll.Scroll += VScroll_Scroll;
            Controls.Add(vScroll);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control != vScroll)
            {
                e.Control.LocationChanged += Child_LocationChanged;
                UpdateScroll();
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (e.Control != vScroll)
            {
                e.Control.LocationChanged -= Child_LocationChanged;
                UpdateScroll();
            }
        }

        private void Child_LocationChanged(object? sender, EventArgs e)
        {
            UpdateScroll();
        }

        private void VScroll_Scroll(object? sender, ScrollEventArgs e)
        {
            foreach (Control c in Controls)
            {
                if (c != vScroll)
                    c.Top = -vScroll.Value;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateScroll();
        }

        private void UpdateScroll()
        {
            int contentHeight = 0;
            foreach (Control c in Controls)
            {
                if (c == vScroll) continue;
                contentHeight = Math.Max(contentHeight, c.Bottom);
            }

            int visibleHeight = Height;
            if (contentHeight > visibleHeight)
            {
                vScroll.Visible = true;
                vScroll.Maximum = contentHeight - visibleHeight;
                vScroll.LargeChange = visibleHeight;
            }
            else
            {
                vScroll.Visible = false;
                vScroll.Value = 0;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw dark scrollbar track
            if (vScroll.Visible)
            {
                e.Graphics.FillRectangle(
                    new SolidBrush(DarkTheme.ControlColor),
                    vScroll.Bounds
                );
            }
        }
    }
}

