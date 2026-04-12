using Cubase.Macro.Forms;
using Cubase.Macro.Forms.Main;
using Cubase.Macro.Forms.Main.Buttons;
using Cubase.Macro.Models;
using Cubase.Macro.Services;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Keyboard;
using Cubase.Macro.Services.Mouse;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro
{
    public partial class MainForm : BaseWindows11Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action ActionComplete { get; set; } = () => { };

        public bool HaveError { get; private set; }

        private CubaseMacroCollection macros;

        private readonly IKeyboardService keyboardService;

        private readonly IConfigurationService configurationService;

        private readonly IServiceProvider serviceProvider;

        private readonly ILogger<MainForm> logger;
        public MainForm(IKeyboardService keyboardService, 
                        IConfigurationService configurationService,
                        ILogger<MainForm> log,
                        IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.keyboardService = keyboardService;
            this.configurationService = configurationService;
            this.serviceProvider = serviceProvider;
            this.logger = log;
            StaticConfig.Instance.SetConfiguration(this.configurationService.Configuration);
            this.WindowState = FormWindowState.Minimized;
            ThemeApplier.ApplyDarkTheme(this);
            this.macros = CubaseMacroCollection.Load();
            if (this.macros.Count > 0)
            {
                this.mainMenuControl.Initialise(this.macros.First(), MacroClicked, this.OnBackClicked);
            }
            else
            {
                MessageBox.Show("No macros configured. Please configure macros (Right-click and Open Settings) before using.");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            this.serviceProvider.GetService<IMouseService>()?.Dispose();
        }

        private void OnBackClicked(CubaseMacro currentMacro)
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
                        this.TopMost = true;
                        this.BringToFront();
                    }
                }
                else 
                { 
                   MessageBox.Show("No Reset Visibility Key configured. Please set a Reset Visibility Key in settings to ensure the menu is visible after navigating back.");
                }
            }
            var parentMenu = this.macros.FindParentIdRecursive(this.macros.First(), currentMacro.ParentId.Value);
            this.mainMenuControl.Initialise(parentMenu, MacroClicked, this.OnBackClicked);
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
                        RunMacro(macro.ToggleOnKeys, macro);
                    }
                    else
                    {
                        macro.ToggleState = CubaseMacroToggleState.Off;
                        RunMacro(macro.ToggleOffKeys, macro);
                    }
                }
                else
                {
                    RunMacro(macro.ToggleOnKeys, macro);
                }
                macroButton.SetColoursAndTitle();
                this.CloseWindow();
            }
            else if (macro.MacroType == CubaseMacroType.Menu)
            {
                if (macro.ToggleOnKeys.Count > 0)
                {
                    RunMacro(macro.ToggleOnKeys, macro);
                    ToFront();
                }
                this.mainMenuControl.Initialise(macro, MacroClicked, this.OnBackClicked);
            }
        }

        private void ToFront()
        {
            this.TopMost = true;                  // Always on top
            this.Show();                           // Show the form if hidden
            this.BringToFront();                   // Bring to front
            this.Activate();                       // Give focus to form
            this.Focus();
        }

        private void ToBack()
        {
            this.TopMost = false;
            this.SendToBack();
        }

        private void RunMacro(List<CubaseKeyCommand> macros, CubaseMacro macro)
        {
            HaveError = false;
            bool okToContinue = true;
            ToBack();   
            Thread.Sleep(100);
            foreach (var command in macros)
            {
                if (okToContinue)
                {
                    this.logger.LogInformation("Executing command {CommandName} with key {CommandKey}", command.Name, command.Key);
                    okToContinue = this.keyboardService.SendKeyToCubase(command.Key, (err) =>
                    {
                        okToContinue = false;
                        HaveError = true;
                        this.TopMost = true;
                        this.BringToFront();
                        MessageBox.Show(err);
                        return;
                    });
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
                this.OnBackClicked(macro);
            }
        }

        public void CloseWindow()
        {
            this.TopMost = false;
            this.WindowState = FormWindowState.Minimized;
            if (this.ActionComplete != null)
            {
                this.ActionComplete.Invoke();
            }
        }

        public void ShowMacros()
        {

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            // Use WorkingArea to avoid taskbar overlap
            // Use Bounds if you WANT to cover taskbar

            this.SuspendLayout();

            this.StartPosition = FormStartPosition.Manual;
            // this.FormBorderStyle = FormBorderStyle.None; // optional (for panel look)

            this.Location = new Point(screen.Left, screen.Top);
            this.Size = new Size(this.Width, screen.Height);

            this.ResumeLayout();

            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            ToFront();
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ActionComplete?.Invoke();
            }
            base.OnResize(e);
        }

    }
}
