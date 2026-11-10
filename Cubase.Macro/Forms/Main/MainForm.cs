using Cubase.Hub.Forms;
using Cubase.Hub.Forms.Albums;
using Cubase.Hub.Forms.Tracks;
using Cubase.Hub.Services.Models;
using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms;
using Cubase.Macro.Forms.AudioCapture;
using Cubase.Macro.Forms.Configuration;
using Cubase.Macro.Forms.Cues;
using Cubase.Macro.Forms.Lyrics;
using Cubase.Macro.Forms.Main.Buttons;
using Cubase.Macro.Services;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Keyboard;
using Cubase.Macro.Services.Midi;
using Cubase.Macro.Services.Plugins;
using Cubase.Macro.Services.Window;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nutstone.AI.Assistant.Forms.Chat;
using Nutstone.AI.Assistant.Models;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace Cubase.Macro
{
    public partial class MainForm : BaseWindows11Form
    {
        private int boundsTolerence = SystemInformation.BorderSize.Width * 9;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action ActionComplete { get; set; } = () => { };

        private enum CubaseState
        {
            WaitingForActive,
            WaitingForClosed
        }

        public bool HaveError { get; private set; }

        private CubaseMacroCollection macros;

        private readonly IKeyboardService keyboardService;

        private readonly IConfigurationService configurationService;

        private readonly IServiceProvider serviceProvider;

        private readonly IWindowService windowService;

        private readonly IMidiService midiService;

        private readonly IPLuginManagerService pluginManagerService;

        private readonly ILogger<MainForm> logger;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool PositionCubaseWithMouse { get; set; } = true;

        public MainForm(IKeyboardService keyboardService,
                        IConfigurationService configurationService,
                        IPLuginManagerService pluginManagerService,
                        IWindowService windowService,
                        IMidiService midiService,
                        ILogger<MainForm> log,
                        IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.keyboardService = keyboardService;
            this.configurationService = configurationService;
            this.serviceProvider = serviceProvider;
            this.midiService = midiService;
            this.windowService = windowService;
            this.pluginManagerService = pluginManagerService;
            this.logger = log;
            this.mainMenuControl.Logger = this.Log;
            this.mainMenuControl.MainForm = this;
            this.mainMenuControl.ServiceProvider = serviceProvider;
            this.midiService.RegisterForTransportMonitoring(this.HookTransportMonitoring);
            StaticConfig.Instance.SetConfiguration(this.configurationService.Configuration);
            ThemeApplier.ApplyDarkTheme(this);
            LoadMacros();
            this.ShowMacros();
            this.HookMouseMove(this);
        }

        private void HookTransportMonitoring(bool monitoringState)
        {
            // this.mainMenuControl.SetTransportMonitorState(monitoringState);
        }

        private void HookMouseMove(Control baseControl)
        {
            baseControl.MouseLeave += this.MouseAllOut;
            foreach (Control control in baseControl.Controls)
            {
                HookMouseMove(control);
            }
        }

        private void MouseAllOut(object? sender, EventArgs e)
        {

            if (!this.Bounds.Contains(System.Windows.Forms.Cursor.Position))
            {
                this.windowService.BringCubaseToFront();
            }
            else
            {
                this.Activate();
            }
        }

        private void Log(string msg)
        {
            this.logger.LogInformation(msg);
        }

        private void LoadMacros()
        {
            this.macros = CubaseMacroCollection.Load();
            if (this.macros.Macros.Count > 0)
            {
                this.mainMenuControl.InitialiseMain(this.macros.Macros.First(), MacroClicked, this.OnBackClicked, this, this.configurationService, this.pluginManagerService);
                if (this.macros.CommonMacros.Count > 0)
                {
                    this.mainMenuControl.InitialiseCommon(this.macros.CommonMacros, MacroClicked);
                }
            }
            else
            {
                MessageBox.Show("No macros configured. Please configure macros (Right-click and Open Settings) before using.");
            }
        }

        private void OnBackClicked(CubaseMacro currentMacro, PictureButton pictureButton)
        {
            if (currentMacro.ParentId == null)
            {
                return;
            }
            if (currentMacro.MacroType == CubaseMacroType.Menu)
            {
                if (!string.IsNullOrEmpty(this.configurationService.Configuration.ResetVisibilityKey))
                {
                    // if there are any togglebuttons that are active - need to execute the eir toggle off 
                    if (currentMacro.Macros != null)
                    {
                        if (currentMacro.Macros.Any(x => x.ToggleState == CubaseMacroToggleState.On))
                        {
                            currentMacro.Macros.Where(x => x.ToggleState == CubaseMacroToggleState.On).ToList().ForEach(x =>
                            {
                                RunMacro(x.ToggleOffKeys, x);
                                x.ToggleState = CubaseMacroToggleState.Off;
                            });
                        }
                    }
                    if (currentMacro.MenuChangesVisibility)
                    {
                        RunMacro([CubaseKeyCommand.Deserialise(this.configurationService.Configuration.ResetVisibilityKey)], currentMacro);
                    }
                }
                else
                {
                    MessageBox.Show("No Reset Visibility Key configured. Please set a Reset Visibility Key in settings to ensure the menu is visible after navigating back.");
                }
            }
            var parentMenu = this.macros.FindParentIdRecursive(this.macros.Macros.First(), currentMacro.ParentId.Value);
            this.mainMenuControl.InitialiseMain(parentMenu, MacroClicked, this.OnBackClicked, this, this.configurationService, this.pluginManagerService);
            this.windowService.BringCubaseToFront();
        }

        private void MacroClicked(CubaseMacro macro, MacroButton macroButton)
        {
            if (macro.MacroType == CubaseMacroType.KeyCommand)
            {
                if (macro.ButtonType == CubaseMacroButtonType.Toggle)
                {
                    if (macro.ToggleState != CubaseMacroToggleState.On)
                    {
                        macro.ToggleState = CubaseMacroToggleState.On;
                        macroButton.SetBlockCursor();
                        RunMacro(macro.ToggleOnKeys, macro);
                        macroButton.SetDefaultCursor();

                    }
                    else
                    {
                        macro.ToggleState = CubaseMacroToggleState.Off;
                        macroButton.SetBlockCursor();
                        RunMacro(macro.ToggleOffKeys, macro);
                        macroButton.SetDefaultCursor();
                    }
                }
                else
                {
                    RunMacro(macro.ToggleOnKeys, macro);
                }
                macroButton.SetColoursAndTitle();
                this.windowService.BringCubaseToFront();
            }
            else if (macro.MacroType == CubaseMacroType.Menu)
            {
                if (macro.ToggleOnKeys.Count > 0)
                {
                    macroButton.SetBlockCursor();
                    RunMacro(macro.ToggleOnKeys, macro);
                }
                this.mainMenuControl.InitialiseMain(macro, MacroClicked, this.OnBackClicked, this, this.configurationService, this.pluginManagerService);
                macroButton.SetDefaultCursor();
                this.windowService.BringCubaseToFront();
            }
        }

        public void ToggleTransportMonitoring(bool startStop)
        {
            if (startStop)
            {
                this.midiService.StartTransportMonitoring();
            }
            else
            {
                this.midiService.StopTransportMonitoring();
            }
        }

        public void Minimise()
        {

            this.WindowState = FormWindowState.Minimized;
            this.MaximiseCubase();
        }

        public void ReloadConfiguration()
        {
            this.configurationService.ReloadConfiguration();
            StaticConfig.Instance.SetConfiguration(this.configurationService.Configuration);
            LoadMacros();
        }

        private void ToFront()
        {
            this.TopMost = true;
            this.BringToFront();                   // Bring to front
            this.Activate();                       // Give focus to form
        }

        private void ToBack()
        {
            this.TopMost = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.midiService.Dispose();
            // this.MaximiseCubase();
            base.OnFormClosing(e);

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState != FormWindowState.Minimized)
            {
                if (this.PositionCubaseWithMouse)
                {
                    this.mainMenuControl.SetSize();
                    if (this.windowService != null)
                    {
                        if (this.windowService.IsCubaseMainWindowActive())
                        {
                            if (!this.windowService.IsCubasePositioned(this.Width))
                            {
                                this.PositionCubase();
                            }
                        }
                    }
                }
            }
        }


        private void RunMidiMacro(CubaseKeyCommand command)
        {
            var haveMidiResponse = false;
            this.midiService.OnMidiResponse = (midiResponse) =>
            {
                if (midiResponse.Name == command.Name) // maybe should check not and channel ?
                {
                    haveMidiResponse = true;
                    this.logger.LogInformation($"Command {command.Name} has been processed");
                }
            };

            this.midiService.SendMidiMessage(command);

            var count = 0;
            // very naff - but hey ho 
            while (!haveMidiResponse && count <= 100)
            {
                Thread.Sleep(20);
                count++;
            }
            if (count > 100)
            {
                this.logger.LogError($"Did not receive a response for midi command {command.Name}");
                var commandResponse = MessageBox.Show("Cubase midi is not responding. Shall I restart the midi service?", "Midi not responding", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (commandResponse == DialogResult.Yes)
                {
                    this.midiService.RestartMidi();
                }
            }
        }

        private void RunMacro(List<CubaseKeyCommand> macros, CubaseMacro macro)
        {
            HaveError = false;
            bool okToContinue = true;
            ToBack();
            Thread.Sleep(300);

            foreach (var command in macros)
            {
                if (okToContinue)
                {

                    if (command.Category == CubaseMacroConstants.Midi)
                    {
                        this.RunMidiMacro(command);
                    }
                    else
                    {
                        this.logger.LogInformation("Executing command {CommandName} with key {CommandKey}", command.Name, command.Key);
                        okToContinue = this.keyboardService.SendKeyToCubase(command.Key, (err) =>
                        {
                            if (!err.ToLower().Contains("the operation completed"))
                            {

                                okToContinue = false;
                                HaveError = true;
                                this.TopMost = true;
                                this.BringToFront();
                                MessageBox.Show(err);
                                return;
                            }
                        });
                    }
                    if (command.ThreadWaitAfterExecutionMs > 0)
                    {
                        Thread.Sleep(command.ThreadWaitAfterExecutionMs);
                    }
                }
            }
            if (macro.ReturnToParentMenuAfterExecution)
            {
                if (macro.ParentId != null)
                {
                    if (macro.MacroType == CubaseMacroType.KeyCommand)
                    {
                        macro = this.macros.FindParentIdRecursive(this.macros.Macros.First(), macro.ParentId.Value);
                    }
                }
                this.OnBackClicked(macro, null);
            }

        }


        public void PositionCubase()
        {
            if (this.windowService != null)
            {
                this.windowService.PositionCubase(this.Width);
                this.windowService.BringCubaseToFront();
            }
        }

        public void MaximiseCubase()
        {
            if (this.windowService.IsCubaseMainWindowActive())
            {
                this.windowService.MaximiseCubase();
            }
        }

        public void ShowMacros()
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            // Use WorkingArea to avoid taskbar overlap
            // Use Bounds if you WANT to cover taskbar

            this.SuspendLayout();

            this.StartPosition = FormStartPosition.Manual;
            this.FormBorderStyle = FormBorderStyle.None; // optional (for panel look)

            this.Location = new Point(screen.Left, screen.Top);
            this.Size = new Size(this.Width, Screen.PrimaryScreen.WorkingArea.Height);
            // this.WindowState = FormWindowState.Minimized;
            this.ResumeLayout();
        }

        public void ShowCueLevels()
        {
            this.PositionCubaseWithMouse = false;
            this.serviceProvider.GetService<CueForm>().ShowDialog();
            this.PositionCubaseWithMouse = true;
        }

        private Cubase.Hub.Services.Config.IConfigurationService? InitialiseCubaseHubConfiguration()
        {
            var cubaseHubconfig = this.serviceProvider.GetService<Cubase.Hub.Services.Config.IConfigurationService>();
            bool canInitialiseCubaseHub = true;
            cubaseHubconfig?.LoadConfiguration(() =>
            {
                MessageBox.Show("Could not load the Cubase Hub Configuration");
                canInitialiseCubaseHub = false;
            });

            var cubaseExeLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Steinberg", this.configurationService.Configuration.CubaseExecutable, $"{this.configurationService.Configuration.CubaseExecutable}.exe");
            var cubaseUserTemplateLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Steinberg", $"{this.configurationService.Configuration.CubaseExecutable}_64", "Project Templates");
            var cubaseTemplateLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Steinberg", this.configurationService.Configuration.CubaseExecutable, "Project Templates", "Cubase Pro");
            cubaseHubconfig?.Configuration?.CubaseExeLocation = cubaseExeLocation;
            cubaseHubconfig?.Configuration?.CubaseUserTemplateLocation = cubaseUserTemplateLocation;
            cubaseHubconfig?.Configuration?.CubaseTemplateLocation = cubaseTemplateLocation;
            return canInitialiseCubaseHub ? cubaseHubconfig : null;
        }

        public void OpenSettings()
        {
            var settingsForm = this.serviceProvider.GetService<SettingsForm>();
            var windowPos = this.PositionFormNextToMainForm();
            settingsForm.Location = new Point(windowPos.X, windowPos.Y);
            settingsForm.StartPosition = FormStartPosition.Manual;
            settingsForm.ShowDialog();
        }

        public void CaptureAudio()
        {
            var captureAudioFrom = new AudioCaptureForm();
            var windowPos = this.PositionFormNextToMainForm();
            captureAudioFrom.Location = new Point(windowPos.X, windowPos.Y);
            captureAudioFrom.StartPosition = FormStartPosition.Manual;
            captureAudioFrom.ShowDialog();
        }

        public void NewTrack()
        {
            var cubaseHubconfig = this.InitialiseCubaseHubConfiguration();
            if (cubaseHubconfig != null)
            {
                var newTrack = this.serviceProvider.GetService<NewTrackForm>();

                var windowPos = this.PositionFormNextToMainForm();
                newTrack.Location = new Point(windowPos.X, windowPos.Y);
                newTrack.StartPosition = FormStartPosition.Manual;
                newTrack.Initialise();
                newTrack.ShowDialog();
            }
        }

        public void NewAlbum()
        {
            var cubaseHubconfig = this.InitialiseCubaseHubConfiguration();
            if (cubaseHubconfig != null)
            {
                var newAlbumForm = this.serviceProvider.GetService<NewAlbumForm>();

                var windowPos = this.PositionFormNextToMainForm();
                newAlbumForm.Location = new Point(windowPos.X, windowPos.Y);
                newAlbumForm.StartPosition = FormStartPosition.Manual;
                newAlbumForm.ShowDialog();
            }
        }

        public void OpenAlbums()
        {
            var albumForm = this.serviceProvider.GetService<ManageAlbumsForm>();
            var cubaseHubconfig = this.InitialiseCubaseHubConfiguration();
            if (cubaseHubconfig != null)
            {
                cubaseHubconfig?.Configuration?.AlbumWindowLocation = this.PositionFormNextToMainForm();
                albumForm?.Initialise();
                albumForm?.Show();
            }
        }

        private WindowSettings PositionFormNextToMainForm()
        {
            var workingArea = Screen.AllScreens[0].WorkingArea;
            var windowLocation = new Hub.Services.Models.WindowSettings()
            {
                Height = workingArea.Height + boundsTolerence,
                isMaximised = false,
                Width = (workingArea.Width - this.Width) + (boundsTolerence * 2),
                X = this.Width - boundsTolerence,
                Y = 0
            };
            return windowLocation;
        }

        public void ShowAI()
        {
            var aiForm = this.serviceProvider.GetService<ChatForm>();
            var request = this.pluginManagerService.GetChatRequest((err) =>
            {

            });
            if (request != null)
            {
                aiForm?.Initialise(request, this.OnAIActionResult);
            }
            else
            {
                aiForm?.Initialise();
            }
            aiForm?.Show();
        }

        private void OnAIActionResult(ChatResult chatResult)
        {
            this.pluginManagerService.ProcessPluginRequest(chatResult);
        }


        public void ShowLyrics()
        {
            var lyricForm = this.serviceProvider.GetService<LyricViewerForm>();
            var dropBoxDir = CubaseMacroConstants.NutstoneLyricBaseDirectory;
            var projectName = this.windowService.GetCubaseProjectTitle();
            if (!string.IsNullOrEmpty(projectName))
            {
                var realProjectName = $"{projectName}{CubaseMacroConstants.NutstoneLyricNotation}";
                var fullProjectLyricFile = System.IO.Path.Combine(dropBoxDir, realProjectName);
                if (System.IO.File.Exists(fullProjectLyricFile))
                {
                    lyricForm.FileName = fullProjectLyricFile;
                    lyricForm.LoadFile();
                }
            }
            lyricForm.Show();
        }

        public void ReloadScripts()
        {
            this.midiService.ReloadScripts();
        }
    }
}
