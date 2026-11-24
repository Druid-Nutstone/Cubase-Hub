using Cubase.Macro.Common.Models;
using Cubase.Macro.Services.ConditionalMacros;
using Microsoft.Extensions.DependencyInjection;

namespace Cubase.Macro.Forms.Configuration.KeyEditors.ConditionalKeys
{
    public partial class ConditionalKeyControl : UserControl
    {
        private IConditionalMacroService conditionalService;

        private CubaseCondition cubaseCondition;

        public ConditionalKeyControl()
        {
            InitializeComponent();
        }


        public ConditionalKeyControl(CubaseCondition cubaseConditional,
                                     IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.cubaseCondition = cubaseConditional;
            this.conditionalService = serviceProvider.GetService<IConditionalMacroService>();

            this.Function.Items.Clear();
            this.Property.Items.Clear();
            this.Condition.Items.Clear();

            this.Function.Items.AddRange(conditionalService.GetFunctionNames().ToArray());
            if (!string.IsNullOrEmpty(cubaseConditional.FunctionName))
            {

                this.Function.SelectedItem = cubaseConditional.FunctionName;
                this.PopulateProperties();
            }

            this.Condition.Items.Add(bool.FalseString);
            this.Condition.Items.Add(bool.TrueString);

            this.Function.SelectedIndexChanged += Function_SelectedIndexChanged;
            this.Property.SelectedIndexChanged += Property_SelectedIndexChanged;
            this.Condition.SelectedIndexChanged += Condition_SelectedIndexChanged;
        }

        private void Condition_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.cubaseCondition.State = bool.Parse(this.Condition.SelectedItem.ToString());
        }

        private void Property_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.cubaseCondition.PropertyName = this.Property.SelectedItem.ToString();
        }

        private void Function_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.cubaseCondition.FunctionName = this.Function.SelectedItem.ToString();
            this.PopulateProperties();
        }

        private void PopulateProperties()
        {
            this.Property.Items.Clear();
            this.Property.Items.AddRange(this.conditionalService.GetFunctionProperties(this.Function.SelectedItem.ToString()).ToArray());
        }
    }
}
