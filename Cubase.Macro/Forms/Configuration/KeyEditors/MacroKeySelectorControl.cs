using Cubase.Macro.Forms.Configuration.KeySelector;
using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Configuration.KeyEditors
{
    public partial class MacroKeySelectorControl : UserControl
    {
        private List<CubaseKeyCommand> keys;

        public MacroKeySelectorControl()
        {
            InitializeComponent();
            ButtonAdd.Click += ButtonAdd_Click;
            ButtonDel.Click += ButtonDel_Click;
        }

        private void ButtonDel_Click(object? sender, EventArgs e)
        {
            if (this.MacroCommandListView.SelectedItems.Count > 0)
            {
                var item = this.MacroCommandListView.SelectedItems[0] as MacroKeyCommandListViewItem;
                if (item != null)
                {
                    // Remove the selected command from the macro's key list
                    this.keys.Remove(item.Command);
                    // Refresh the list view to reflect the removal
                    this.MacroCommandListView.Populate(this.keys);
                }
            }
        }

        private void ButtonAdd_Click(object? sender, EventArgs e)
        {
            var addKeyForm = new KeyCommandSelectorForm(this.KeySelected);
            addKeyForm.ShowDialog();
        }


        private void KeySelected(CubaseKeyCommand command)
        {
            // Add the selected command to the macro's key list
            this.keys.Add(command);
            // Refresh the list view to show the new command
            this.MacroCommandListView.Populate(this.keys);
        }

        public void Initialise(List<CubaseKeyCommand> commands)
        {
            this.keys = commands;
            this.MacroCommandListView.Populate(commands);
        }


    }
}
