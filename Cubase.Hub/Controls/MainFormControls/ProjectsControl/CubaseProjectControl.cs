using Cubase.Hub.Controls.HorizontalLine;
using Cubase.Hub.Controls.MainFormControls.ProjectsForm;
using Cubase.Hub.Forms.BaseForm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl
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
            this.DoubleBuffered = true; 
            // ✅ THIS IS CRITICAL
            ColumnStyles.Clear();
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            Padding = new Padding(10);
        }

        public void ClearProjects()
        {
            this.Controls.Clear();
            this.RowStyles.Clear();
            this.RowCount = 0;
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

            item.ProjectExpanded += (expandedItem) =>
            {
                this.SuspendLayout();
                // Deselect all other items
                foreach (CubaseProjectItemControl control in this.Controls)
                {
                    if (control != expandedItem)
                    {
                        ThemeApplier.ApplyDarkTheme(control, false);
                    }
                }
                this.ResumeLayout();
            };

            item.ProjectMinimized += (minimizedItem) =>
            {
                this.SuspendLayout();
                // Deselect all other items
                foreach (CubaseProjectItemControl control in this.Controls)
                {
                    if (control != minimizedItem)
                    {
                        ThemeApplier.ApplyDarkTheme(control, true);
                    }
                }
                this.ResumeLayout();
            };  

        }

        private void AddHorizontalLine()
        {
            this.RowCount++;

            // Ensure row style (optional, so rows auto-size)
            this.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Add the control to the new row
            this.Controls.Add(new HrControl(), 0, this.RowCount - 1);
        }
    }
}
