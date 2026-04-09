using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Configuration.KeySelector
{
    public partial class KeyCommandSelectorForm : BaseWindows11Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CubaseKeyCommand> OnKeySelected { get; set; }

        public KeyCommandSelectorForm()
        {
            InitializeComponent();
        }

        public KeyCommandSelectorForm(Action<CubaseKeyCommand> OnKeySelected)
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            var commands = new CubaseKeyCommandParser().Parse();
            this.keyCommandListView.Populate(commands);
            this.OnKeySelected = OnKeySelected;
            this.keyCommandListView.OnKeySelected = (cmd) => 
            { 
                this.OnKeySelected.Invoke(cmd);
                this.Close();
            };
        }
    }
}
