using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Main.Buttons;
using Cubase.Macro.Forms.Main.Menus;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Plugins;
using System.ComponentModel;

namespace Cubase.Macro.Forms.Main
{
    public partial class MainMenuControl : UserControl
    {

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<string> Logger { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MainForm MainForm { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IServiceProvider ServiceProvider { get; set; }

        private bool haveSetSize = false;

        private CubaseMacro menu;

        private MenuControl menuControl;

        private MenuCommonControl menuCommonControl;

        private Action<CubaseMacro, MacroButton> onMacroClicked;

        private Action<CubaseMacro, PictureButton> onBack;

        private MainForm mainForm;

        private IConfigurationService configurationService;

        private IPLuginManagerService pluginManagerService;

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
            this.InitialiseActions();

        }

        private bool ToggleTransportMonitoring(bool startStop)
        {
            this.MainForm.ToggleTransportMonitoring(startStop);
            return startStop;
        }

        private bool ButtonLyricsClick(bool isActive)
        {
            this.mainForm.ShowLyrics();
            return false;
        }

        private bool ButtonAIClick(bool isActive)
        {
            this.mainForm.ShowAI();
            return false;
        }

        private bool ButtonCueLevelsClick(bool isActive)
        {
            this.mainForm.ShowCueLevels();
            return false; // always inactive because the form is modal
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
            this.ActionMenuClick(this.ButtonClose, this.mainForm.Minimise);
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

        public void InitialiseMain(CubaseMacro menu,
                                   Action<CubaseMacro, MacroButton> onMacroClicked,
                                   Action<CubaseMacro, PictureButton> onBack,
                                   MainForm mainForm,
                                   IConfigurationService configurationService,
                                   IPLuginManagerService pluginManagerService)
        {
            this.configurationService = configurationService;
            this.pluginManagerService = pluginManagerService;
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

        private void InitialiseActions()
        {
            var actionCollection = new DropDownToggleItemCollection();
            actionCollection.AddToggleButton("Options", "⚙️", (isActive) =>
            {
                this.mainForm.OpenSettings();
                return isActive;
            });
            actionCollection.AddToggleButton("Cue Levels", "🎚️", ButtonCueLevelsClick);
            actionCollection.AddToggleButton("Open Lyrics", "📜", ButtonLyricsClick);
            actionCollection.AddToggleButton("Open AI", "🤖", ButtonAIClick);
            actionCollection.AddToggleButton("Stop Transport", "🔴",
                                             ToggleTransportMonitoring,
                                             "Start Transport",
                                             Color.Red,
                                             Color.White,
                                             Color.Green, Color.Black);
            var albumButtons = actionCollection.AddToggleButton("Albums", "💿", null);
            albumButtons.AddItem("Manage", "📖", (isActive) =>
            {
                this.mainForm.OpenAlbums();
                return isActive;
            });
            albumButtons.AddItem("New", "➕", (isActive) =>
            {
                this.mainForm.NewAlbum();
                return isActive;
            });
            actionCollection.AddToggleButton("New Track", "🎵", (isActive) =>
            {
                this.mainForm.NewTrack();
                return isActive;
            });
            actionCollection.AddToggleButton("Capture Audio", "🔊", (isActive) =>
            {
                this.mainForm.CaptureAudio();
                return isActive;
            });
            this.Actions.Initialise(actionCollection);
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
