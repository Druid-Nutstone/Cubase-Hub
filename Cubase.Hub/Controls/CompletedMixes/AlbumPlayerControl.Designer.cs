namespace Cubase.Hub.Controls.CompletedMixes
{
    partial class AlbumPlayerControl
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
            TopPanel = new Panel();
            AlbumProducer = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            AlbumEngineer = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            label7 = new Label();
            label6 = new Label();
            CommandPanel = new Panel();
            DistributerPanel = new Panel();
            SelectAllTracks = new CheckBox();
            PlayAllButton = new Button();
            AlbumComments = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            AlbumGenre = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            label5 = new Label();
            label4 = new Label();
            AlbumYear = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            label3 = new Label();
            AlbumArtist = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            label2 = new Label();
            AlbumArt = new Cubase.Hub.Controls.Media.AlbumCover.AlbumCoverControl();
            AlbumTitle = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            label1 = new Label();
            DataPanel = new Panel();
            TrackPanel = new Panel();
            TrackPlayView = new Cubase.Hub.Controls.CompletedMixes.Tracks.TrackPlayView();
            PlayerPanel = new Panel();
            PlayTrackControl = new Cubase.Hub.Controls.Media.Play.PlayTrackControl();
            label8 = new Label();
            AlbumLabel = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            TopPanel.SuspendLayout();
            CommandPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AlbumArt).BeginInit();
            DataPanel.SuspendLayout();
            TrackPanel.SuspendLayout();
            PlayerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TopPanel
            // 
            TopPanel.Controls.Add(AlbumLabel);
            TopPanel.Controls.Add(label8);
            TopPanel.Controls.Add(AlbumProducer);
            TopPanel.Controls.Add(AlbumEngineer);
            TopPanel.Controls.Add(label7);
            TopPanel.Controls.Add(label6);
            TopPanel.Controls.Add(CommandPanel);
            TopPanel.Controls.Add(AlbumComments);
            TopPanel.Controls.Add(AlbumGenre);
            TopPanel.Controls.Add(label5);
            TopPanel.Controls.Add(label4);
            TopPanel.Controls.Add(AlbumYear);
            TopPanel.Controls.Add(label3);
            TopPanel.Controls.Add(AlbumArtist);
            TopPanel.Controls.Add(label2);
            TopPanel.Controls.Add(AlbumArt);
            TopPanel.Controls.Add(AlbumTitle);
            TopPanel.Controls.Add(label1);
            TopPanel.Dock = DockStyle.Top;
            TopPanel.Location = new Point(0, 0);
            TopPanel.Name = "TopPanel";
            TopPanel.Size = new Size(908, 176);
            TopPanel.TabIndex = 0;
            // 
            // AlbumProducer
            // 
            AlbumProducer.AutoSize = true;
            AlbumProducer.Location = new Point(678, 34);
            AlbumProducer.Name = "AlbumProducer";
            AlbumProducer.Size = new Size(112, 20);
            AlbumProducer.TabIndex = 16;
            AlbumProducer.Text = "AlbumProducer";
            // 
            // AlbumEngineer
            // 
            AlbumEngineer.AutoSize = true;
            AlbumEngineer.Location = new Point(555, 34);
            AlbumEngineer.Name = "AlbumEngineer";
            AlbumEngineer.Size = new Size(111, 20);
            AlbumEngineer.TabIndex = 15;
            AlbumEngineer.Text = "AlbumEngineer";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(678, 14);
            label7.Name = "label7";
            label7.Size = new Size(72, 20);
            label7.TabIndex = 14;
            label7.Text = "Producer";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(555, 14);
            label6.Name = "label6";
            label6.Size = new Size(70, 20);
            label6.TabIndex = 13;
            label6.Text = "Engineer";
            // 
            // CommandPanel
            // 
            CommandPanel.Controls.Add(DistributerPanel);
            CommandPanel.Controls.Add(SelectAllTracks);
            CommandPanel.Controls.Add(PlayAllButton);
            CommandPanel.Dock = DockStyle.Bottom;
            CommandPanel.Location = new Point(0, 124);
            CommandPanel.Name = "CommandPanel";
            CommandPanel.Size = new Size(908, 52);
            CommandPanel.TabIndex = 12;
            // 
            // DistributerPanel
            // 
            DistributerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            DistributerPanel.Location = new Point(294, 5);
            DistributerPanel.Name = "DistributerPanel";
            DistributerPanel.Size = new Size(456, 41);
            DistributerPanel.TabIndex = 3;
            // 
            // SelectAllTracks
            // 
            SelectAllTracks.AutoSize = true;
            SelectAllTracks.Location = new Point(33, 17);
            SelectAllTracks.Name = "SelectAllTracks";
            SelectAllTracks.Size = new Size(93, 24);
            SelectAllTracks.TabIndex = 2;
            SelectAllTracks.Text = "Select All";
            SelectAllTracks.UseVisualStyleBackColor = true;
            // 
            // PlayAllButton
            // 
            PlayAllButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PlayAllButton.Image = Properties.Resources.Play;
            PlayAllButton.ImageAlign = ContentAlignment.MiddleLeft;
            PlayAllButton.Location = new Point(769, 5);
            PlayAllButton.Name = "PlayAllButton";
            PlayAllButton.Size = new Size(125, 41);
            PlayAllButton.TabIndex = 1;
            PlayAllButton.Text = "Play All";
            PlayAllButton.UseVisualStyleBackColor = true;
            // 
            // AlbumComments
            // 
            AlbumComments.AutoSize = true;
            AlbumComments.Location = new Point(555, 94);
            AlbumComments.Name = "AlbumComments";
            AlbumComments.Size = new Size(124, 20);
            AlbumComments.TabIndex = 11;
            AlbumComments.Text = "AlbumComments";
            // 
            // AlbumGenre
            // 
            AlbumGenre.AutoSize = true;
            AlbumGenre.Location = new Point(193, 94);
            AlbumGenre.Name = "AlbumGenre";
            AlbumGenre.Size = new Size(92, 20);
            AlbumGenre.TabIndex = 10;
            AlbumGenre.Text = "AlbumGenre";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(555, 74);
            label5.Name = "label5";
            label5.Size = new Size(85, 20);
            label5.TabIndex = 9;
            label5.Text = "Comments";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(193, 74);
            label4.Name = "label4";
            label4.Size = new Size(51, 20);
            label4.TabIndex = 8;
            label4.Text = "Genre";
            // 
            // AlbumYear
            // 
            AlbumYear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AlbumYear.AutoSize = true;
            AlbumYear.Location = new Point(813, 34);
            AlbumYear.Name = "AlbumYear";
            AlbumYear.Size = new Size(81, 20);
            AlbumYear.TabIndex = 7;
            AlbumYear.Text = "AlbumYear";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(813, 14);
            label3.Name = "label3";
            label3.Size = new Size(39, 20);
            label3.TabIndex = 6;
            label3.Text = "Year";
            // 
            // AlbumArtist
            // 
            AlbumArtist.AutoSize = true;
            AlbumArtist.Location = new Point(402, 34);
            AlbumArtist.Name = "AlbumArtist";
            AlbumArtist.Size = new Size(88, 20);
            AlbumArtist.TabIndex = 5;
            AlbumArtist.Text = "AlbumArtist";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(402, 14);
            label2.Name = "label2";
            label2.Size = new Size(49, 20);
            label2.TabIndex = 4;
            label2.Text = "Artist";
            // 
            // AlbumArt
            // 
            AlbumArt.ImageLocation = "C:\\Users\\david\\AppData\\Local\\Microsoft\\VisualStudio\\18.0_113ede53\\WinFormsDesigner\\hrmdh3et.3up\\NoImage.png";
            AlbumArt.Location = new Point(33, 14);
            AlbumArt.Name = "AlbumArt";
            AlbumArt.Size = new Size(104, 104);
            AlbumArt.SizeMode = PictureBoxSizeMode.StretchImage;
            AlbumArt.TabIndex = 3;
            AlbumArt.TabStop = false;
            // 
            // AlbumTitle
            // 
            AlbumTitle.AutoSize = true;
            AlbumTitle.Location = new Point(193, 34);
            AlbumTitle.Name = "AlbumTitle";
            AlbumTitle.Size = new Size(82, 20);
            AlbumTitle.TabIndex = 2;
            AlbumTitle.Text = "AlbumTitle";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(193, 14);
            label1.Name = "label1";
            label1.Size = new Size(91, 20);
            label1.TabIndex = 1;
            label1.Text = "Album Title";
            // 
            // DataPanel
            // 
            DataPanel.Controls.Add(TrackPanel);
            DataPanel.Controls.Add(PlayerPanel);
            DataPanel.Dock = DockStyle.Fill;
            DataPanel.Location = new Point(0, 176);
            DataPanel.Name = "DataPanel";
            DataPanel.Size = new Size(908, 201);
            DataPanel.TabIndex = 1;
            // 
            // TrackPanel
            // 
            TrackPanel.AutoScroll = true;
            TrackPanel.Controls.Add(TrackPlayView);
            TrackPanel.Dock = DockStyle.Fill;
            TrackPanel.Location = new Point(0, 0);
            TrackPanel.Name = "TrackPanel";
            TrackPanel.Size = new Size(908, 88);
            TrackPanel.TabIndex = 1;
            // 
            // TrackPlayView
            // 
            TrackPlayView.AutoSize = true;
            TrackPlayView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            TrackPlayView.ColumnCount = 2;
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TrackPlayView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TrackPlayView.Dock = DockStyle.Top;
            TrackPlayView.Location = new Point(0, 0);
            TrackPlayView.Name = "TrackPlayView";
            TrackPlayView.Padding = new Padding(10);
            TrackPlayView.RowCount = 2;
            TrackPlayView.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TrackPlayView.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            TrackPlayView.Size = new Size(908, 20);
            TrackPlayView.TabIndex = 0;
            // 
            // PlayerPanel
            // 
            PlayerPanel.Controls.Add(PlayTrackControl);
            PlayerPanel.Dock = DockStyle.Bottom;
            PlayerPanel.Location = new Point(0, 88);
            PlayerPanel.Name = "PlayerPanel";
            PlayerPanel.Size = new Size(908, 113);
            PlayerPanel.TabIndex = 0;
            // 
            // PlayTrackControl
            // 
            PlayTrackControl.Dock = DockStyle.Fill;
            PlayTrackControl.Location = new Point(0, 0);
            PlayTrackControl.Name = "PlayTrackControl";
            PlayTrackControl.Size = new Size(908, 113);
            PlayTrackControl.TabIndex = 0;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(402, 74);
            label8.Name = "label8";
            label8.Size = new Size(46, 20);
            label8.TabIndex = 17;
            label8.Text = "Label";
            // 
            // AlbumLabel
            // 
            AlbumLabel.AutoSize = true;
            AlbumLabel.Location = new Point(402, 94);
            AlbumLabel.Name = "AlbumLabel";
            AlbumLabel.Size = new Size(89, 20);
            AlbumLabel.TabIndex = 18;
            AlbumLabel.Text = "AlbumLabel";
            // 
            // AlbumPlayerControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DataPanel);
            Controls.Add(TopPanel);
            Name = "AlbumPlayerControl";
            Size = new Size(908, 377);
            TopPanel.ResumeLayout(false);
            TopPanel.PerformLayout();
            CommandPanel.ResumeLayout(false);
            CommandPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)AlbumArt).EndInit();
            DataPanel.ResumeLayout(false);
            TrackPanel.ResumeLayout(false);
            TrackPanel.PerformLayout();
            PlayerPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel TopPanel;
        private Panel DataPanel;
        private BoundControls.BoundLabel AlbumTitle;
        private Label label1;
        private Media.AlbumCover.AlbumCoverControl AlbumArt;
        private BoundControls.BoundLabel AlbumArtist;
        private Label label2;
        private Label label3;
        private BoundControls.BoundLabel AlbumYear;
        private BoundControls.BoundLabel AlbumGenre;
        private Label label5;
        private Label label4;
        private BoundControls.BoundLabel AlbumComments;
        private Panel CommandPanel;
        private Panel TrackPanel;
        private Panel PlayerPanel;
        private Media.Play.PlayTrackControl PlayTrackControl;
        private Tracks.TrackPlayView TrackPlayView;
        private Button PlayAllButton;
        private CheckBox SelectAllTracks;
        private Panel DistributerPanel;
        private BoundControls.BoundLabel AlbumProducer;
        private BoundControls.BoundLabel AlbumEngineer;
        private Label label7;
        private Label label6;
        private BoundControls.BoundLabel AlbumLabel;
        private Label label8;
    }
}
