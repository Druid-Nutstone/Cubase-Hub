namespace Cubase.Hub.Forms.Tracks
{
    partial class NewTrackForm
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
            panel1 = new Panel();
            CreateTrackButton = new Button();
            panel2 = new Panel();
            label4 = new Label();
            TrackName = new TextBox();
            label3 = new Label();
            SelectedTemplate = new ComboBox();
            label2 = new Label();
            SelectedAlbum = new ComboBox();
            label1 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(CreateTrackButton);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 294);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 56);
            panel1.TabIndex = 0;
            // 
            // CreateTrackButton
            // 
            CreateTrackButton.Location = new Point(34, 15);
            CreateTrackButton.Name = "CreateTrackButton";
            CreateTrackButton.Size = new Size(129, 29);
            CreateTrackButton.TabIndex = 0;
            CreateTrackButton.Text = "Create Track";
            CreateTrackButton.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(label4);
            panel2.Controls.Add(TrackName);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(SelectedTemplate);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(SelectedAlbum);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 294);
            panel2.TabIndex = 1;
            // 
            // label4
            // 
            label4.Location = new Point(37, 177);
            label4.Name = "label4";
            label4.Size = new Size(733, 75);
            label4.TabIndex = 6;
            label4.Text = "Cubase (sometimes) does not support creating a new project programatically. I have copied the target directory to the clipboard . Paste that into the File open dialogue.  AND REMEMBER TO SAVE IT!";
            // 
            // TrackName
            // 
            TrackName.Location = new Point(34, 125);
            TrackName.Name = "TrackName";
            TrackName.Size = new Size(736, 27);
            TrackName.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(29, 102);
            label3.Name = "label3";
            label3.Size = new Size(92, 20);
            label3.TabIndex = 4;
            label3.Text = "Track Name";
            // 
            // SelectedTemplate
            // 
            SelectedTemplate.FormattingEnabled = true;
            SelectedTemplate.Location = new Point(417, 45);
            SelectedTemplate.Name = "SelectedTemplate";
            SelectedTemplate.Size = new Size(353, 28);
            SelectedTemplate.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(417, 23);
            label2.Name = "label2";
            label2.Size = new Size(74, 20);
            label2.TabIndex = 2;
            label2.Text = "Template";
            // 
            // SelectedAlbum
            // 
            SelectedAlbum.FormattingEnabled = true;
            SelectedAlbum.Location = new Point(34, 45);
            SelectedAlbum.Name = "SelectedAlbum";
            SelectedAlbum.Size = new Size(353, 28);
            SelectedAlbum.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(29, 23);
            label1.Name = "label1";
            label1.Size = new Size(56, 20);
            label1.TabIndex = 0;
            label1.Text = "Album";
            // 
            // NewTrackForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 350);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "NewTrackForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "New Track ";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private ComboBox SelectedAlbum;
        private ComboBox SelectedTemplate;
        private Label label2;
        private Button CreateTrackButton;
        private TextBox TrackName;
        private Label label3;
        private Label label4;
    }
}