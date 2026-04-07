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

        public EditMacroControl()
        {
            InitializeComponent();
        }

        public EditMacroControl(CubaseMacroCollection macros, CubaseMacro macroToEdit, Action macroSavedEventHandler)
        {
            InitializeComponent();
            this.cubaseMacros = macros;
            this.macro = macroToEdit;
            this.MacoSavedEventHandler = macroSavedEventHandler;
            this.UpdateButton.Click += UpdateButton_Click; 
            this.MacroTitle.Bind(nameof(CubaseMacro.Title), this.macro);
            if (this.macro.MacroType == CubaseMacroType.KeyCommand)
            {
                To do - Add as new Control to edit the content of the macro   
            }
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
