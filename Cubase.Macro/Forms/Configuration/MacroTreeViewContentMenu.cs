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
            if (parent.MacroType == CubaseMacroType.Menu)
            {
                this.Items.Add(new NewMenuTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));
                this.Items.Add(new NewCommandTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));
            }
            this.Items.Add(new MoveUpCommandTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));
            this.Items.Add(new MoveDownCommandTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));
            this.Items.Add(new CopyCommandTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));
            if (parent.MacroType == CubaseMacroType.Menu)
            {
                this.Items.Add(new PasteCommandTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));
            }
            this.Items.Add(new DeleteMenuTreeViewMenuItem(macros, parent, dataPanel, macroUpdatedEventHandler));
        }

        public MacroTreeViewContentMenu(CubaseMacroCollection macros, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Items.Add(new NewCommonCommandTreeViewMenuItem(macros, dataPanel, macroUpdatedEventHandler));
            this.Items.Add(new MoveUpCommandTreeViewMenuItem(macros, null, dataPanel, macroUpdatedEventHandler));
            this.Items.Add(new MoveDownCommandTreeViewMenuItem(macros, null, dataPanel, macroUpdatedEventHandler));
            this.Items.Add(new CopyCommandTreeViewMenuItem(macros, null, dataPanel, macroUpdatedEventHandler)); ;
            this.Items.Add(new PasteCommandTreeViewMenuItem(macros, null, dataPanel, macroUpdatedEventHandler));
            this.Items.Add(new DeleteMenuTreeViewMenuItem(macros, null, dataPanel, macroUpdatedEventHandler));
        }
    }

    public class MoveDownCommandTreeViewMenuItem : ToolStripMenuItem
    {
        public CubaseMacro Macro { get; private set; }

        public CubaseMacroCollection Macros { get; private set; }

        private Panel DataPanel { get; set; }

        private Action MacroUpdatedEventHandler { get; set; }


        public MoveDownCommandTreeViewMenuItem(CubaseMacroCollection macros, CubaseMacro parent, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Macro = parent;
            this.Macros = macros;
            this.DataPanel = dataPanel;
            this.MacroUpdatedEventHandler = macroUpdatedEventHandler;
            this.Text = "Move Down";
        }

        protected override void OnClick(EventArgs e)
        {
            List<CubaseMacro> siblings = new List<CubaseMacro>();
            if (this.Macro.ParentId == Guid.Empty) // must be common macros 
            {
                siblings = this.Macros.CommonMacros;
            }
            else
            {
                var actualParent = this.Macros.FindParentFromBase(this.Macro.ParentId.Value);

                if (actualParent?.Macros == null)
                    return;

                siblings = actualParent.Macros;
            }

            var index = siblings.IndexOf(this.Macro);

            // Already at the bottom → do nothing
            if (index < 0 || index >= siblings.Count - 1)
                return;

            // Swap with next item
            var temp = siblings[index + 1];
            siblings[index + 1] = siblings[index];
            siblings[index] = temp;

            MacroUpdatedEventHandler();
        }
    }

    public class MoveUpCommandTreeViewMenuItem : ToolStripMenuItem
    {
        public CubaseMacro Macro { get; private set; }

        public CubaseMacroCollection Macros { get; private set; }

        private Panel DataPanel { get; set; }

        private Action MacroUpdatedEventHandler { get; set; }


        public MoveUpCommandTreeViewMenuItem(CubaseMacroCollection macros, CubaseMacro parent, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Macro = parent;
            this.Macros = macros;
            this.DataPanel = dataPanel;
            this.MacroUpdatedEventHandler = macroUpdatedEventHandler;
            this.Text = "Move Up";
        }

        protected override void OnClick(EventArgs e)
        {
            List<CubaseMacro> siblings = new List<CubaseMacro>();
            if (this.Macro.ParentId == Guid.Empty) // must be common macros 
            {
                siblings = this.Macros.CommonMacros;
            }
            else
            {
                var actualParent = this.Macros.FindParentFromBase(this.Macro.ParentId.Value);

                if (actualParent?.Macros == null)
                    return;

                siblings = actualParent.Macros;
            }

            var index = siblings.IndexOf(this.Macro);
            // Already at the top → do nothing
            if (index <= 0)
                return;

            // Swap with previous item
            var temp = siblings[index - 1];
            siblings[index - 1] = siblings[index];
            siblings[index] = temp;

            MacroUpdatedEventHandler();
        }
    }

    public class CopyCommandTreeViewMenuItem : ToolStripMenuItem
    {
        public CubaseMacro Macro { get; private set; }

        public CubaseMacroCollection Macros { get; private set; }

        private Panel DataPanel { get; set; }

        private Action MacroUpdatedEventHandler { get; set; }


        public CopyCommandTreeViewMenuItem(CubaseMacroCollection macros, CubaseMacro parent, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Macro = parent;
            this.Macros = macros;
            this.DataPanel = dataPanel;
            this.MacroUpdatedEventHandler = macroUpdatedEventHandler;
            this.Text = "Copy Macro";
        }

        protected override void OnClick(EventArgs e)
        {
            var macroAsJson = System.Text.Json.JsonSerializer.Serialize(this.Macro);
            Clipboard.SetText(macroAsJson);
        }
    }

    public class PasteCommandTreeViewMenuItem : ToolStripMenuItem
    {
        public CubaseMacro Macro { get; private set; }

        public CubaseMacroCollection Macros { get; private set; }

        private Panel DataPanel { get; set; }

        private Action MacroUpdatedEventHandler { get; set; }


        public PasteCommandTreeViewMenuItem(CubaseMacroCollection macros, CubaseMacro parent, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Macro = parent;
            this.Macros = macros;
            this.DataPanel = dataPanel;
            this.MacroUpdatedEventHandler = macroUpdatedEventHandler;
            this.Text = "Paste Macro";
        }

        protected override void OnClick(EventArgs e)
        {
            var clipboardText = Clipboard.GetText();

            if (!string.IsNullOrEmpty(clipboardText))
            {
                var pasteMacro = System.Text.Json.JsonSerializer.Deserialize<CubaseMacro>(clipboardText);
                if (pasteMacro != null)
                {
                    if (this.Macro.MacroType != CubaseMacroType.Menu)
                    {
                        MessageBox.Show("Cannot paste a macro into a non-menu macro.", "Invalid Paste", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (pasteMacro.MacroType == CubaseMacroType.KeyCommand)
                        {
                            var newMacro = CubaseMacro.CreateKeyCommandMacro(pasteMacro.Title, this.Macro.Id);
                            newMacro.TitleToggle = pasteMacro.TitleToggle;
                            newMacro.ButtonType = pasteMacro.ButtonType;
                            newMacro.BackgroundColourARGB = pasteMacro.BackgroundColourARGB;
                            newMacro.ForegroundColourARGB = pasteMacro.ForegroundColourARGB;
                            newMacro.ToggleBackgroundColourARGB = pasteMacro.ToggleBackgroundColourARGB;
                            newMacro.ToggleForegroundColourARGB = pasteMacro.ToggleForegroundColourARGB;
                            newMacro.ToggleState = pasteMacro.ToggleState;
                            newMacro.ToggleOnKeys = pasteMacro.ToggleOnKeys;
                            newMacro.ToggleOffKeys = pasteMacro.ToggleOffKeys;
                            newMacro.ReturnToParentMenuAfterExecution = pasteMacro.ReturnToParentMenuAfterExecution;
                            this.Macro.Macros.Add(newMacro);
                        }
                        else
                        {
                            var newMacro = CubaseMacro.CreateMenuMacro(pasteMacro.Title, new List<CubaseMacro>(), this.Macro.Id);
                            this.Macro.Macros.Add(newMacro);
                            foreach (var child in pasteMacro.Macros)
                            {
                                var childCopy = CubaseMacro.CreateKeyCommandMacro(child.Title, newMacro.Id);
                                childCopy.TitleToggle = child.TitleToggle;
                                childCopy.ButtonType = child.ButtonType;
                                childCopy.BackgroundColourARGB = child.BackgroundColourARGB;
                                childCopy.ForegroundColourARGB = child.ForegroundColourARGB;
                                childCopy.ToggleBackgroundColourARGB = child.ToggleBackgroundColourARGB;
                                childCopy.ToggleForegroundColourARGB = child.ToggleForegroundColourARGB;
                                childCopy.ToggleState = child.ToggleState;
                                childCopy.ToggleOnKeys = child.ToggleOnKeys;
                                childCopy.ToggleOffKeys = child.ToggleOffKeys;
                                childCopy.ReturnToParentMenuAfterExecution = child.ReturnToParentMenuAfterExecution;
                                newMacro.Macros.Add(childCopy);
                            }
                        }
                    }
                    this.MacroUpdatedEventHandler.Invoke();
                }

            }
        }
    }

    public class NewCommonCommandTreeViewMenuItem : ToolStripMenuItem
    {
        public CubaseMacroCollection Macros { get; private set; }

        private Panel DataPanel { get; set; }

        private Action MacroUpdatedEventHandler { get; set; }


        public NewCommonCommandTreeViewMenuItem(CubaseMacroCollection macros, Panel dataPanel, Action macroUpdatedEventHandler)
        {
            this.Macros = macros;
            this.DataPanel = dataPanel;
            this.MacroUpdatedEventHandler = macroUpdatedEventHandler;
            this.Text = "Add Macro";
        }

        protected override void OnClick(EventArgs e)
        {
            var newMacro = CubaseMacro.CreateKeyCommandMacro("New Macro", Guid.Empty);
            this.Macros.CommonMacros.Add(newMacro);
            var editControl = new EditMacroControl(this.Macros, newMacro, this.MacroUpdatedEventHandler);
            this.DataPanel.Controls.Clear();
            editControl.Dock = DockStyle.Fill;
            this.DataPanel.Controls.Add(editControl);

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

            foreach (var macro in this.Macros.Macros)
            {
                RemoveMacroRecursive(macro, this.Macro);
            }

            this.MacroUpdatedEventHandler?.Invoke();
        }

        private bool RemoveMacroRecursive(CubaseMacro parent, CubaseMacro target)
        {
            if (target.ParentId == Guid.Empty)
            {
                this.Macros.CommonMacros.Remove(target);
                return true;
            }

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
