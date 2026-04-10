using Cubase.Macro.Forms.Configuration.Config;
using Cubase.Macro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Macro.Forms.Configuration
{
    public class MacroTreeView : TreeView
    {
        private Panel dataPanel;

        private CubaseMacroCollection macros;

        private Action macroUpdatedEventHandler;

        private CubaseMacroConfiguration cubaseMacroConfiguration;

        public MacroTreeView(Panel DataPanel, Action MacroUpdatedEventHandler, CubaseMacroConfiguration cubaseMacroConfiguration)
        {
            this.Dock = DockStyle.Fill;
            ThemeApplier.ApplyDarkTheme(this);
            this.macroUpdatedEventHandler = MacroUpdatedEventHandler;
            this.dataPanel = DataPanel;
            this.cubaseMacroConfiguration = cubaseMacroConfiguration;   
        }

        public void Build(CubaseMacroCollection macros)
        {
            this.macros = macros;
            this.Nodes.Clear();
            this.Nodes.Add(new PrimaryMacroTreeNode(macros, dataPanel, macroUpdatedEventHandler)); ;
            this.Nodes.Add(new ConfigurationTreeNode(this.cubaseMacroConfiguration));
            this.ExpandAll();
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (e.Node is MenuMacroTreeNode menuNode)
            {
                var editControl = new EditMacroControl(this.macros, menuNode.Macro, this.macroUpdatedEventHandler);
                this.dataPanel.Controls.Clear();
                editControl.Dock = DockStyle.Fill;
                this.dataPanel.Controls.Add(editControl);
            }
            else if (e.Node is KeyCommandMacroTreeNode keyCommandNode)
            {
                var editControl = new EditMacroControl(this.macros, keyCommandNode.Macro, this.macroUpdatedEventHandler);
                this.dataPanel.Controls.Clear();
                editControl.Dock = DockStyle.Fill;
                this.dataPanel.Controls.Add(editControl);
            }
            else if (e.Node is ConfigurationTreeNode)
            {
                var editControl = new EditConfigurationControl(this.cubaseMacroConfiguration);
                this.dataPanel.Controls.Clear();
                editControl.Dock = DockStyle.Fill;
                this.dataPanel.Controls.Add(editControl);
            }

        }

        public class BaseMacroTreeNode : TreeNode
        {
            public BaseMacroTreeNode()
            {

            }
        }

        public class ConfigurationTreeNode : BaseMacroTreeNode
        {
            private CubaseMacroConfiguration cubaseMacroConfiguration;

            public ConfigurationTreeNode(CubaseMacroConfiguration cubaseMacroConfiguration)
            {
                this.Text = "Configuration";
                this.cubaseMacroConfiguration = cubaseMacroConfiguration;

            }
        }

        public class PrimaryMacroTreeNode : BaseMacroTreeNode
        {
            public PrimaryMacroTreeNode(CubaseMacroCollection macros, Panel DataPanel, Action MacroUpdatedEventHandler)
            {
                this.Text = "Macros";
                foreach (var macro in macros)
                {
                    if (macro.MacroType == CubaseMacroType.Menu)
                    {
                        this.Nodes.Add(new MenuMacroTreeNode(macro, macros, DataPanel, MacroUpdatedEventHandler));
                    }
                    else if (macro.MacroType == CubaseMacroType.KeyCommand)
                    {
                        this.Nodes.Add(new KeyCommandMacroTreeNode(macro, macros, DataPanel, MacroUpdatedEventHandler));
                    }
                }
            }
        }

        public class MenuMacroTreeNode : BaseMacroTreeNode
        {
            public CubaseMacro Macro { get; private set; }

            public MenuMacroTreeNode(CubaseMacro macro, CubaseMacroCollection macros, Panel DataPanel, Action MacroUpdatedEventHandler)
            {
                this.Macro = macro;
                this.Text = macro.Title;
                this.ContextMenuStrip = new MacroTreeViewContentMenu(macros, macro, DataPanel, MacroUpdatedEventHandler);
                foreach (var subMacro in macro.Macros)
                {
                    if (subMacro.MacroType == CubaseMacroType.Menu)
                    {
                        this.Nodes.Add(new MenuMacroTreeNode(subMacro, macros, DataPanel, MacroUpdatedEventHandler));
                    }
                    else if (subMacro.MacroType == CubaseMacroType.KeyCommand)
                    {
                        this.Nodes.Add(new KeyCommandMacroTreeNode(subMacro, macros, DataPanel, MacroUpdatedEventHandler));
                    }
                }
            }
        }

        public class KeyCommandMacroTreeNode : BaseMacroTreeNode
        {
            public CubaseMacro Macro { get; private set; }

            public KeyCommandMacroTreeNode(CubaseMacro macro, CubaseMacroCollection macros, Panel DataPanel, Action MacroUpdatedEventHandler)
            {
                this.Text = macro.Title;
                this.Macro = macro;
                this.ContextMenuStrip = new MacroTreeViewContentMenu(macros, macro, DataPanel, MacroUpdatedEventHandler);
            }
        }
    }
}
