using Cubase.Macro.Forms;
using Cubase.Macro.Forms.Main;
using Cubase.Macro.Forms.Main.Buttons;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Keyboard;
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

        public MainForm(IKeyboardService keyboardService)
        {
            InitializeComponent();
            this.keyboardService = keyboardService;
            this.WindowState = FormWindowState.Minimized;
            ThemeApplier.ApplyDarkTheme(this);
            this.macros = CubaseMacroCollection.Load();
            this.mainMenuControl.Initialise(this.macros.First(), MacroClicked, this.OnBackClicked);
        }

        private void OnBackClicked(CubaseMacro currentMacro)
        {
            if (currentMacro.ParentId == null)
            {
                return;
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
                macroButton.SetColours();
                this.CloseWindow();
            }
            else if (macro.MacroType == CubaseMacroType.Menu)
            {
                if (macro.ToggleOnKeys.Count > 0)
                {
                    RunMacro(macro.ToggleOnKeys, macro);
                }
                this.mainMenuControl.Initialise(macro, MacroClicked, this.OnBackClicked);
            }
        }

        private void RunMacro(List<CubaseKeyCommand> macros, CubaseMacro macro)
        {
            HaveError = false;
            bool okToContinue = true;
            this.TopMost = false;
            this.SendToBack();
            Thread.Sleep(100);
            foreach (var command in macros)
            {
                if (okToContinue)
                {
                    okToContinue = this.keyboardService.SendKeyToCubase(command.Key, (err) =>
                    {
                        okToContinue = false;
                        HaveError = true;
                        this.TopMost = true;
                        this.BringToFront();
                        MessageBox.Show(err);
                        return;
                    });
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

            this.TopMost = true;                  // Always on top
            this.Show();                           // Show the form if hidden
            this.BringToFront();                   // Bring to front
            this.Activate();                       // Give focus to form

            this.Focus();
        }
    }
}
