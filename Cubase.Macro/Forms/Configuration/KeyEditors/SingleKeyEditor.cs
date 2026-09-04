using Cubase.Macro.Common.Models;
using System.ComponentModel;

namespace Cubase.Macro.Forms.Configuration.KeyEditors
{
    public partial class SingleKeyEditor : UserControl, IKeyEditor
    {
        private CubaseMacro macro;

        public SingleKeyEditor()
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CubaseMacro Macro
        {
            get
            {
                return this.macro;
            }
            set
            {
                this.macro = value;
                this.BindControls();
            }
        }


        private void BindControls()
        {
            this.KeySelectorControl.Initialise(this.macro.ToggleOnKeys);
            this.ReturnToParentMenuAfterExecution.Bind(nameof(CubaseMacro.ReturnToParentMenuAfterExecution), this.macro);
        }
    }
}
