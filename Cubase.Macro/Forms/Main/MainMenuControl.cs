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

        private Action<CubaseMacro, PictureButton> onBack;

        private MainForm mainForm;

        public MainMenuControl()
        {
            InitializeComponent(); 
            this.menuControl = new MenuControl();
            this.MainPanel.Controls.Add(menuControl);
            ButtonBack.Click += ButtonBack_Click;
            ButtonBack.HelpText = "Go Back To Previous Menu";
            ButtonClose.Click += ButtonClose_Click;
            ButtonClose.HelpText = "Close Cubase Macro Completely";
            ButtonMinimise.Click += ButtonMinimise_Click;
            ButtonMinimise.HelpText = "Minimise Cubase Macro";
            ButtonPositionCubase.Click += ButtonPositionCubase_Click;
            ButtonPositionCubase.HelpText = "Position Cubase";
            ButtonRefresh.Click += ButtonRefresh_Click;
            ButtonRefresh.HelpText = "Reload Configuration";
        }

        private void ButtonRefresh_Click(object? sender, EventArgs e)
        {
            ActionMenuClick(this.ButtonRefresh, () =>
            {
                if (this.menu.ParentId != null)
                {
                    this.ButtonBack_Click(this, null);
                }
                this.mainForm.ReloadConfiguration();
            });
        }

        private void ButtonMinimise_Click(object? sender, EventArgs e)
        {
            ActionMenuClick(this.ButtonMinimise, this.mainForm.Minimise);
            
        }

        private void ButtonPositionCubase_Click(object? sender, EventArgs e)
        {
            ActionMenuClick(this.ButtonPositionCubase, this.mainForm.PositionCubase);
        }

        private void ActionMenuClick(PictureButton pictureButton, Action action)
        {
            pictureButton.Cursor = Cursors.WaitCursor;
            action.Invoke();
            pictureButton.Cursor= Cursors.Hand;
        }


        private void ButtonClose_Click(object? sender, EventArgs e)
        {
            this.ActionMenuClick(this.ButtonClose, this.mainForm.Close);
        }

        private void ButtonBack_Click(object? sender, EventArgs e)
        {
            this.ActionMenuClick(this.ButtonBack, () => 
            {
                this.onBack?.Invoke(this.menu, this.ButtonBack);
            });
        }

        public void Initialise(CubaseMacro menu, Action<CubaseMacro, MacroButton> onMacroClicked, Action<CubaseMacro, PictureButton> onBack, MainForm mainForm)
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
