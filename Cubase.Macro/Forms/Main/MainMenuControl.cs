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

        private List<CubaseMacro> macros;

        private MenuControl menuControl;

        public MainMenuControl()
        {
            InitializeComponent(); 
            this.menuControl = new MenuControl();
            this.MainPanel.Controls.Add(menuControl);
        }

        public void Initialise(List<CubaseMacro> macros, Action<CubaseMacro> OnMacroClicked)
        {
            this.macros = macros;
            foreach (var macro in macros)
            {
                menuControl.AddMacro(macro, OnMacroClicked);
            }

        }
    }
}
