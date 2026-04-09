using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Configuration
{
    public class MacroTreeViewContentMenu : ContextMenuStrip
    {
        public MacroTreeViewContentMenu(CubaseMacroCollection macros, CubaseMacro parent, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Items.Add(new NewMenuTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));
            this.Items.Add(new NewCommandTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));
            this.Items.Add(new DeleteMenuTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));    
        }
    }

    public class NewCommandTreeViewMenuItem : ToolStripMenuItem
    {
        public CubaseMacro Macro { get; private set; }
        
        public CubaseMacroCollection Macros { get; private set; }

        private Panel DataPanel { get; set; }

        private Action MacroUpdatedEventHandler { get; set; }


        public NewCommandTreeViewMenuItem(CubaseMacroCollection macros, CubaseMacro parent, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Macro = parent;
            this.Macros = macros;
            this.DataPanel = dataPanel;
            this.MacroUpdatedEventHandler = macroUpdatedEventHandler;
            this.Text = "Add Macro";
        }

        protected override void OnClick(EventArgs e)
        {
            var newMacro = CubaseMacro.CreateKeyCommandMacro("New Macro", this.Macro.Id);
            this.Macro.Macros.Add(newMacro);
            var editControl = new EditMacroControl(this.Macros, newMacro, this.MacroUpdatedEventHandler);
            this.DataPanel.Controls.Clear();
            editControl.Dock = DockStyle.Fill;
            this.DataPanel.Controls.Add(editControl);

        }
    }


    public class DeleteMenuTreeViewMenuItem : ToolStripMenuItem
    {
        public CubaseMacro Macro { get; private set; }

        public CubaseMacroCollection Macros { get; private set; }

        private Panel DataPanel { get; set; }

        private Action MacroUpdatedEventHandler { get; set; }

        public DeleteMenuTreeViewMenuItem(CubaseMacroCollection macros, CubaseMacro parent, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Macro = parent;
            this.Macros = macros;
            this.DataPanel = dataPanel;
            this.MacroUpdatedEventHandler = macroUpdatedEventHandler;
            this.Text = "Delete";
        }

        protected override void OnClick(EventArgs e)
        {
            this.DataPanel.Controls.Clear();

            foreach (var macro in this.Macros)
            {
                RemoveMacroRecursive(macro, this.Macro);
            }

            this.MacroUpdatedEventHandler?.Invoke();
        }

        private bool RemoveMacroRecursive(CubaseMacro parent, CubaseMacro target)
        {
            if (parent.Macros == null)
                return false;

            // Try remove directly
            if (parent.Macros.Remove(target))
                return true;

            // Otherwise search children
            foreach (var child in parent.Macros)
            {
                if (RemoveMacroRecursive(child, target))
                    return true;
            }

            return false;
        }

    }

    public class NewMenuTreeViewMenuItem : ToolStripMenuItem
    {
        public CubaseMacro Macro { get; private set; }

        public CubaseMacroCollection Macros { get; private set; }

        private Panel DataPanel { get; set; }

        private Action MacroUpdatedEventHandler { get; set; }

        public NewMenuTreeViewMenuItem(CubaseMacroCollection macros, CubaseMacro parent, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Macro = parent;
            this.Macros = macros;
            this.DataPanel = dataPanel;
            this.MacroUpdatedEventHandler = macroUpdatedEventHandler;
            this.Text = "Add Menu";
        }

        protected override void OnClick(EventArgs e)
        {
            var newMacro = CubaseMacro.CreateNewMenuMacro(this.Macro.Id);
            this.Macro.Macros.Add(newMacro);
            var editControl = new EditMacroControl(this.Macros, newMacro, this.MacroUpdatedEventHandler);
            this.DataPanel.Controls.Clear();
            editControl.Dock = DockStyle.Fill;
            this.DataPanel.Controls.Add(editControl);

        }
    }
}
