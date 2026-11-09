using Cubase.Macro.Models;
using System.ComponentModel;

namespace Cubase.Macro.Forms.Main.Buttons
{
    public class DropdownToggleMenuButton : Button
    {
        private ContextMenuStrip _contextMenu;
        private DropDownToggleItemCollection buttons;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ActiveColor { get; set; } = Color.LightGreen;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color DefaultColor { get; set; } = Color.FromArgb(45, 45, 48);

        public DropdownToggleMenuButton() : base()
        {
            this.Text = "Actions";
            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = DefaultColor;
            this.ForeColor = Color.White;
            this.Font = new Font(this.Font, FontStyle.Bold);
            this.FlatAppearance.BorderSize = 0;
            this.UseVisualStyleBackColor = false;

            _contextMenu = new ContextMenuStrip
            {
                Renderer = new DropDownMenuRenderer(this, DefaultColor, Color.White),
                ShowImageMargin = false,
                ShowCheckMargin = false,
            };

            _contextMenu.Opening += (s, e) =>
            {
                int targetWidth = this.Width;
                _contextMenu.Width = targetWidth;
                _contextMenu.MinimumSize = new Size(targetWidth, 0);

                foreach (ToolStripItem item in _contextMenu.Items)
                {
                    item.Width = targetWidth;
                }
            };
        }

        public ContextMenuStrip Menu => _contextMenu;

        public void Initialise(DropDownToggleItemCollection items)
        {
            this.buttons = items;
            foreach (var item in this.buttons)
            {
                this._contextMenu.Items.Add(new DropDownButtonItem(item, _contextMenu, this));
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            // Force the context menu drop down container to match button width
            _contextMenu.Width = this.Width;

            Point offsetLocation = new Point(0, this.Height);
            _contextMenu.Show(this, offsetLocation);
        }
    }

    public class DropDownMenuRenderer : ToolStripProfessionalRenderer
    {
        private DropdownToggleMenuButton ownerButton;

        private Color defaultBackgroundColour;

        private Color defaultTextColour;

        public DropDownMenuRenderer(DropdownToggleMenuButton button, Color defaultBackGroundColour, Color defaultTextColour) : base()
        {
            this.ownerButton = button;
            this.defaultBackgroundColour = defaultBackGroundColour;
            this.defaultTextColour = defaultTextColour;
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item is DropDownButtonItem)
            {
                int targetWidth = ownerButton != null ? ownerButton.Width : e.Item.Width;
                Rectangle fullWidthRect = new Rectangle(0, 0, targetWidth, e.Item.Height);

                var dropDownItem = (DropDownButtonItem)e.Item;
                var backColour = dropDownItem.isActive
                                 ? (dropDownItem.MenuButton.ActiveBackColour != null ? dropDownItem.MenuButton.ActiveBackColour : defaultBackgroundColour)
                                 : (dropDownItem.MenuButton.InActiveBackColour != null ? dropDownItem.MenuButton.InActiveBackColour : defaultBackgroundColour);
                using (var brush = new SolidBrush(backColour.Value))
                {
                    e.Graphics.FillRectangle(brush, fullWidthRect);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Item is DropDownButtonItem)
            {
                int targetWidth = ownerButton != null ? ownerButton.Width : e.Item.Width;
                var dropDownItem = (DropDownButtonItem)e.Item;
                var textColour = dropDownItem.isActive
                                 ? (dropDownItem.MenuButton.ActiveTextColour != null ? dropDownItem.MenuButton.ActiveTextColour : defaultTextColour)
                                 : (dropDownItem.MenuButton.InActiveTextColour != null ? dropDownItem.MenuButton.InActiveTextColour : defaultTextColour);

                // Define icon area width and text area bounds
                int iconWidth = 28;
                var iconRect = new Rectangle(
                    4,
                    0,
                    iconWidth,
                    e.Item.Height
                );

                var textRect = new Rectangle(
                    iconWidth + 4,
                    0,
                    targetWidth - (iconWidth + 8),
                    e.Item.Height
                );

                // Draw the emoji icon using the GDI TextRenderer with Segoe UI Emoji to prevent square boxes
                using (var emojiFont = new Font("Segoe UI Emoji", e.TextFont.Size))
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        dropDownItem.MenuButton.IconText,
                        emojiFont,
                        iconRect,
                        textColour.Value,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix
                    );
                }

                // Draw the main text label
                string displayText = dropDownItem.isActive
                    ? dropDownItem.MenuButton.ActiveText
                    : (!string.IsNullOrEmpty(dropDownItem.MenuButton.InActiveText) ? dropDownItem.MenuButton.InActiveText : dropDownItem.MenuButton.ActiveText);

                using (var niceFont = new Font(e.TextFont, FontStyle.Bold))
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        displayText,
                        niceFont,
                        textRect,
                        textColour.Value,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix
                    );
                }
            }
            else
            {
                base.OnRenderItemText(e);
            }
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            if (!(e.Item is DropDownButtonItem))
            {
                base.OnRenderItemCheck(e);
            }
        }
    }

    public class DropDownButtonItem : ToolStripMenuItem
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DropDownToggleItem MenuButton { get; set; }

        private ContextMenuStrip menuStrip;
        private DropdownToggleMenuButton ownerButton;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool isActive { get; set; } = false;

        public DropDownButtonItem(DropDownToggleItem menuButton, ContextMenuStrip parent, DropdownToggleMenuButton ownerButton)
        {
            this.MenuButton = menuButton;
            this.menuStrip = parent;
            this.ownerButton = ownerButton;

            // Remove item internal padding that shrinks the content area
            this.Padding = new Padding(0);

            if (menuButton.SubItems != null)
            {
                foreach (var subButton in menuButton.SubItems)
                {
                    this.DropDownItems.Add(new DropDownButtonItem(subButton, parent, ownerButton));
                }
            }
        }

        public override Size GetPreferredSize(Size constrainingSize)
        {
            Size baseSize = base.GetPreferredSize(constrainingSize);
            int targetWidth = ownerButton != null ? ownerButton.Width : baseSize.Width;

            // Explicitly force full width on preferred layout sizing
            return new Size(targetWidth, Math.Max(baseSize.Height, 48));
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (menuStrip != null)
            {
                // Change to Hand cursor (or Cursors.Default, Cursors.AppStarting, etc.)
                menuStrip.Cursor = Cursors.Hand;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (menuStrip != null)
            {
                // Reset back to default arrow cursor when mouse leaves the item
                menuStrip.Cursor = Cursors.Default;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            this.isActive = !this.isActive;
            if (this.MenuButton.SubItems == null)
            {
                this.isActive = this.MenuButton.OnClick.Invoke(this.isActive);
                this.menuStrip.Invalidate();
            }
        }
    }
}