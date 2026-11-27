using Cubase.Macro.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Cubase.Macro.Forms.Scripts
{
    public partial class ScriptMainForm : Form
    {
        private readonly IServiceProvider serviceProvider;

        private ScriptIndexControl scriptIndex;

        public ScriptMainForm(IServiceProvider serviceProvider)
        {
            Application.SetColorMode(SystemColorMode.Dark);
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Cubase Scripts";
            this.Initialise();
        }

        private void Initialise()
        {
            this.NewScript.Click += NewScript_Click;
            this.OpenScript.Click += OpenScript_Click;
            this.scriptIndex = new ScriptIndexControl(this.serviceProvider);
            this.MainPanel.Controls.Clear();
            this.MainPanel.Controls.Add(this.scriptIndex);
            this.scriptIndex.Initialise();

        }

        private void OpenScript_Click(object? sender, EventArgs e)
        {
            var fileOpen = new OpenFileDialog()
            {
                InitialDirectory = CubaseMacroConstants.ScriptFolder,
                Filter = $"Cubase Script (*{CubaseMacroConstants.NutstoneCubaseScript})|*{CubaseMacroConstants.NutstoneCubaseScript}",
                AddExtension = true,
            };
            if (fileOpen.ShowDialog() == DialogResult.OK)
            {
                var content = File.ReadAllText(fileOpen.FileName);
                var newScriptForm = this.serviceProvider.GetService<EditScriptForm>();
                newScriptForm.Script = content;
                newScriptForm.ShowDialog();
            }
        }

        private void NewScript_Click(object? sender, EventArgs e)
        {
            var newScriptForm = this.serviceProvider.GetService<EditScriptForm>();
            newScriptForm.ShowDialog();
        }
    }
}
