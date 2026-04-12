using Cubase.Macro.Forms.Main.Buttons;
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

        private Action<CubaseMacro, MacroButton> onMacroClicked;

        private Action<CubaseMacro> onBack;

        private MainForm mainForm;

        public MainMenuControl()
        {
            InitializeComponent(); 
            this.menuControl = new MenuControl();
            this.MainPanel.Controls.Add(menuControl);
            ButtonBack.Click += ButtonBack_Click;
            ButtonClose.Click += ButtonClose_Click;
            ButtonPositionCubase.Click += ButtonPositionCubase_Click;
        }

        public void SetColours()
        {
            ButtonClose.BackColor = Color.IndianRed;

             
        }

        private void ButtonPositionCubase_Click(object? sender, EventArgs e)
        {
            this.mainForm.PositionCubase();
        }

        private void ButtonClose_Click(object? sender, EventArgs e)
        {
            this.mainForm.Close();
        }

        private void ButtonBack_Click(object? sender, EventArgs e)
        {
            this.onBack?.Invoke(this.menu);
        }

        public void Initialise(CubaseMacro menu, Action<CubaseMacro, MacroButton> onMacroClicked, Action<CubaseMacro> onBack, MainForm mainForm)
        {
            this.menu = menu;
            this.mainForm = mainForm;
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
