using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Controls.MainFormControls.ProjectsControl.Search
{
    public partial class ProjectSearch : UserControl
    {
        public Action<string> OnSearchTextChanged;

        public ProjectSearch()
        {
            InitializeComponent();
            this.FilterText.TextChanged += (s, e) =>
            {
                OnSearchTextChanged?.Invoke(this.FilterText.Text);
            };
            this.ClearFilter.Click += (s, e) =>
            {
                this.FilterText.Text = string.Empty;
                this.FilterText.PlaceholderText = "Search projects...";
                OnSearchTextChanged?.Invoke(this.FilterText.Text);
            };
        }
    }
}
