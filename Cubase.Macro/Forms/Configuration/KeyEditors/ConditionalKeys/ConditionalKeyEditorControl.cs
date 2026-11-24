using Cubase.Macro.Common.Models;

namespace Cubase.Macro.Forms.Configuration.KeyEditors.ConditionalKeys
{
    public partial class ConditionalKeyEditorControl : UserControl
    {

        private CubaseConditionalKey conditionalKey;

        private IServiceProvider serviceProvider;

        private Action<CubaseConditionalKey> OnKeyChange;

        private ConditionalKeyPanel conditionalKeyPanel;

        public ConditionalKeyEditorControl()
        {
            InitializeComponent();
        }

        public void Initialise(IServiceProvider serviceProvider,
                               CubaseConditionalKey cubaseConditionalKey,
                               Action<CubaseConditionalKey> onConditionalKeyChange)
        {
            this.serviceProvider = serviceProvider;
            this.conditionalKey = cubaseConditionalKey;
            this.OnKeyChange = onConditionalKeyChange;
            this.MainPanel.Controls.Clear();
            this.conditionalKeyPanel = new ConditionalKeyPanel();
            this.conditionalKeyPanel.Reset();
            this.MainPanel.Controls.Add(this.conditionalKeyPanel);
            cubaseConditionalKey.Conditions.ForEach(this.AddCondition);
            this.AddConditionButton.Click += AddConditionButton_Click;
        }

        private void AddConditionButton_Click(object? sender, EventArgs e)
        {
            var newCondition = new CubaseCondition();
            this.conditionalKey.Conditions.Add(newCondition);
            this.AddCondition(newCondition);
        }

        private void AddCondition(CubaseCondition cubaseCondition)
        {
            this.conditionalKeyPanel.AddCondition(cubaseCondition, this.serviceProvider);
            ThemeApplier.ApplyDarkTheme(this);
        }
    }
}
