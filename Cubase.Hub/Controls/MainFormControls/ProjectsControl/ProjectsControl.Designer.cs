namespace Cubase.Hub.Controls.MainFormControls.ProjectsForm
{
    partial class ProjectsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            IndexPanel = new Panel();
            MenuPanel = new Panel();
            ProjectSearch = new Cubase.Hub.Controls.MainFormControls.ProjectsControl.Search.ProjectSearch();
            SeperatorPanel = new Panel();
            DataPanel = new Panel();
            MenuPanel.SuspendLayout();
            SuspendLayout();
            // 
            // IndexPanel
            // 
            IndexPanel.Dock = DockStyle.Left;
            IndexPanel.Location = new Point(0, 0);
            IndexPanel.Name = "IndexPanel";
            IndexPanel.Size = new Size(125, 445);
            IndexPanel.TabIndex = 0;
            // 
            // MenuPanel
            // 
            MenuPanel.Controls.Add(ProjectSearch);
            MenuPanel.Dock = DockStyle.Top;
            MenuPanel.Location = new Point(125, 0);
            MenuPanel.Name = "MenuPanel";
            MenuPanel.Size = new Size(502, 51);
            MenuPanel.TabIndex = 3;
            // 
            // ProjectSearch
            // 
            ProjectSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ProjectSearch.Location = new Point(178, 6);
            ProjectSearch.Name = "ProjectSearch";
            ProjectSearch.Size = new Size(321, 39);
            ProjectSearch.TabIndex = 0;
            // 
            // SeperatorPanel
            // 
            SeperatorPanel.Dock = DockStyle.Top;
            SeperatorPanel.Location = new Point(125, 51);
            SeperatorPanel.Name = "SeperatorPanel";
            SeperatorPanel.Size = new Size(502, 10);
            SeperatorPanel.TabIndex = 4;
            // 
            // DataPanel
            // 
            DataPanel.Dock = DockStyle.Fill;
            DataPanel.Location = new Point(125, 61);
            DataPanel.Name = "DataPanel";
            DataPanel.Size = new Size(502, 384);
            DataPanel.TabIndex = 5;
            // 
            // ProjectsControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DataPanel);
            Controls.Add(SeperatorPanel);
            Controls.Add(MenuPanel);
            Controls.Add(IndexPanel);
            Name = "ProjectsControl";
            Size = new Size(627, 445);
            MenuPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel IndexPanel;
        private Panel MenuPanel;
        private Panel SeperatorPanel;
        private Panel DataPanel;
        private MainFormControls.ProjectsControl.Search.ProjectSearch ProjectSearch;
    }
}
