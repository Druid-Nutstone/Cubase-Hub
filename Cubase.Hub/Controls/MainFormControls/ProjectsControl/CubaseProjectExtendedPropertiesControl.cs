using Cubase.Hub.Services.Models;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{
    public partial class CubaseProjectExtendedPropertiesControl : UserControl
    {
        private readonly CubaseProjectItemMixesControl cubaseProjectItemMixesControl;

        public CubaseProjectExtendedPropertiesControl(CubaseProjectItemMixesControl cubaseProjectItemMixesControl)
        {
            InitializeComponent();
            this.cubaseProjectItemMixesControl = cubaseProjectItemMixesControl;
            this.Dock = DockStyle.Top;
        }

        public void SetProject(CubaseProject project)
        {
            this.Mixes.Controls.Clear();
            this.cubaseProjectItemMixesControl.SetMixes(project.Mixes);
            this.Mixes.Controls.Add(this.cubaseProjectItemMixesControl);
        }


    }
}
