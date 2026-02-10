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
            ClearAlbumButton = new PictureBox();
            AlbumList = new ComboBox();
            label1 = new Label();
            ProjectSearch = new Cubase.Hub.Controls.MainFormControls.ProjectsControl.Search.ProjectSearch();
            SeperatorPanel = new Panel();
            DataPanel = new Panel();
            RefreshProjectsButton = new Button();
            MenuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ClearAlbumButton).BeginInit();
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
            MenuPanel.Controls.Add(RefreshProjectsButton);
            MenuPanel.Controls.Add(ClearAlbumButton);
            MenuPanel.Controls.Add(AlbumList);
            MenuPanel.Controls.Add(label1);
            MenuPanel.Controls.Add(ProjectSearch);
            MenuPanel.Dock = DockStyle.Top;
            MenuPanel.Location = new Point(125, 0);
            MenuPanel.Name = "MenuPanel";
            MenuPanel.Size = new Size(884, 79);
            MenuPanel.TabIndex = 3;
            // 
            // ClearAlbumButton
            // 
            ClearAlbumButton.Cursor = Cursors.Hand;
            ClearAlbumButton.Image = Properties.Resources.close;
            ClearAlbumButton.Location = new Point(264, 31);
            ClearAlbumButton.Name = "ClearAlbumButton";
            ClearAlbumButton.Size = new Size(24, 24);
            ClearAlbumButton.TabIndex = 3;
            ClearAlbumButton.TabStop = false;
            // 
            // AlbumList
            // 
            AlbumList.FormattingEnabled = true;
            AlbumList.Location = new Point(19, 31);
            AlbumList.Name = "AlbumList";
            AlbumList.Size = new Size(239, 28);
            AlbumList.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(19, 8);
            label1.Name = "label1";
            label1.Size = new Size(56, 20);
            label1.TabIndex = 1;
            label1.Text = "Album";
            // 
            // ProjectSearch
            // 
            ProjectSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ProjectSearch.Location = new Point(337, 8);
            ProjectSearch.Name = "ProjectSearch";
            ProjectSearch.Size = new Size(321, 65);
            ProjectSearch.TabIndex = 0;
            // 
            // SeperatorPanel
            // 
            SeperatorPanel.Dock = DockStyle.Top;
            SeperatorPanel.Location = new Point(125, 79);
            SeperatorPanel.Name = "SeperatorPanel";
            SeperatorPanel.Size = new Size(884, 10);
            SeperatorPanel.TabIndex = 4;
            // 
            // DataPanel
            // 
            DataPanel.Dock = DockStyle.Fill;
            DataPanel.Location = new Point(125, 89);
            DataPanel.Name = "DataPanel";
            DataPanel.Size = new Size(884, 356);
            DataPanel.TabIndex = 5;
            // 
            // RefreshProjectsButton
            // 
            RefreshProjectsButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RefreshProjectsButton.Image = Properties.Resources.refresh;
            RefreshProjectsButton.ImageAlign = ContentAlignment.MiddleLeft;
            RefreshProjectsButton.Location = new Point(689, 28);
            RefreshProjectsButton.Name = "RefreshProjectsButton";
            RefreshProjectsButton.Size = new Size(176, 29);
            RefreshProjectsButton.TabIndex = 4;
            RefreshProjectsButton.Text = "Refresh Tracks";
            RefreshProjectsButton.UseVisualStyleBackColor = true;
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
            Size = new Size(1009, 445);
            MenuPanel.ResumeLayout(false);
            MenuPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ClearAlbumButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel IndexPanel;
        private Panel MenuPanel;
        private Panel SeperatorPanel;
        private Panel DataPanel;
        private MainFormControls.ProjectsControl.Search.ProjectSearch ProjectSearch;
        private ComboBox AlbumList;
        private Label label1;
        private PictureBox ClearAlbumButton;
        private Button RefreshProjectsButton;
    }
}
