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
            this.MacroTitle.Bind(nameof(CubaseMacro.Title), this.macro);
            this.MacroButtonType.EnumType = typeof(CubaseMacroButtonType);
            this.MacroButtonType.Bind(this.macro.ButtonType);
            this.MacroButtonType.OnEnumSelected += (selectedValue) =>
            {
                this.macro.ButtonType = (CubaseMacroButtonType)selectedValue;
                this.LoadEditControlForButtonType();
            };
            this.LoadEditControlForButtonType();
        }

        private void LoadEditControlForButtonType()
        {
            switch (this.macro.ButtonType)
            {
                case CubaseMacroButtonType.Single:
                    this.LoadEditControl(new SingleKeyEditor());
                    break;
                case CubaseMacroButtonType.Toggle:
                    this.LoadEditControl(new ToggleKeyEditor());
                    break;
            }
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
