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

        private MenuCommonControl menuCommonControl;

        private Action<CubaseMacro, MacroButton> onMacroClicked;

        private Action<CubaseMacro, PictureButton> onBack;

        private MainForm mainForm;

        public MainMenuControl()
        {
            InitializeComponent();
            this.menuControl = new MenuControl();
            this.MainPanel.Controls.Add(menuControl);
            this.menuCommonControl = new MenuCommonControl();
            this.CommonPanel.Controls.Add(menuCommonControl);
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

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            var mainPanelHeight = this.Height * 0.7;
            MainPanel.Height = (int)mainPanelHeight;
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
            pictureButton.Cursor = Cursors.Hand;
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

        public void InitialiseMain(CubaseMacro menu, Action<CubaseMacro, MacroButton> onMacroClicked, Action<CubaseMacro, PictureButton> onBack, MainForm mainForm)
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

        public void InitialiseCommon(List<CubaseMacro> macros, Action<CubaseMacro, MacroButton> onMacroClicked)
        {
            foreach (var macro in macros)
            {
                this.menuCommonControl.AddMacro(macro, onMacroClicked);
            }
            }
    }
}
