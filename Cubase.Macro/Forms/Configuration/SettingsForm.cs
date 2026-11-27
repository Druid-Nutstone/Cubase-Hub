using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Scripts;
using System.Diagnostics;

namespace Cubase.Macro.Forms.Configuration
{
    public partial class SettingsForm : BaseWindows11Form
    {
        private readonly SettingsMainControl settingsMainControl;

        private readonly ScriptMainForm scriptMainForm;

        public SettingsForm(SettingsMainControl settingsMainControl, ScriptMainForm scriptsForm)
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.settingsMainControl = settingsMainControl;
            this.scriptMainForm = scriptsForm;
            this.OpenConfig.Click += OpenConfig_Click;
            this.OpenLyrics.Click += OpenLyrics_Click;
            this.OpenScripts.Click += OpenScripts_Click;
            this.Initialise();
        }

        private void OpenScripts_Click(object? sender, EventArgs e)
        {
            this.scriptMainForm.ShowDialog();
        }

        private void OpenLyrics_Click(object? sender, EventArgs e)
        {

        }

        private void OpenConfig_Click(object? sender, EventArgs e)
        {
            Process p = new Process() { StartInfo = new ProcessStartInfo() { FileName = CubaseMacroConstants.MacroConfigurationFileName, UseShellExecute = true } };
            p.Start();
            p.WaitForExit();
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
