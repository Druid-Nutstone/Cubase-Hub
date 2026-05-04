using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Main.Buttons;
using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Main.Menus
{
    public class MenuCommonControl : TableLayoutPanel
    {
        private int _columns = 2; // number of buttons per row

        public MenuCommonControl()
        {
            Dock = DockStyle.Top;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink; 
            GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            DoubleBuffered = true;
            Padding = new Padding(10);

            SetupColumns();
        }

        public void ClearMacros()
        {
            this.Controls.Clear();
            this.RowStyles.Clear();
            this.RowCount = 0;
            this.SetupColumns(); 
        }

        private void SetupColumns()
        {
            ColumnCount = _columns;
            ColumnStyles.Clear();

            for (int i = 0; i < _columns; i++)
            {
                ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 100f / _columns));
            }
        }

        public void AddMacro(CubaseMacro macro, Action<CubaseMacro, MacroButton> onMacroClicked)
        {
            var button = new MacroCommonButton(macro, onMacroClicked);

            int index = Controls.Count;
            int row = index / _columns;
            int col = index % _columns;

            if (row >= RowCount)
            {
                RowCount++;
                RowStyles.Add(new RowStyle(SizeType.Absolute, button.Height)); // 60px height
            }

            Controls.Add(button, col, row);
        }
    }
}
