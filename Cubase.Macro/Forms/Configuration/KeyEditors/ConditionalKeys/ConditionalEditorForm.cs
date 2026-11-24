using Cubase.Macro.Common.Models;
using Cubase.Macro.Forms.Configuration.KeySelector;

namespace Cubase.Macro.Forms.Configuration.KeyEditors.ConditionalKeys
{
    public partial class ConditionalEditorForm : BaseWindows11Form
    {

        private IServiceProvider serviceProvider;

        private Action<CubaseConditionalKey> OnConditionalKeyChange;

        private CubaseConditionalKey cubaseConditionalKey;

        public ConditionalEditorForm()
        {
            InitializeComponent();
        }

        public ConditionalEditorForm(IServiceProvider serviceProvider,
                                     CubaseConditionalKey cubaseConditionalKey,
                                     Action<CubaseConditionalKey> onConditionalKeyChange)
        {
            InitializeComponent();
            this.serviceProvider = serviceProvider;
            this.cubaseConditionalKey = cubaseConditionalKey;
            this.OnConditionalKeyChange = onConditionalKeyChange;
            this.Text = "Edit Conditional Macro Key";
            this.StartPosition = FormStartPosition.CenterParent;
            ThemeApplier.ApplyDarkTheme(this);
            this.EditorControl.Initialise(serviceProvider, cubaseConditionalKey, onConditionalKeyChange);
            this.PopulateMacroName();
            this.SelectMacroButton.Click += SelectMacroButton_Click;
            this.OK.Click += OK_Click;
        }

        private void OK_Click(object? sender, EventArgs e)
        {
            if (this.cubaseConditionalKey.Conditions.Count == 0)
            {
                MessageBox.Show("You must select at least one condition");
                return;
            }
            this.OnConditionalKeyChange.Invoke(this.cubaseConditionalKey);
            this.Close();
        }

        private void SelectMacroButton_Click(object? sender, EventArgs e)
        {
            var selectMacroForm = new KeyCommandSelectorForm(this.MacroSelected);
            selectMacroForm.ShowDialog();
        }

        private void MacroSelected(CubaseKeyCommand cubaseKeyCommand)
        {
            this.cubaseConditionalKey.Command = cubaseKeyCommand;
            this.PopulateMacroName();
        }

        private void PopulateMacroName()
        {
            if (this.cubaseConditionalKey.Command != null)
            {
                this.MacroName.Text = $"{this.cubaseConditionalKey.Command.Name}";
            }

        }

    }
}
