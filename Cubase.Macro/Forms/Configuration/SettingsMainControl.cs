using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Configuration
{
    public partial class SettingsMainControl : UserControl
    {
        private CubaseMacroCollection macros;
        
        private readonly IConfigurationService configurationService;

        public SettingsMainControl(IConfigurationService configurationService)
        {
            InitializeComponent();
            this.configurationService = configurationService;
            NewMenu.Click += NewMenu_Click;
        }

        private void NewMenu_Click(object? sender, EventArgs e)
        {
            var newMenuMacro = CubaseMacro.CreateNewMenuMacro();    
            this.macros.Macros.Add(newMenuMacro);
            var editMacro = new EditMacroControl(this.macros, newMenuMacro, this.MacroSaved);
            this.LoadDataPanel(editMacro);
        }

        private void MacroSaved()
        {
            this.macros.Save();
            this.LoadTreeView();
        }

        public void Initialise()
        {
            ThemeApplier.ApplyDarkTheme(this);
            this.macros = CubaseMacroCollection.Load();
            this.LoadTreeView();
        }
    
        private void LoadDataPanel(Control control)
        {
            this.DataPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            this.DataPanel.Controls.Add(control);
        }

        private void LoadTreeView()
        {
            this.TreePanel.Controls.Clear();
            var treeView = new MacroTreeView(this.DataPanel, this.MacroSaved, this.configurationService.Configuration);
            this.TreePanel.Controls.Add(treeView);
            treeView.Build(this.macros);
        }
    }
}
