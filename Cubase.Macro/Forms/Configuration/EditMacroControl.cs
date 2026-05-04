using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Configuration.KeyEditors;
using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Configuration
{
    public partial class EditMacroControl : UserControl
    {
        private CubaseMacro macro;

        private CubaseMacroCollection cubaseMacros;

        private Action MacoSavedEventHandler;


        public EditMacroControl(CubaseMacroCollection macros, CubaseMacro macroToEdit, Action macroSavedEventHandler)
        {
            InitializeComponent();
            this.cubaseMacros = macros;
            this.macro = macroToEdit;
            this.MacoSavedEventHandler = macroSavedEventHandler;
            this.UpdateButton.Click += UpdateButton_Click;
            ThemeApplier.ApplyDarkTheme(this);
            this.BindControls();

        }

        private void BindControls()
        {
            this.BackgroundColour.Bind(Color.FromArgb(this.macro.BackgroundColourARGB), "Background Colour", (color) =>
            {
                this.macro.BackgroundColourARGB = color.ToArgb();
                this.UpdateExampleButton();
            });
            this.ForegroundColour.Bind(Color.FromArgb(this.macro.ForegroundColourARGB), "Foreground Colour", (color) =>
            {
                this.macro.ForegroundColourARGB = color.ToArgb();
                this.UpdateExampleButton();
            });
            this.ToggleBackgroundColour.Bind(Color.FromArgb(this.macro.ToggleBackgroundColourARGB), "Toggle Background Colour", (color) =>
            {
                this.macro.ToggleBackgroundColourARGB = color.ToArgb();
                this.UpdateExampleButton();
            });
            this.ToggleForgroundColour.Bind(Color.FromArgb(this.macro.ToggleForegroundColourARGB), "Toggle Foreground Colour", (color) =>
            {
                this.macro.ToggleForegroundColourARGB = color.ToArgb();
                this.UpdateExampleButton();
            });
            this.MacroTitle.Bind(nameof(CubaseMacro.Title), this.macro);
            this.MacroButtonType.EnumType = typeof(CubaseMacroButtonType);
            this.MacroButtonType.Bind(this.macro.ButtonType);
            this.MacroTitleToggled.Bind(nameof(CubaseMacro.TitleToggle), this.macro);
            this.MacroMenuChangesVisibility.Bind(nameof(CubaseMacro.MenuChangesVisibility), this.macro);
            this.MacroButtonType.OnEnumSelected += (selectedValue) =>
            {
                this.macro.ButtonType = (CubaseMacroButtonType)selectedValue;
                this.LoadEditControlForButtonType();
            };
            this.LoadEditControlForButtonType();
            this.UpdateExampleButton();
        }

        private void UpdateExampleButton()
        {
            this.ExampleSingle.BackColor = Color.FromArgb(this.macro.BackgroundColourARGB);
            this.ExampleSingle.ForeColor = Color.FromArgb(this.macro.ForegroundColourARGB);
            this.ExampleToggled.BackColor = Color.FromArgb(this.macro.ToggleBackgroundColourARGB);
            this.ExampleToggled.ForeColor = Color.FromArgb(this.macro.ToggleForegroundColourARGB);  
        }

        private void LoadEditControlForButtonType()
        {
            switch (this.macro.ButtonType)
            {
                case CubaseMacroButtonType.Single:
                    this.LoadEditControl(new SingleKeyEditor());
                    this.ToggleBackgroundColour.Enabled = false;
                    this.ToggleForgroundColour.Enabled = false;
                    break;
                case CubaseMacroButtonType.Toggle:
                    this.LoadEditControl(new ToggleKeyEditor());
                    this.ToggleBackgroundColour.Enabled = true;
                    this.ToggleForgroundColour.Enabled = true;
                    break;
            }
            if (this.macro.MacroType == CubaseMacroType.Menu)
            {
                this.LoadEditControl(new SingleKeyEditor());
            }
            this.MacroTitleToggled.Enabled = this.macro.ButtonType == CubaseMacroButtonType.Toggle;
            this.MacroMenuChangesVisibility.Enabled = this.macro.MacroType == CubaseMacroType.Menu;
        }

        private void LoadEditControl(Control control)
        {

            this.ContentPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            this.ContentPanel.Controls.Add(control);
            ((IKeyEditor)control).Macro = this.macro;
        }

        private void UpdateButton_Click(object? sender, EventArgs e)
        {
            if (this.MacoSavedEventHandler != null)
            {
                this.MacoSavedEventHandler.Invoke();
            }
        }

    }
}
