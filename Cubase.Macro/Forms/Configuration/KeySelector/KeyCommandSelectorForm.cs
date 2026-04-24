using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Configuration.KeySelector
{
    public partial class KeyCommandSelectorForm : BaseWindows11Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<CubaseKeyCommand> OnKeySelected { get; set; }

        private CubaseKeyCommandCollection allCommands;

        public KeyCommandSelectorForm()
        {
            InitializeComponent();
        }

        public KeyCommandSelectorForm(Action<CubaseKeyCommand> OnKeySelected)
        {
            InitializeComponent();
            SearchForText.TextChanged += SearchForText_TextChanged;
            ClearButton.Click += ClearButton_Click; 
            ThemeApplier.ApplyDarkTheme(this);
            this.allCommands = new CubaseKeyCommandParser().Parse();
            this.keyCommandListView.Populate(this.allCommands);
            this.BuildFilter();
            this.FilterBy.SelectedIndexChanged += FilterBy_SelectedIndexChanged;
            this.OnKeySelected = OnKeySelected;
            this.keyCommandListView.OnKeySelected = (cmd) => 
            { 
                this.OnKeySelected.Invoke(cmd);
                this.Close();
            };
            this.ClearFilterButton.Click += ClearFilterButton_Click;
        }

        private void ClearFilterButton_Click(object? sender, EventArgs e)
        {
            this.FilterBy.SelectedIndex = -1;
        }

        private void BuildFilter()
        {
            var filtered = this.allCommands.Select(x => x.Category).Distinct();
            this.FilterBy.Items.AddRange(filtered.ToArray());
        } 

        private void FilterBy_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (this.FilterBy.SelectedIndex > -1)
            {
                var filterbySelection = this.FilterBy.SelectedItem.ToString();
                this.keyCommandListView.Populate(this.allCommands.GetFilteredBy(filterbySelection));
            }
            else
            {
                this.keyCommandListView.Populate(this.allCommands);
            }
        }

        private void ClearButton_Click(object? sender, EventArgs e)
        {
            this.SearchForText.Text = "";
            this.keyCommandListView.Populate(this.allCommands);    
        }

        private void SearchForText_TextChanged(object? sender, EventArgs e)
        {
            if (SearchForText.Text.Length >= 2)
            {
                var filtered = this.allCommands.Search(SearchForText.Text);
                this.keyCommandListView.Populate(filtered);
            }
        }
    }
}
