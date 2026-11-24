using Cubase.Macro.Common.Models;
using System.ComponentModel;

namespace Cubase.Macro.Forms.Configuration.KeyEditors
{
    public partial class ToggleKeyEditor : UserControl, IKeyEditor
    {

        private CubaseMacro macro;

        public ToggleKeyEditor()
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IServiceProvider ServiceProvider { get; set; }

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
            MacroToggleOn.Initialise(this.macro.ToggleOnKeys, this.ServiceProvider);
            MacroToggleOff.Initialise(this.macro.ToggleOffKeys, this.ServiceProvider);
        }
    }
}
