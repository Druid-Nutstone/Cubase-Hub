using Cubase.Macro.Forms;
using Cubase.Macro.Forms.Main;
using Cubase.Macro.Forms.Main.Buttons;
using Cubase.Macro.Models;
using Cubase.Macro.Services;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Keyboard;
using Cubase.Macro.Services.Midi;
using Cubase.Macro.Services.Monitor;
using Cubase.Macro.Services.Mouse;
using Cubase.Macro.Services.Window;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace Cubase.Macro
{
    public partial class MainForm : BaseWindows11Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action ActionComplete { get; set; } = () => { };

        private enum CubaseState
        {
            WaitingForActive,
            WaitingForClosed
        }

        private CubaseState _state = CubaseState.WaitingForActive;
        
        private System.Windows.Forms.Timer _timer;

        private System.Windows.Forms.Timer _mouseWatcherTimer;

        public bool HaveError { get; private set; }

        private CubaseMacroCollection macros;

        private readonly IKeyboardService keyboardService;

        private readonly IConfigurationService configurationService;

        private readonly IServiceProvider serviceProvider;

        private readonly IWindowService windowService;

        private readonly IMidiService midiService;

        private readonly ILogger<MainForm> logger;

        public MainForm(IKeyboardService keyboardService, 
                        IConfigurationService configurationService,
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
            this.logger = log;
            StaticConfig.Instance.SetConfiguration(this.configurationService.Configuration);
            ThemeApplier.ApplyDarkTheme(this);
            LoadMacros();
            this.ShowMacros();
        }

        private void LoadMacros()
        {
            this.macros = CubaseMacroCollection.Load();
            if (this.macros.Count > 0)
            {
                this.mainMenuControl.Initialise(this.macros.First(), MacroClicked, this.OnBackClicked, this);
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
                        RunMacro([CubaseKeyCommand.CreateFromKey(this.configurationService.Configuration.ResetVisibilityKey)], currentMacro);
                    }
                }
                else 
                { 
                   MessageBox.Show("No Reset Visibility Key configured. Please set a Reset Visibility Key in settings to ensure the menu is visible after navigating back.");
                }
            }
            var parentMenu = this.macros.FindParentIdRecursive(this.macros.First(), currentMacro.ParentId.Value);
            this.mainMenuControl.Initialise(parentMenu, MacroClicked, this.OnBackClicked, this);
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
                this.mainMenuControl.Initialise(macro, MacroClicked, this.OnBackClicked, this);
                macroButton.SetDefaultCursor();
                this.windowService.BringCubaseToFront();
            }
        }

        //private void StartCubaseWatcher()
        //{
        //    cubaseWatcher = new System.Windows.Forms.Timer();
        //    cubaseWatcher.Interval = 50;
        //    bool newProjectLoaded = false;
        //    cubaseWatcher.Tick += (s, e) =>
        //    {

        //        bool isRunning = this.windowService.IsCubaseRunning();
        //        bool isActive = this.windowService.IsCubaseMainWindowActive();

        //        if (isRunning && isActive)
        //        {
        //            if (!userForcedMinimised)
        //            {
        //                if (this.WindowState == FormWindowState.Minimized)
        //                {
        //                    if (!windowService.IsCubasePositioned(this.Width))
        //                    {
        //                        this.WindowState = FormWindowState.Normal;
        //                    }
        //                }
        //            }
        //            /*
        //            if (newProjectLoaded)
        //            {
        //                this.logger.LogInformation("New Cubase project loaded. Restarting MIDI service to ensure MIDI macros work correctly.");
        //                this.midiService.RestartMidi();
                        
        //                newProjectLoaded = false;
        //            }
        //            */
        //        }
        //        else
        //        {
        //            if (!isActive)
        //            {
        //                newProjectLoaded = true;
        //            }
        //        }
        //    };

        //    cubaseWatcher.Start();
        //}

        public void Minimise()         {

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
            base.OnFormClosing(e);
            // this.MaximiseCubase();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.Activate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Point mousePos = System.Windows.Forms.Cursor.Position;

            if (!insideForm())
            {
                this.windowService.BringCubaseToFront();
            }

            bool insideForm()
            {
                return mousePos.X >= this.Bounds.Left &&
                       mousePos.X <= this.Bounds.Right &&
                       mousePos.Y >= this.Bounds.Top &&
                       mousePos.Y <= this.Bounds.Bottom;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            if (this.windowService != null)
            {
                switch (this.WindowState)
                {
                    case FormWindowState.Normal:
                        if (this.windowService.IsCubaseMainWindowActive())
                        {
                            this.PositionCubase();
                        }
                        break;
                }
            }
            
        }

        private void RunMidiMacro(CubaseKeyCommand command)
        {
            this.midiService.SendMidiMessage(command);
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
                        macro = this.macros.FindParentIdRecursive(this.macros.First(), macro.ParentId.Value);
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
            this.Size = new Size(this.Width, screen.Height);

            this.ResumeLayout();
        }

    }
}
