namespace Cubase.Hub.Forms.Mixes.MixActions
{
    partial class MixActionFlacControl
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
            CompressionComboBox = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            BitRate = new ComboBox();
            label3 = new Label();
            SampleRate = new ComboBox();
            SuspendLayout();
            // 
            // CompressionComboBox
            // 
            CompressionComboBox.FormattingEnabled = true;
            CompressionComboBox.Location = new Point(23, 32);
            CompressionComboBox.Name = "CompressionComboBox";
            CompressionComboBox.Size = new Size(176, 28);
            CompressionComboBox.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(23, 9);
            label1.Name = "label1";
            label1.Size = new Size(100, 20);
            label1.TabIndex = 2;
            label1.Text = "Compression";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(237, 9);
            label2.Name = "label2";
            label2.Size = new Size(65, 20);
            label2.TabIndex = 4;
            label2.Text = "Bit Rate";
            // 
            // BitRate
            // 
            BitRate.FormattingEnabled = true;
            BitRate.Location = new Point(237, 32);
            BitRate.Name = "BitRate";
            BitRate.Size = new Size(96, 28);
            BitRate.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(389, 9);
            label3.Name = "label3";
            label3.Size = new Size(96, 20);
            label3.TabIndex = 6;
            label3.Text = "Sample Rate";
            // 
            // SampleRate
            // 
            SampleRate.FormattingEnabled = true;
            SampleRate.Location = new Point(389, 32);
            SampleRate.Name = "SampleRate";
            SampleRate.Size = new Size(151, 28);
            SampleRate.TabIndex = 7;
            // 
            // MixActionFlacControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(SampleRate);
            Controls.Add(label3);
            Controls.Add(BitRate);
            Controls.Add(label2);
            Controls.Add(CompressionComboBox);
            Controls.Add(label1);
            Name = "MixActionFlacControl";
            Size = new Size(608, 83);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox CompressionComboBox;
        private Label label1;
        private Label label2;
        private ComboBox BitRate;
        private Label label3;
        private ComboBox SampleRate;
    }
}
