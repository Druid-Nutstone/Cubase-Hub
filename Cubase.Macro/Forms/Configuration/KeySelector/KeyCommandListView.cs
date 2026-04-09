using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cubase.Macro.Forms.Configuration.KeySelector
{
    public class KeyCommandListView : ListView
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CubaseKeyCommand> OnKeySelected { get; set; }

        public KeyCommandListView() : base()
        {
            this.View = View.Details;
            this.DoubleBuffered = true;
            this.FullRowSelect = true;
            this.MultiSelect = false;
            this.AddHeader("Name");
            this.AddHeader("Key");
            this.AddHeader("Category");
        }

        private void AddHeader(string text)
        {
            var header = new ColumnHeader() { Text = text };
            this.Columns.Add(header);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            if (this.SelectedItems.Count > 0)
            {
                var item = this.SelectedItems[0] as KeyCommandListViewItem;
                if (item != null)
                {
                    OnKeySelected?.Invoke(item.Command);
                }
            }
        }

        private void AutoFit()
        {
            foreach (ColumnHeader column in this.Columns)
            {
                // Measure header width
                this.AutoResizeColumn(column.Index, ColumnHeaderAutoResizeStyle.HeaderSize);
                int headerWidth = column.Width;

                // Measure content width
                this.AutoResizeColumn(column.Index, ColumnHeaderAutoResizeStyle.ColumnContent);
                int contentWidth = column.Width;

                // Pick whichever is larger
                column.Width = Math.Max(headerWidth, contentWidth);
            }
        }

        public void Populate(CubaseKeyCommandCollection commands)
        {
            this.Items.Clear();
            foreach (var command in commands)
            {
                this.Items.Add(new KeyCommandListViewItem(command));
            }

            AutoFit();
        }
    }


    public class KeyCommandListViewItem : ListViewItem
    {
        public CubaseKeyCommand Command { get; private set; }
        public KeyCommandListViewItem(CubaseKeyCommand command) : base(command.Name)
        {
            this.Command = command;
            this.SubItems.Add(command.Key);
            this.SubItems.Add(command.Category);
        }
    }
}
