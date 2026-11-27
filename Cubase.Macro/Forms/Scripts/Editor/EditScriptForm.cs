using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Scripts.Editor;
using Cubase.Macro.Services.Lex;
using System.ComponentModel;
using System.IO;

namespace Cubase.Macro.Forms.Scripts
{
    public partial class EditScriptForm : Form
    {
        private readonly IServiceProvider serviceProvider;

        private ScriptEditorControl editorControl;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Script { get; set; }

        public EditScriptForm(IServiceProvider serviceProvider)
        {
            Application.SetColorMode(SystemColorMode.Dark);
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.serviceProvider = serviceProvider;
            this.Text = "Edit Cubase Script";
            var menu = new EditScriptMenu(this.MainMenu, serviceProvider, this);
            this.Initialise();
        }


        private void Initialise()
        {
            this.editorControl = new ScriptEditorControl(this.serviceProvider)
            {
                Dock = DockStyle.Fill
            };
            this.MainPanel.Controls.Clear();
            this.MainPanel.Controls.Add(this.editorControl);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!string.IsNullOrEmpty(this.Script))
            {
                this.editorControl.LoadScript(this.Script);
            }
        }

        public void OpenScript()
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
                this.editorControl.LoadScript(content);
            }
        }

        public void SaveScript()
        {
            var scriptName = IsValidScript();
            if (!string.IsNullOrEmpty(scriptName))
            {
                var fileSaveAs = new SaveFileDialog()
                {
                    InitialDirectory = CubaseMacroConstants.ScriptFolder,
                    Filter = $"Cubase Script (*{CubaseMacroConstants.NutstoneCubaseScript})|*{CubaseMacroConstants.NutstoneCubaseScript}",
                    AddExtension = true,
                    OverwritePrompt = true,
                    FileName = scriptName
                };
                if (fileSaveAs.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(fileSaveAs.FileName, this.editorControl.GetScriptContent());
                }
            }
        }

        private string? IsValidScript()
        {
            var script = this.editorControl.GetScriptContent();
            var macroParser = new MacroParser(new LexParser().Parse(script));
            var metaData = macroParser.ParseScriptMetaData();
            if (string.IsNullOrEmpty(metaData.Name))
            {
                MessageBox.Show("There is no NAME specified in the script");
                return null;
            }
            if (string.IsNullOrEmpty(metaData.Description))
            {
                MessageBox.Show("There is no Description specified in the script");
                return null;
            }
            return metaData.Name;
        }
    }
}
