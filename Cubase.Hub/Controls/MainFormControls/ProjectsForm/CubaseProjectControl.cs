using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{
    public class CubaseProjectControl : TableLayoutPanel
    {
        public CubaseProjectControl()
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

        public void AddProjectItem(CubaseProjectItemControl item) 
        {
            // Add a new row
            this.RowCount++;

            // Ensure row style (optional, so rows auto-size)
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Add the control to the new row
            this.Controls.Add(item, 0, this.RowCount - 1);

            // Stretch control to fill horizontally (if desired)
            item.Dock = DockStyle.Fill;

            item.SetParent(this);

        }
    }
}
