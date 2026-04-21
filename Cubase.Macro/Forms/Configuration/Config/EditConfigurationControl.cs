using Cubase.Macro.Forms.Configuration.KeySelector;
using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Configuration.Config
{
    public partial class EditConfigurationControl : UserControl
    {
        private readonly CubaseMacroConfiguration cubaseMacroConfiguration;

        public EditConfigurationControl(CubaseMacroConfiguration cubaseMacroConfiguration)
        {
            InitializeComponent();
            this.SelectVisibilityKey.Click += SelectVisibilityKey_Click;
            this.cubaseMacroConfiguration = cubaseMacroConfiguration;
            ThemeApplier.ApplyDarkTheme(this);
            ResetVisibilityKey.Bind(nameof(CubaseMacroConfiguration.ResetVisibilityKey), this.cubaseMacroConfiguration);
            MenuHeight.Bind(nameof(CubaseMacroConfiguration.MenuHeight), this.cubaseMacroConfiguration);
            MenuHeight.LostFocus += SaveConfig;
            KeyHeight.Bind(nameof(CubaseMacroConfiguration.KeyHeight), this.cubaseMacroConfiguration);
            KeyHeight.LostFocus += SaveConfig;
            CubaseExecutableName.Bind(nameof(CubaseMacroConfiguration.CubaseExecutable), this.cubaseMacroConfiguration);
            CubaseExecutableName.LostFocus += SaveConfig;   
            CubaseProjectWindowStartsWith.Bind(nameof(CubaseMacroConfiguration.CubaseProjectWindowName), this.cubaseMacroConfiguration);
            CubaseProjectWindowStartsWith.LostFocus += SaveConfig;
            CubaseRestartWindowsMidiService.Bind(nameof(CubaseMacroConfiguration.ReloadWindowsMidiService), this.cubaseMacroConfiguration);
            CubaseRestartWindowsMidiService.CheckedChanged += SaveConfig;
        }

        private void SaveConfig(object? sender, EventArgs e)
        {
            this.cubaseMacroConfiguration.Save();
        }

        private void SelectVisibilityKey_Click(object? sender, EventArgs e)
        {
            var keySelector = new KeyCommandSelectorForm((cubaseMacro) => 
            {
                this.ResetVisibilityKey.Text = cubaseMacro.Serialise();
                this.cubaseMacroConfiguration.Save();
            });
            keySelector.ShowDialog();
        }
    }
}
