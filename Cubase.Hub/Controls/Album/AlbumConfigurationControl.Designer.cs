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
            Holder = new TableLayoutPanel();
            panel1 = new Panel();
            panel2 = new Panel();
            Holder.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // AlbumGenre
            // 
            AlbumGenre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            AlbumGenre.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AlbumGenre.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AlbumGenre.Location = new Point(26, 92);
            AlbumGenre.Name = "AlbumGenre";
            AlbumGenre.Size = new Size(343, 27);
            AlbumGenre.TabIndex = 21;
            // 
            // AlbumComments
            // 
            AlbumComments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            AlbumComments.Location = new Point(25, 182);
            AlbumComments.Name = "AlbumComments";
            AlbumComments.Size = new Size(747, 27);
            AlbumComments.TabIndex = 20;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(25, 159);
            label6.Name = "label6";
            label6.Size = new Size(136, 20);
            label6.TabIndex = 19;
            label6.Text = "Album Comments";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(26, 69);
            label5.Name = "label5";
            label5.Size = new Size(102, 20);
            label5.TabIndex = 18;
            label5.Text = "Album Genre";
            // 
            // AlbumYear
            // 
            AlbumYear.Location = new Point(21, 92);
            AlbumYear.Name = "AlbumYear";
            AlbumYear.Size = new Size(125, 27);
            AlbumYear.TabIndex = 17;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(22, 69);
            label4.Name = "label4";
            label4.Size = new Size(90, 20);
            label4.TabIndex = 16;
            label4.Text = "Album Year";
            // 
            // AlbumArtist
            // 
            AlbumArtist.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            AlbumArtist.Location = new Point(26, 23);
            AlbumArtist.Name = "AlbumArtist";
            AlbumArtist.Size = new Size(343, 27);
            AlbumArtist.TabIndex = 15;
            // 
            // AlbumTitle
            // 
            AlbumTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            AlbumTitle.Location = new Point(21, 23);
            AlbumTitle.Name = "AlbumTitle";
            AlbumTitle.Size = new Size(364, 27);
            AlbumTitle.TabIndex = 14;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(26, -3);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 13;
            label3.Text = "Artist";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(21, 0);
            label1.Name = "label1";
            label1.Size = new Size(91, 20);
            label1.TabIndex = 12;
            label1.Text = "Album Title";
            // 
            // Holder
            // 
            Holder.ColumnCount = 2;
            Holder.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Holder.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            Holder.Controls.Add(panel1, 0, 0);
            Holder.Controls.Add(panel2, 1, 0);
            Holder.Dock = DockStyle.Top;
            Holder.Location = new Point(0, 0);
            Holder.Name = "Holder";
            Holder.RowCount = 1;
            Holder.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            Holder.Size = new Size(798, 145);
            Holder.TabIndex = 22;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Controls.Add(AlbumTitle);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(AlbumYear);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(393, 139);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(label3);
            panel2.Controls.Add(AlbumGenre);
            panel2.Controls.Add(AlbumArtist);
            panel2.Controls.Add(label5);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(402, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(393, 139);
            panel2.TabIndex = 1;
            // 
            // AlbumConfigurationControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Holder);
            Controls.Add(AlbumComments);
            Controls.Add(label6);
            Name = "AlbumConfigurationControl";
            Size = new Size(798, 229);
            Holder.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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
        private TableLayoutPanel Holder;
        private Panel panel1;
        private Panel panel2;
    }
}
