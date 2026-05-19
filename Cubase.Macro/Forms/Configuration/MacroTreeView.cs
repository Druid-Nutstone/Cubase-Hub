using Cubase.Macro.Common.Models;
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

        private Action<string> nodeSelected;

        private CubaseMacroConfiguration cubaseMacroConfiguration;

        private string lastPath = string.Empty;

        public MacroTreeView(Panel DataPanel, Action MacroUpdatedEventHandler, Action<string> NodeSelected, CubaseMacroConfiguration cubaseMacroConfiguration, string lastNode)
        {
            this.lastPath = lastNode;
            this.nodeSelected = NodeSelected;
            this.Dock = DockStyle.Fill;
            ThemeApplier.ApplyDarkTheme(this);
            this.macroUpdatedEventHandler = MacroUpdatedEventHandler;
            this.nodeSelected = NodeSelected;
            this.dataPanel = DataPanel;
            this.cubaseMacroConfiguration = cubaseMacroConfiguration;   
        }

        public void Build(CubaseMacroCollection macros)
        {
            this.macros = macros;
            this.Nodes.Clear();
            this.Nodes.Add(new PrimaryMacroTreeNode(macros, dataPanel, macroUpdatedEventHandler)); 
            this.Nodes.Add(new CommonMacrosTreeNode(macros, dataPanel, macroUpdatedEventHandler));
            this.Nodes.Add(new ConfigurationTreeNode(this.cubaseMacroConfiguration));
            this.ExpandAll();
            if (!string.IsNullOrEmpty(lastPath))
            {
                var lastNode = FindLastNodeSelected(this.Nodes[0]);
                if (lastNode != null)
                {
                    lastNode.EnsureVisible();
                    return;
                }
                lastNode = FindLastNodeSelected(this.Nodes[1]);
                if (lastNode != null)
                {
                    lastNode.EnsureVisible();
                    return;
                }
                lastNode = FindLastNodeSelected(this.Nodes[2]);
                if (lastNode != null)
                {
                    lastNode.EnsureVisible();
                    return;
                }
            }
        }

        private TreeNode FindLastNodeSelected(TreeNode rootNode)
        {
            if (((BaseMacroTreeNode)rootNode).IsItMe(this.lastPath))
            {
                return rootNode;
            }
            else
            {
                foreach (TreeNode node in rootNode.Nodes)
                {
                    return FindLastNodeSelected(node);
                }
            }
            return null;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Right)
            {
                var nodeAtCursor = this.GetNodeAt(new Point(e.X, e.Y));
                if (nodeAtCursor != null)
                {
                    this.lastPath = nodeAtCursor.FullPath;
                    this.SetLastNode();
                }
            }
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
            this.lastPath = e.Node.FullPath;
            this.SetLastNode();
        }

        private void SetLastNode()
        {
            this.nodeSelected?.Invoke(this.lastPath);
        }

        public class BaseMacroTreeNode : TreeNode
        {
            public BaseMacroTreeNode()
            {

            }
        
            public bool IsItMe(string lastPath)
            {
                return lastPath == this.FullPath;
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

        public class CommonMacrosTreeNode : BaseMacroTreeNode
        {
            public CommonMacrosTreeNode(CubaseMacroCollection macros, Panel DataPanel, Action MacroUpdatedEventHandler)
            {
                this.Text = "Common Macros";
                foreach (var macro in macros.CommonMacros)
                {
                    this.Nodes.Add(new KeyCommandMacroTreeNode(macro, macros, DataPanel, MacroUpdatedEventHandler));
                }
                this.ContextMenuStrip = new MacroTreeViewContentMenu(macros, DataPanel, MacroUpdatedEventHandler);
            }
        }

        public class PrimaryMacroTreeNode : BaseMacroTreeNode
        {
            public PrimaryMacroTreeNode(CubaseMacroCollection macros, Panel DataPanel, Action MacroUpdatedEventHandler)
            {
                this.Text = "Macros";
                foreach (var macro in macros.Macros)
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
