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
            // MixActionFlacControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(CompressionComboBox);
            Controls.Add(label1);
            Name = "MixActionFlacControl";
            Size = new Size(518, 83);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox CompressionComboBox;
        private Label label1;
    }
}
