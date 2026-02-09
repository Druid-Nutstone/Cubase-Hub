namespace Cubase.Hub.Forms.Mixes.MixActions
{
    partial class MixActionMp3Control
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
            label1 = new Label();
            QualityComboBox = new ComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(23, 9);
            label1.Name = "label1";
            label1.Size = new Size(59, 20);
            label1.TabIndex = 0;
            label1.Text = "Quality";
            // 
            // QualityComboBox
            // 
            QualityComboBox.FormattingEnabled = true;
            QualityComboBox.Location = new Point(23, 32);
            QualityComboBox.Name = "QualityComboBox";
            QualityComboBox.Size = new Size(176, 28);
            QualityComboBox.TabIndex = 1;
            // 
            // MixActionMp3Control
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(QualityComboBox);
            Controls.Add(label1);
            Name = "MixActionMp3Control";
            Size = new Size(493, 77);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox QualityComboBox;
    }
}
