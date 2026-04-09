using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Configuration.KeyEditors
{
    public class MacroKeyCommandListView : ListView
    {
        public MacroKeyCommandListView()
        {
            this.View = View.Details;
            this.DoubleBuffered = true;
            this.FullRowSelect = true;
            this.MultiSelect = false;
            AddHeader("Name");
            AddHeader("Key");
            ThemeApplier.ApplyDarkTheme(this);
        }

        private void AddHeader(string text)
        {
            var header = new ColumnHeader() { Text = text };
            this.Columns.Add(header);
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

        public void Populate(List<CubaseKeyCommand> commands)
        {
            this.Items.Clear();
            foreach (var cmd in commands)
            {
                this.Items.Add(new MacroKeyCommandListViewItem(cmd));
            }
            AutoFit();
        }
    }
    
    public class MacroKeyCommandListViewItem : ListViewItem
    {
        public CubaseKeyCommand Command { get; private set; }
        public MacroKeyCommandListViewItem(CubaseKeyCommand command)
        {
            this.Command = command;
            this.Text = command.Name;
            this.SubItems.Add(command.Key);
        }
    }
}
