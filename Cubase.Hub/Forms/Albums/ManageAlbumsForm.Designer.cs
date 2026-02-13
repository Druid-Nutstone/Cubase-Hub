namespace Cubase.Hub.Forms.Albums
{
    partial class ManageAlbumsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageAlbumsForm));
            TopPanel = new Panel();
            OpenAlbumDirectory = new Button();
            SelectedAlbum = new ComboBox();
            label1 = new Label();
            AlbumPanel = new Panel();
            AlbumConfigurationControl = new Cubase.Hub.Controls.Album.AlbumConfigurationControl();
            TracksPanel = new Panel();
            MixDownPanel = new Panel();
            mixdownControl = new Cubase.Hub.Controls.Album.Manage.MixdownControl();
            TracksControlPanel = new Panel();
            SetSelectedTracksTitleButton = new Button();
            SelectDeselectAllMixes = new CheckBox();
            DeleteSelectedButton = new Button();
            ManageMixesButton = new Button();
            RereshFromAblumButton = new Button();
            TopPanel.SuspendLayout();
            AlbumPanel.SuspendLayout();
            TracksPanel.SuspendLayout();
            MixDownPanel.SuspendLayout();
            TracksControlPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TopPanel
            // 
            TopPanel.Controls.Add(OpenAlbumDirectory);
            TopPanel.Controls.Add(SelectedAlbum);
            TopPanel.Controls.Add(label1);
            TopPanel.Dock = DockStyle.Top;
            TopPanel.Location = new Point(0, 0);
            TopPanel.Name = "TopPanel";
            TopPanel.Size = new Size(1382, 102);
            TopPanel.TabIndex = 0;
            // 
            // OpenAlbumDirectory
            // 
            OpenAlbumDirectory.Location = new Point(300, 41);
            OpenAlbumDirectory.Name = "OpenAlbumDirectory";
            OpenAlbumDirectory.Size = new Size(168, 29);
            OpenAlbumDirectory.TabIndex = 2;
            OpenAlbumDirectory.Text = "Open Album Folder";
            OpenAlbumDirectory.UseVisualStyleBackColor = true;
            // 
            // SelectedAlbum
            // 
            SelectedAlbum.FormattingEnabled = true;
            SelectedAlbum.Location = new Point(24, 42);
            SelectedAlbum.Name = "SelectedAlbum";
            SelectedAlbum.Size = new Size(255, 28);
            SelectedAlbum.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(24, 19);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 0;
            label1.Text = "Select Album";
            // 
            // AlbumPanel
            // 
            AlbumPanel.Controls.Add(AlbumConfigurationControl);
            AlbumPanel.Dock = DockStyle.Top;
            AlbumPanel.Location = new Point(0, 102);
            AlbumPanel.Name = "AlbumPanel";
            AlbumPanel.Size = new Size(1382, 248);
            AlbumPanel.TabIndex = 1;
            // 
            // AlbumConfigurationControl
            // 
            AlbumConfigurationControl.Dock = DockStyle.Top;
            AlbumConfigurationControl.Location = new Point(0, 0);
            AlbumConfigurationControl.Name = "AlbumConfigurationControl";
            AlbumConfigurationControl.Size = new Size(1382, 248);
            AlbumConfigurationControl.TabIndex = 0;
            // 
            // TracksPanel
            // 
            TracksPanel.Controls.Add(MixDownPanel);
            TracksPanel.Controls.Add(TracksControlPanel);
            TracksPanel.Dock = DockStyle.Fill;
            TracksPanel.Location = new Point(0, 350);
            TracksPanel.Name = "TracksPanel";
            TracksPanel.Size = new Size(1382, 353);
            TracksPanel.TabIndex = 2;
            // 
            // MixDownPanel
            // 
            MixDownPanel.AutoScroll = true;
            MixDownPanel.Controls.Add(mixdownControl);
            MixDownPanel.Dock = DockStyle.Fill;
            MixDownPanel.Location = new Point(0, 43);
            MixDownPanel.Name = "MixDownPanel";
            MixDownPanel.Size = new Size(1382, 310);
            MixDownPanel.TabIndex = 1;
            // 
            // mixdownControl
            // 
            mixdownControl.AutoSize = true;
            mixdownControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mixdownControl.ColumnCount = 2;
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mixdownControl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mixdownControl.Dock = DockStyle.Top;
            mixdownControl.Location = new Point(0, 0);
            mixdownControl.Name = "mixdownControl";
            mixdownControl.Padding = new Padding(10);
            mixdownControl.RowCount = 2;
            mixdownControl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            mixdownControl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            mixdownControl.Size = new Size(1382, 20);
            mixdownControl.TabIndex = 0;
            // 
            // TracksControlPanel
            // 
            TracksControlPanel.Controls.Add(SetSelectedTracksTitleButton);
            TracksControlPanel.Controls.Add(SelectDeselectAllMixes);
            TracksControlPanel.Controls.Add(DeleteSelectedButton);
            TracksControlPanel.Controls.Add(ManageMixesButton);
            TracksControlPanel.Controls.Add(RereshFromAblumButton);
            TracksControlPanel.Dock = DockStyle.Top;
            TracksControlPanel.Location = new Point(0, 0);
            TracksControlPanel.Name = "TracksControlPanel";
            TracksControlPanel.Size = new Size(1382, 43);
            TracksControlPanel.TabIndex = 0;
            // 
            // SetSelectedTracksTitleButton
            // 
            SetSelectedTracksTitleButton.Location = new Point(602, 8);
            SetSelectedTracksTitleButton.Name = "SetSelectedTracksTitleButton";
            SetSelectedTracksTitleButton.Size = new Size(265, 29);
            SetSelectedTracksTitleButton.TabIndex = 5;
            SetSelectedTracksTitleButton.Text = "Set Selected Titles To Track Name";
            SetSelectedTracksTitleButton.UseVisualStyleBackColor = true;
            // 
            // SelectDeselectAllMixes
            // 
            SelectDeselectAllMixes.AutoSize = true;
            SelectDeselectAllMixes.Location = new Point(12, 11);
            SelectDeselectAllMixes.Name = "SelectDeselectAllMixes";
            SelectDeselectAllMixes.Size = new Size(128, 24);
            SelectDeselectAllMixes.TabIndex = 4;
            SelectDeselectAllMixes.Text = "SelectDeselect";
            SelectDeselectAllMixes.UseVisualStyleBackColor = true;
            // 
            // DeleteSelectedButton
            // 
            DeleteSelectedButton.BackColor = Color.IndianRed;
            DeleteSelectedButton.Location = new Point(431, 8);
            DeleteSelectedButton.Name = "DeleteSelectedButton";
            DeleteSelectedButton.Size = new Size(156, 29);
            DeleteSelectedButton.TabIndex = 3;
            DeleteSelectedButton.Text = "Delete Selected";
            DeleteSelectedButton.UseVisualStyleBackColor = false;
            // 
            // ManageMixesButton
            // 
            ManageMixesButton.Location = new Point(146, 8);
            ManageMixesButton.Name = "ManageMixesButton";
            ManageMixesButton.Size = new Size(266, 29);
            ManageMixesButton.TabIndex = 2;
            ManageMixesButton.Text = "Manage Mixes (Convert, Copy etc)";
            ManageMixesButton.UseVisualStyleBackColor = true;
            // 
            // RereshFromAblumButton
            // 
            RereshFromAblumButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RereshFromAblumButton.Location = new Point(1101, 8);
            RereshFromAblumButton.Name = "RereshFromAblumButton";
            RereshFromAblumButton.Size = new Size(269, 29);
            RereshFromAblumButton.TabIndex = 0;
            RereshFromAblumButton.Text = "Refresh Tags From Album";
            RereshFromAblumButton.UseVisualStyleBackColor = true;
            // 
            // ManageAlbumsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1382, 703);
            Controls.Add(TracksPanel);
            Controls.Add(AlbumPanel);
            Controls.Add(TopPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ManageAlbumsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage Albums";
            TopPanel.ResumeLayout(false);
            TopPanel.PerformLayout();
            AlbumPanel.ResumeLayout(false);
            TracksPanel.ResumeLayout(false);
            MixDownPanel.ResumeLayout(false);
            MixDownPanel.PerformLayout();
            TracksControlPanel.ResumeLayout(false);
            TracksControlPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel TopPanel;
        private Label label1;
        private Panel AlbumPanel;
        private Panel TracksPanel;
        private ComboBox SelectedAlbum;
        private Controls.Album.AlbumConfigurationControl AlbumConfigurationControl;
        private Panel MixDownPanel;
        private Panel TracksControlPanel;
        private Controls.Album.Manage.MixdownControl mixdownControl;
        private Button OpenAlbumDirectory;
        private Button RereshFromAblumButton;
        private Button ManageMixesButton;
        private Button DeleteSelectedButton;
        private CheckBox SelectDeselectAllMixes;
        private Button SetSelectedTracksTitleButton;
    }
}