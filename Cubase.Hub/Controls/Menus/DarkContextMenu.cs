using Cubase.Hub.Controls.MainFormControls.ProjectsControl.Menu;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cubase.Hub.Controls.Menus
{
    public class DarkContextMenu : ContextMenuStrip
    {
        public bool IsOpen { get; private set; } = false;

        public DarkContextMenu() : base()
        {
            this.Padding = new Padding(5);
            this.ShowImageMargin = false;
            this.ShowCheckMargin = false;
            this.Renderer = new DarkCenteredMenuRenderer();
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);
            this.IsOpen = true;
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            this.IsOpen = true;
        }

        protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            base.OnClosing(e);
            this.IsOpen = false;
        }

    }

    public class DarkCenteredMenuRenderer : ToolStripProfessionalRenderer
    {
        private Color _backColor = Color.FromArgb(35, 35, 35);   // Dark background
        private Color _hoverColor = Color.FromArgb(60, 60, 60);  // Hover / selected
        private Color _textColor = Color.Gainsboro;             // Normal text

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using (var brush = new SolidBrush(_backColor))
            {
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            // Determine text color
            Color color = e.Item.Selected ? Color.White : _textColor;

            var font = new Font(e.TextFont, FontStyle.Bold);

            // Draw text centered in full item rectangle
            var rect = new Rectangle(Point.Empty, e.Item.Size);

            TextRenderer.DrawText(
                e.Graphics,
                e.Text,
                font,
                rect,
                color,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.SingleLine);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            // Draw hover / selection background
            Color back = e.Item.Selected ? _hoverColor : _backColor;

            using (var brush = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
            }
        }
    }
}

