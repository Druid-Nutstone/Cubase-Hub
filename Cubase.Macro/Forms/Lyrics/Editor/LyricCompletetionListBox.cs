using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Cubase.Macro.Forms.Lyrics.Editor
{
    public class LyricCompletetionListBox : ListBox
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<LyricControlCommand?> OnSelected { get; set; }

        public LyricCompletetionListBox() : base()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = 40;
            this.BorderStyle = BorderStyle.None;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            using (Brush textBrush = new SolidBrush(DarkTheme.TextColor))
            {
                var txt = ((LyricControlCommand)this.Items[e.Index]).Description; 
                // Draw the text, centered vertically within the item height
                e.Graphics.DrawString(txt, e.Font, textBrush, e.Bounds.X, e.Bounds.Y + 10);
            }
            e.DrawFocusRectangle();
        }

        public void Populate(List<LyricControlCommand> list)
        {
            this.Items.Clear();
            this.Items.AddRange(list.ToArray());
            this.DisplayMember = nameof(LyricControlCommand.Description);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Enter)
            {
                this.OnSelected?.Invoke(GetSelected());
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.OnSelected?.Invoke(null);
                return;
            }
        }


        public void MoveDown()
        {
            this.SelectedIndex = this.SelectedIndex + 1 > this.Items.Count ? this.Items.Count-1 : this.SelectedIndex + 1;
        }

        public void MoveUp()
        {
            this.SelectedIndex = this.SelectedIndex - 1 < 0 ? 0 : this.SelectedIndex - 1;
        }

        public LyricControlCommand GetSelected()
        {
            return this.Items[SelectedIndex] as LyricControlCommand;
        }

    }
}
