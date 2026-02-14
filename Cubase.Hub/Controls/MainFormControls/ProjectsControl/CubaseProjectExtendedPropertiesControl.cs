using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
