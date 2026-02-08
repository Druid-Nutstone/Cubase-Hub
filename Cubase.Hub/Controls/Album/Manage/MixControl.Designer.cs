namespace Cubase.Hub.Controls.Album.Manage
{
    partial class MixControl
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
            MixTitle = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            MixTrackNo = new Cubase.Hub.Controls.BoundControls.BoundNumericUpDown();
            MixPerformers = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            MixDuration = new Cubase.Hub.Controls.BoundControls.BoundLabel();
            PlayerPanel = new Panel();
            MixExtra = new Label();
            label1 = new Label();
            label2 = new Label();
            Performers = new Label();
            label3 = new Label();
            label4 = new Label();
            MixComments = new Cubase.Hub.Controls.BoundControls.BoundTextBox();
            label5 = new Label();
            MixSelected = new Cubase.Hub.Controls.BoundControls.BoundCheckbox();
            ((System.ComponentModel.ISupportInitialize)MixTrackNo).BeginInit();
            SuspendLayout();
            // 
            // MixTitle
            // 
            MixTitle.Location = new Point(100, 48);
            MixTitle.Name = "MixTitle";
            MixTitle.Size = new Size(182, 27);
            MixTitle.TabIndex = 0;
            // 
            // MixTrackNo
            // 
            MixTrackNo.Location = new Point(309, 48);
            MixTrackNo.Name = "MixTrackNo";
            MixTrackNo.Size = new Size(45, 27);
            MixTrackNo.TabIndex = 1;
            // 
            // MixPerformers
            // 
            MixPerformers.Location = new Point(388, 47);
            MixPerformers.Name = "MixPerformers";
            MixPerformers.Size = new Size(195, 27);
            MixPerformers.TabIndex = 2;
            // 
            // MixDuration
            // 
            MixDuration.AutoSize = true;
            MixDuration.Location = new Point(814, 50);
            MixDuration.Name = "MixDuration";
            MixDuration.Size = new Size(31, 20);
            MixDuration.TabIndex = 3;
            MixDuration.Text = "dur";
            // 
            // PlayerPanel
            // 
            PlayerPanel.Location = new Point(879, 41);
            PlayerPanel.Name = "PlayerPanel";
            PlayerPanel.Size = new Size(196, 36);
            PlayerPanel.TabIndex = 4;
            // 
            // MixExtra
            // 
            MixExtra.AutoSize = true;
            MixExtra.Location = new Point(1096, 47);
            MixExtra.Name = "MixExtra";
            MixExtra.Size = new Size(70, 20);
            MixExtra.TabIndex = 5;
            MixExtra.Text = "Mix Extra";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(100, 25);
            label1.Name = "label1";
            label1.Size = new Size(38, 20);
            label1.TabIndex = 6;
            label1.Text = "Title";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(299, 25);
            label2.Name = "label2";
            label2.Size = new Size(67, 20);
            label2.TabIndex = 7;
            label2.Text = "Track No";
            // 
            // Performers
            // 
            Performers.AutoSize = true;
            Performers.Location = new Point(388, 25);
            Performers.Name = "Performers";
            Performers.Size = new Size(80, 20);
            Performers.TabIndex = 8;
            Performers.Text = "Performers";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(814, 25);
            label3.Name = "label3";
            label3.Size = new Size(42, 20);
            label3.TabIndex = 9;
            label3.Text = "Time";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(598, 25);
            label4.Name = "label4";
            label4.Size = new Size(80, 20);
            label4.TabIndex = 10;
            label4.Text = "Comments";
            // 
            // MixComments
            // 
            MixComments.Location = new Point(598, 47);
            MixComments.Name = "MixComments";
            MixComments.Size = new Size(198, 27);
            MixComments.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(35, 25);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 13;
            label5.Text = "Select";
            // 
            // MixSelected
            // 
            MixSelected.AutoSize = true;
            MixSelected.Location = new Point(45, 53);
            MixSelected.Name = "MixSelected";
            MixSelected.Size = new Size(18, 17);
            MixSelected.TabIndex = 14;
            MixSelected.UseVisualStyleBackColor = true;
            // 
            // MixControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(MixSelected);
            Controls.Add(label5);
            Controls.Add(MixComments);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(Performers);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(MixExtra);
            Controls.Add(PlayerPanel);
            Controls.Add(MixDuration);
            Controls.Add(MixPerformers);
            Controls.Add(MixTrackNo);
            Controls.Add(MixTitle);
            Name = "MixControl";
            Size = new Size(1347, 109);
            ((System.ComponentModel.ISupportInitialize)MixTrackNo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private BoundControls.BoundTextBox MixTitle;
        private BoundControls.BoundNumericUpDown MixTrackNo;
        private BoundControls.BoundTextBox MixPerformers;
        private BoundControls.BoundLabel MixDuration;
        private Panel PlayerPanel;
        private Label MixExtra;
        private Label label1;
        private Label label2;
        private Label Performers;
        private Label label3;
        private Label label4;
        private BoundControls.BoundTextBox MixComments;
        private Label label5;
        private BoundControls.BoundCheckbox MixSelected;
    }
}
