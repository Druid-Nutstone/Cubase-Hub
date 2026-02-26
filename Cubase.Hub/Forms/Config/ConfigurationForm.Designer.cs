namespace Cubase.Hub.Forms.Config
{
    partial class ConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            panel1 = new Panel();
            ButtonSave = new Button();
            label1 = new Label();
            SourceCubaseFolders = new TextBox();
            AddSourceFolderButton = new Button();
            panel2 = new Panel();
            AlbumExportLocation = new TextBox();
            label5 = new Label();
            BrowseCubaseTemplateButton = new Button();
            CubaseTemplateLocation = new TextBox();
            label4 = new Label();
            BrowseUserTemplateLocationButton = new Button();
            CubaseUserTemplateLocation = new TextBox();
            label3 = new Label();
            BrowseCubaseExeButton = new Button();
            CubaseExeLocation = new TextBox();
            label2 = new Label();
            BrowseAlbumExportLocation = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(ButtonSave);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 483);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 52);
            panel1.TabIndex = 0;
            // 
            // ButtonSave
            // 
            ButtonSave.Location = new Point(12, 11);
            ButtonSave.Name = "ButtonSave";
            ButtonSave.Size = new Size(94, 29);
            ButtonSave.TabIndex = 0;
            ButtonSave.Text = "Save";
            ButtonSave.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(22, 27);
            label1.Name = "label1";
            label1.Size = new Size(165, 20);
            label1.TabIndex = 1;
            label1.Text = "Source Cubase Folders";
            // 
            // SourceCubaseFolders
            // 
            SourceCubaseFolders.Location = new Point(22, 50);
            SourceCubaseFolders.Name = "SourceCubaseFolders";
            SourceCubaseFolders.Size = new Size(666, 27);
            SourceCubaseFolders.TabIndex = 2;
            // 
            // AddSourceFolderButton
            // 
            AddSourceFolderButton.Location = new Point(694, 50);
            AddSourceFolderButton.Name = "AddSourceFolderButton";
            AddSourceFolderButton.Size = new Size(94, 29);
            AddSourceFolderButton.TabIndex = 3;
            AddSourceFolderButton.Text = "Add";
            AddSourceFolderButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(BrowseAlbumExportLocation);
            panel2.Controls.Add(AlbumExportLocation);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(BrowseCubaseTemplateButton);
            panel2.Controls.Add(CubaseTemplateLocation);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(BrowseUserTemplateLocationButton);
            panel2.Controls.Add(CubaseUserTemplateLocation);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(BrowseCubaseExeButton);
            panel2.Controls.Add(CubaseExeLocation);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(AddSourceFolderButton);
            panel2.Controls.Add(SourceCubaseFolders);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 483);
            panel2.TabIndex = 4;
            // 
            // AlbumExportLocation
            // 
            AlbumExportLocation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            AlbumExportLocation.Location = new Point(22, 409);
            AlbumExportLocation.Name = "AlbumExportLocation";
            AlbumExportLocation.Size = new Size(666, 27);
            AlbumExportLocation.TabIndex = 14;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(22, 386);
            label5.Name = "label5";
            label5.Size = new Size(174, 20);
            label5.TabIndex = 13;
            label5.Text = "Album Export Location ";
            // 
            // BrowseCubaseTemplateButton
            // 
            BrowseCubaseTemplateButton.Location = new Point(694, 317);
            BrowseCubaseTemplateButton.Name = "BrowseCubaseTemplateButton";
            BrowseCubaseTemplateButton.Size = new Size(94, 29);
            BrowseCubaseTemplateButton.TabIndex = 12;
            BrowseCubaseTemplateButton.Text = "Browse";
            BrowseCubaseTemplateButton.UseVisualStyleBackColor = true;
            // 
            // CubaseTemplateLocation
            // 
            CubaseTemplateLocation.Location = new Point(22, 317);
            CubaseTemplateLocation.Name = "CubaseTemplateLocation";
            CubaseTemplateLocation.Size = new Size(666, 27);
            CubaseTemplateLocation.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(22, 294);
            label4.Name = "label4";
            label4.Size = new Size(192, 20);
            label4.TabIndex = 10;
            label4.Text = "Cubase Template Location";
            // 
            // BrowseUserTemplateLocationButton
            // 
            BrowseUserTemplateLocationButton.Location = new Point(694, 218);
            BrowseUserTemplateLocationButton.Name = "BrowseUserTemplateLocationButton";
            BrowseUserTemplateLocationButton.Size = new Size(94, 29);
            BrowseUserTemplateLocationButton.TabIndex = 9;
            BrowseUserTemplateLocationButton.Text = "Browse";
            BrowseUserTemplateLocationButton.UseVisualStyleBackColor = true;
            // 
            // CubaseUserTemplateLocation
            // 
            CubaseUserTemplateLocation.Location = new Point(22, 218);
            CubaseUserTemplateLocation.Name = "CubaseUserTemplateLocation";
            CubaseUserTemplateLocation.Size = new Size(666, 27);
            CubaseUserTemplateLocation.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(22, 195);
            label3.Name = "label3";
            label3.Size = new Size(228, 20);
            label3.TabIndex = 7;
            label3.Text = "Cubase User Template Location";
            // 
            // BrowseCubaseExeButton
            // 
            BrowseCubaseExeButton.Location = new Point(694, 131);
            BrowseCubaseExeButton.Name = "BrowseCubaseExeButton";
            BrowseCubaseExeButton.Size = new Size(94, 29);
            BrowseCubaseExeButton.TabIndex = 6;
            BrowseCubaseExeButton.Text = "Browse";
            BrowseCubaseExeButton.UseVisualStyleBackColor = true;
            // 
            // CubaseExeLocation
            // 
            CubaseExeLocation.Location = new Point(22, 131);
            CubaseExeLocation.Name = "CubaseExeLocation";
            CubaseExeLocation.Size = new Size(666, 27);
            CubaseExeLocation.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(22, 108);
            label2.Name = "label2";
            label2.Size = new Size(151, 20);
            label2.TabIndex = 4;
            label2.Text = "Cubase Exe Location";
            // 
            // BrowseAlbumExportLocation
            // 
            BrowseAlbumExportLocation.Location = new Point(694, 409);
            BrowseAlbumExportLocation.Name = "BrowseAlbumExportLocation";
            BrowseAlbumExportLocation.Size = new Size(94, 29);
            BrowseAlbumExportLocation.TabIndex = 15;
            BrowseAlbumExportLocation.Text = "Browse";
            BrowseAlbumExportLocation.UseVisualStyleBackColor = true;
            // 
            // ConfigurationForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 535);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ConfigurationForm";
            Text = "ConfigurationForm";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button ButtonSave;
        private Label label1;
        private TextBox SourceCubaseFolders;
        private Button AddSourceFolderButton;
        private Panel panel2;
        private Label label2;
        private Button BrowseCubaseExeButton;
        private TextBox CubaseExeLocation;
        private Label label3;
        private Button BrowseUserTemplateLocationButton;
        private TextBox CubaseUserTemplateLocation;
        private Button BrowseCubaseTemplateButton;
        private TextBox CubaseTemplateLocation;
        private Label label4;
        private Label label5;
        private TextBox AlbumExportLocation;
        private Button BrowseAlbumExportLocation;
    }
}