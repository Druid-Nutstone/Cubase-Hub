using Cubase.Macro.Services.Lex;
using Microsoft.Extensions.DependencyInjection;

namespace Cubase.Macro.Forms.Scripts.Editor
{
    public partial class ScriptEditorControl : UserControl
    {
        private IServiceProvider serviceProvider;

        private ScriptEditor editor;

        private IScriptService scriptRunner;


        public ScriptEditorControl(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.scriptRunner = serviceProvider.GetService<IScriptService>();
            this.Initialise();
        }

        private void Initialise()
        {
            this.editor = new ScriptEditor(this.serviceProvider, this.OnError, string.Empty)
            {
                Dock = DockStyle.Fill
            };
            this.MainPanel.Controls.Clear();
            this.MainPanel.Controls.Add(this.editor);
            this.RunScript.Click += RunScript_Click;
            this.NewScript.Click += NewScript_Click;
        }

        private void NewScript_Click(object? sender, EventArgs e)
        {
            this.editor.NewScript();
        }

        private void RunScript_Click(object? sender, EventArgs e)
        {
            this.OnMessage("Running Script ..");
            this.scriptRunner.RunScript(this.GetScriptContent(), (err) =>
            {
                MessageBox.Show($"Script Failed. {Environment.NewLine} {err}");
                this.OnError(err);
            }, OnMessage);
        }

        private void OnError(string errText)
        {
            this.ErrorMessage.ForeColor = Color.IndianRed;
            this.ErrorMessage.Text = errText;
            this.ErrorMessage.Refresh();
        }

        private void OnMessage(string msgText)
        {
            this.ErrorMessage.ForeColor = Color.Green;
            this.ErrorMessage.Text = msgText;
            this.ErrorMessage.Refresh();
        }

        public string GetScriptContent()
        {
            return this.editor.Text;
        }

        public void LoadScript(string scriptContent)
        {
            this.editor.LoadScriptText(scriptContent);
        }
    }
}
