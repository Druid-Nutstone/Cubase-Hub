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
            label1 = new Label();
            SourceFoldersListBox = new ListBox();
            label2 = new Label();
            AlbumDetails = new GroupBox();
            panel1 = new Panel();
            CreateAlbumButton = new Button();
            AlbumDetails.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(22, 33);
            label1.Name = "label1";
            label1.Size = new Size(176, 20);
            label1.TabIndex = 0;
            label1.Text = "Album Parent Directory";
            // 
            // SourceFoldersListBox
            // 
            SourceFoldersListBox.FormattingEnabled = true;
            SourceFoldersListBox.Location = new Point(23, 42);
            SourceFoldersListBox.Name = "SourceFoldersListBox";
            SourceFoldersListBox.Size = new Size(765, 104);
            SourceFoldersListBox.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(23, 19);
            label2.Name = "label2";
            label2.Size = new Size(174, 20);
            label2.TabIndex = 2;
            label2.Text = "Select Parent Directory ";
            // 
            // AlbumDetails
            // 
            AlbumDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            AlbumDetails.Controls.Add(label1);
            AlbumDetails.Location = new Point(22, 162);
            AlbumDetails.Name = "AlbumDetails";
            AlbumDetails.Size = new Size(766, 230);
            AlbumDetails.TabIndex = 3;
            AlbumDetails.TabStop = false;
            AlbumDetails.Text = "Album Details";
            // 
            // panel1
            // 
            panel1.Controls.Add(CreateAlbumButton);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 398);
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
            // NewAlbumForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(AlbumDetails);
            Controls.Add(label2);
            Controls.Add(SourceFoldersListBox);
            Name = "NewAlbumForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "New Album";
            AlbumDetails.ResumeLayout(false);
            AlbumDetails.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox SourceFoldersListBox;
        private Label label2;
        private GroupBox AlbumDetails;
        private Panel panel1;
        private Button CreateAlbumButton;
    }
}