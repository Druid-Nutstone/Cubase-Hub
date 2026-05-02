using Cubase.Macro.Forms.Main.Buttons;
using Cubase.Macro.Forms.Main.Menus;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Config;
using Microsoft.Extensions.Logging;
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<string> Logger { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MainForm MainForm { get; set; }

        private bool haveSetSize = false;

        private CubaseMacro menu;

        private MenuControl menuControl;

        private MenuCommonControl menuCommonControl;

        private Action<CubaseMacro, MacroButton> onMacroClicked;

        private Action<CubaseMacro, PictureButton> onBack;

        private MainForm mainForm;

        private IConfigurationService configurationService;

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
            MainPanel.Resize += MenuSizeChanged;
        }

        private void MenuSizeChanged(object? sender, EventArgs e)
        {
            if (this.configurationService != null)
            {
                this.Log($"MenuSizeChanged: Setting menu height to {this.MainPanel.Height}");
                this.configurationService.Configuration.MacroPanelHeight = this.MainPanel.Height;
                if (!this.configurationService.Configuration.Save())
                {
                    MessageBox.Show("Could not save configuration", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SetSize()
        {
            if (!this.haveSetSize)
            {
                this.SetAndSavePanelHeight();
            }
           
        }

        private void SetAndSavePanelHeight()
        {
            if (this.configurationService?.Configuration?.MacroPanelHeight < 0)
            {
                this.Log($"Setting main panel height to default. MacroPanelHeight is {this.configurationService.Configuration.MacroPanelHeight}");
                this.SetDefaultMainPanelHeight();
                this.configurationService.Configuration.MacroPanelHeight = this.MainPanel.Height;
            }
            else
            {
                if (this.configurationService != null)
                {
                    this.Log($"Setting mainpanel height to {this.configurationService.Configuration.MacroPanelHeight}");
                    MainPanel.Height = this.configurationService.Configuration.MacroPanelHeight;
                    this.haveSetSize = true;
                }
                else
                {
                    this.SetDefaultMainPanelHeight();
                }
            }
        }

        private void SetDefaultMainPanelHeight()
        {
            if (this.MainForm?.WindowState != FormWindowState.Minimized)
            {
                var mainPanelHeight = this.Height * 0.7;
                this.Log($"Setting default main menu height to {mainPanelHeight}");
                MainPanel.Height = (int)mainPanelHeight;
            }
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

        private void Log(string msg)
        {
            if (this.Logger != null)
            {
                this.Logger.Invoke(msg);
            }
        }

        public void InitialiseMain(CubaseMacro menu, Action<CubaseMacro, MacroButton> onMacroClicked, Action<CubaseMacro, PictureButton> onBack, MainForm mainForm, IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
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
            menuCommonControl.ClearMacros();
            foreach (var macro in macros)
            {
                this.menuCommonControl.AddMacro(macro, onMacroClicked);
            }
        }
    }
}
