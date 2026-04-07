using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Configuration
{
    public partial class SettingsForm : BaseWindows11Form
    {
        private readonly SettingsMainControl settingsMainControl;

        public SettingsForm(SettingsMainControl settingsMainControl)
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.settingsMainControl = settingsMainControl;
            this.Initialise();
        }

        private void Initialise()
        {
            this.LoadPanel(this.settingsMainControl);
            this.settingsMainControl.Initialise();
        }

        private void LoadPanel(Control control)
        {
            this.DataPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            this.DataPanel.Controls.Add(control);   
        }
    }
}
