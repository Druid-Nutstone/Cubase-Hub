namespace Cubase.Hub.Controls.Album
{
    partial class AlbumConfigurationControl
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
            AlbumGenre = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            AlbumComments = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            label6 = new Label();
            label5 = new Label();
            AlbumYear = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            label4 = new Label();
            AlbumArtist = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            AlbumTitle = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            label3 = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // AlbumGenre
            // 
            AlbumGenre.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AlbumGenre.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AlbumGenre.Location = new Point(474, 115);
            AlbumGenre.Name = "AlbumGenre";
            AlbumGenre.Size = new Size(255, 27);
            AlbumGenre.TabIndex = 21;
            // 
            // AlbumComments
            // 
            AlbumComments.Location = new Point(20, 189);
            AlbumComments.Name = "AlbumComments";
            AlbumComments.Size = new Size(709, 27);
            AlbumComments.TabIndex = 20;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(17, 166);
            label6.Name = "label6";
            label6.Size = new Size(136, 20);
            label6.TabIndex = 19;
            label6.Text = "Album Comments";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(471, 92);
            label5.Name = "label5";
            label5.Size = new Size(102, 20);
            label5.TabIndex = 18;
            label5.Text = "Album Genre";
            // 
            // AlbumYear
            // 
            AlbumYear.Location = new Point(20, 115);
            AlbumYear.Name = "AlbumYear";
            AlbumYear.Size = new Size(125, 27);
            AlbumYear.TabIndex = 17;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(20, 92);
            label4.Name = "label4";
            label4.Size = new Size(90, 20);
            label4.TabIndex = 16;
            label4.Text = "Album Year";
            // 
            // AlbumArtist
            // 
            AlbumArtist.Location = new Point(471, 39);
            AlbumArtist.Name = "AlbumArtist";
            AlbumArtist.Size = new Size(258, 27);
            AlbumArtist.TabIndex = 15;
            // 
            // AlbumTitle
            // 
            AlbumTitle.Location = new Point(20, 38);
            AlbumTitle.Name = "AlbumTitle";
            AlbumTitle.Size = new Size(409, 27);
            AlbumTitle.TabIndex = 14;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(471, 16);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 13;
            label3.Text = "Artist";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(17, 16);
            label1.Name = "label1";
            label1.Size = new Size(91, 20);
            label1.TabIndex = 12;
            label1.Text = "Album Title";
            // 
            // AlbumConfigurationControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(AlbumGenre);
            Controls.Add(AlbumComments);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(AlbumYear);
            Controls.Add(label4);
            Controls.Add(AlbumArtist);
            Controls.Add(AlbumTitle);
            Controls.Add(label3);
            Controls.Add(label1);
            Name = "AlbumConfigurationControl";
            Size = new Size(739, 241);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private BoundControls.BoundTextBox AlbumGenre;
        private BoundControls.BoundTextBox AlbumComments;
        private Label label6;
        private Label label5;
        private BoundControls.BoundTextBox AlbumYear;
        private Label label4;
        private BoundControls.BoundTextBox AlbumArtist;
        private BoundControls.BoundTextBox AlbumTitle;
        private Label label3;
        private Label label1;
    }
}
