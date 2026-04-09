using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Configuration.KeyEditors
{
    public partial class ToggleKeyEditor : UserControl, IKeyEditor
    {

        private CubaseMacro macro;

        public ToggleKeyEditor()
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CubaseMacro Macro
        {
            get
            {
                return this.macro;
            }
            set
            {
                this.macro = value;
                this.BindControls();
            }
        }


        private void BindControls()
        {
            MacroToggleOn.Initialise(this.macro.ToggleOnKeys);
            MacroToggleOff.Initialise(this.macro.ToggleOffKeys);    
        }
    }
}
