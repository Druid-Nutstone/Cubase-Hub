namespace Cubase.Hub.Forms
{
    partial class NewAlbumForm
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
            label2 = new Label();
            AlbumDetails = new GroupBox();
            panel1 = new Panel();
            CreateAlbumButton = new Button();
            SelectedExistingDirectory = new ComboBox();
            label7 = new Label();
            SelectedRootDirectory = new TextBox();
            BrowseRootDirectory = new Button();
            label8 = new Label();
            NewAlbumRoot = new Label();
            albumConfigurationControl = new Cubase.Hub.Controls.Album.AlbumConfigurationControl();
            AlbumDetails.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(23, 19);
            label2.Name = "label2";
            label2.Size = new Size(275, 20);
            label2.TabIndex = 2;
            label2.Text = "Select Existing Root Cubase Directory ";
            // 
            // AlbumDetails
            // 
            AlbumDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            AlbumDetails.Controls.Add(albumConfigurationControl);
            AlbumDetails.Location = new Point(22, 162);
            AlbumDetails.Name = "AlbumDetails";
            AlbumDetails.Size = new Size(766, 256);
            AlbumDetails.TabIndex = 3;
            AlbumDetails.TabStop = false;
            AlbumDetails.Text = "Album Details";
            // 
            // panel1
            // 
            panel1.Controls.Add(CreateAlbumButton);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 523);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 52);
            panel1.TabIndex = 4;
            // 
            // CreateAlbumButton
            // 
            CreateAlbumButton.Location = new Point(22, 11);
            CreateAlbumButton.Name = "CreateAlbumButton";
            CreateAlbumButton.Size = new Size(154, 29);
            CreateAlbumButton.TabIndex = 0;
            CreateAlbumButton.Text = "Create Album";
            CreateAlbumButton.UseVisualStyleBackColor = true;
            // 
            // SelectedExistingDirectory
            // 
            SelectedExistingDirectory.FormattingEnabled = true;
            SelectedExistingDirectory.Location = new Point(28, 40);
            SelectedExistingDirectory.Name = "SelectedExistingDirectory";
            SelectedExistingDirectory.Size = new Size(728, 28);
            SelectedExistingDirectory.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(23, 87);
            label7.Name = "label7";
            label7.Size = new Size(240, 20);
            label7.TabIndex = 6;
            label7.Text = "Or Select Specific Root Directory ";
            // 
            // SelectedRootDirectory
            // 
            SelectedRootDirectory.Location = new Point(29, 108);
            SelectedRootDirectory.Name = "SelectedRootDirectory";
            SelectedRootDirectory.Size = new Size(622, 27);
            SelectedRootDirectory.TabIndex = 7;
            // 
            // BrowseRootDirectory
            // 
            BrowseRootDirectory.Location = new Point(662, 106);
            BrowseRootDirectory.Name = "BrowseRootDirectory";
            BrowseRootDirectory.Size = new Size(94, 29);
            BrowseRootDirectory.TabIndex = 8;
            BrowseRootDirectory.Text = "Browse";
            BrowseRootDirectory.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(22, 446);
            label8.Name = "label8";
            label8.Size = new Size(143, 20);
            label8.TabIndex = 9;
            label8.Text = "Target Album Root";
            // 
            // NewAlbumRoot
            // 
            NewAlbumRoot.AutoSize = true;
            NewAlbumRoot.Location = new Point(28, 470);
            NewAlbumRoot.Name = "NewAlbumRoot";
            NewAlbumRoot.Size = new Size(100, 20);
            NewAlbumRoot.TabIndex = 10;
            NewAlbumRoot.Text = "Not Specified";
            // 
            // albumConfigurationControl
            // 
            albumConfigurationControl.Dock = DockStyle.Fill;
            albumConfigurationControl.Location = new Point(3, 23);
            albumConfigurationControl.Name = "albumConfigurationControl";
            albumConfigurationControl.Size = new Size(760, 230);
            albumConfigurationControl.TabIndex = 0;
            // 
            // NewAlbumForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 575);
            Controls.Add(NewAlbumRoot);
            Controls.Add(label8);
            Controls.Add(BrowseRootDirectory);
            Controls.Add(SelectedRootDirectory);
            Controls.Add(label7);
            Controls.Add(SelectedExistingDirectory);
            Controls.Add(panel1);
            Controls.Add(AlbumDetails);
            Controls.Add(label2);
            Name = "NewAlbumForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "New Album";
            AlbumDetails.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private GroupBox AlbumDetails;
        private Panel panel1;
        private Button CreateAlbumButton;
        private ComboBox SelectedExistingDirectory;
        private Label label7;
        private TextBox SelectedRootDirectory;
        private Button BrowseRootDirectory;
        private Label label8;
        private Label NewAlbumRoot;
        private Controls.Album.AlbumConfigurationControl albumConfigurationControl;
    }
}