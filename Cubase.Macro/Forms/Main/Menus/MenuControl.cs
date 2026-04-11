using Cubase.Macro.Forms.Main.Buttons;
using Cubase.Macro.Models;
using Cubase.Macro.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Main.Menus
{
    public class MenuControl : TableLayoutPanel
    {
        public MenuControl()
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
         
        public void ClearMacros()
        {
            this.Controls.Clear();
            this.RowStyles.Clear();
            this.RowCount = 0;
        }

        public void AddMacro(CubaseMacro macro, Action<CubaseMacro, MacroButton> OnMacroClicked)
        {
            var buttonControl = new MacroButtonPanel(macro, OnMacroClicked);
            this.Controls.Add(buttonControl, 0, this.RowCount);
            //this.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.RowStyles.Add(new RowStyle(SizeType.Absolute, macro.MacroType == CubaseMacroType.Menu ? StaticConfig.Instance.Config.MenuHeight : StaticConfig.Instance.Config.KeyHeight)); // example height
            this.RowCount++;

            // this.RowStyles.Add(new RowStyle(SizeType.Absolute, 10)); // bottom spacing
        }


    }
}
