using Cubase.Macro.Forms.Main.Menus;
using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Main
{
    public partial class MainMenuControl : UserControl
    {

        private CubaseMacro menu;

        private MenuControl menuControl;

        private Action<CubaseMacro> onMacroClicked;

        private Action<CubaseMacro> onBack;

        public MainMenuControl()
        {
            InitializeComponent(); 
            this.menuControl = new MenuControl();
            this.MainPanel.Controls.Add(menuControl);
            ButtonBack.Click += ButtonBack_Click;   
        }

        private void ButtonBack_Click(object? sender, EventArgs e)
        {
            this.onBack?.Invoke(this.menu);
        }

        public void Initialise(CubaseMacro menu, Action<CubaseMacro> onMacroClicked, Action<CubaseMacro> onBack)
        {
            this.menu = menu;
            this.onMacroClicked = onMacroClicked;
            this.onBack = onBack;
            menuControl.ClearMacros();
            foreach (var macro in menu.Macros)
            {
                menuControl.AddMacro(macro, onMacroClicked);
            }

        }
    }
}
