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
            TopPanel = new Panel();
            SelectedAlbum = new ComboBox();
            label1 = new Label();
            AlbumPanel = new Panel();
            AlbumConfigurationControl = new Cubase.Hub.Controls.Album.AlbumConfigurationControl();
            TracksPanel = new Panel();
            MixDownPanel = new Panel();
            mixdownControl = new Cubase.Hub.Controls.Album.Manage.MixdownControl();
            TracksControlPanel = new Panel();
            TopPanel.SuspendLayout();
            AlbumPanel.SuspendLayout();
            TracksPanel.SuspendLayout();
            MixDownPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TopPanel
            // 
            TopPanel.Controls.Add(SelectedAlbum);
            TopPanel.Controls.Add(label1);
            TopPanel.Dock = DockStyle.Top;
            TopPanel.Location = new Point(0, 0);
            TopPanel.Name = "TopPanel";
            TopPanel.Size = new Size(979, 102);
            TopPanel.TabIndex = 0;
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
            AlbumPanel.Size = new Size(979, 248);
            AlbumPanel.TabIndex = 1;
            // 
            // AlbumConfigurationControl
            // 
            AlbumConfigurationControl.Dock = DockStyle.Fill;
            AlbumConfigurationControl.Location = new Point(0, 0);
            AlbumConfigurationControl.Name = "AlbumConfigurationControl";
            AlbumConfigurationControl.Size = new Size(979, 248);
            AlbumConfigurationControl.TabIndex = 0;
            // 
            // TracksPanel
            // 
            TracksPanel.Controls.Add(MixDownPanel);
            TracksPanel.Controls.Add(TracksControlPanel);
            TracksPanel.Dock = DockStyle.Fill;
            TracksPanel.Location = new Point(0, 350);
            TracksPanel.Name = "TracksPanel";
            TracksPanel.Size = new Size(979, 255);
            TracksPanel.TabIndex = 2;
            // 
            // MixDownPanel
            // 
            MixDownPanel.AutoScroll = true;
            MixDownPanel.Controls.Add(mixdownControl);
            MixDownPanel.Dock = DockStyle.Fill;
            MixDownPanel.Location = new Point(0, 43);
            MixDownPanel.Name = "MixDownPanel";
            MixDownPanel.Size = new Size(979, 212);
            MixDownPanel.TabIndex = 1;
            // 
            // mixdownControl
            // 
            mixdownControl.AutoSize = true;
            mixdownControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mixdownControl.ColumnCount = 2;
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
            mixdownControl.Size = new Size(979, 20);
            mixdownControl.TabIndex = 0;
            // 
            // TracksControlPanel
            // 
            TracksControlPanel.Dock = DockStyle.Top;
            TracksControlPanel.Location = new Point(0, 0);
            TracksControlPanel.Name = "TracksControlPanel";
            TracksControlPanel.Size = new Size(979, 43);
            TracksControlPanel.TabIndex = 0;
            // 
            // ManageAlbumsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(979, 605);
            Controls.Add(TracksPanel);
            Controls.Add(AlbumPanel);
            Controls.Add(TopPanel);
            Name = "ManageAlbumsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage Albums";
            TopPanel.ResumeLayout(false);
            TopPanel.PerformLayout();
            AlbumPanel.ResumeLayout(false);
            TracksPanel.ResumeLayout(false);
            MixDownPanel.ResumeLayout(false);
            MixDownPanel.PerformLayout();
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
    }
}