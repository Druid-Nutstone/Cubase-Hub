using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.Album.Manage
{
    public class MixdownControl : TableLayoutPanel
    {
        public MixdownControl()
        {
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ColumnCount = 1;
            RowCount = 0;
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;

            // ✅ THIS IS CRITICAL
            ColumnStyles.Clear();
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            Padding = new Padding(10);
        }

        public void ShowMixes(MixDownCollection mixes)
        {
            this.Controls.Clear();
        }
    }
}