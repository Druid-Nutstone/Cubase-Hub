namespace Cubase.Hub.Forms.Export
{
    partial class ExportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
            DataPanel = new Panel();
            SuspendLayout();
            // 
            // DataPanel
            // 
            DataPanel.Dock = DockStyle.Fill;
            DataPanel.Location = new Point(0, 0);
            DataPanel.Name = "DataPanel";
            DataPanel.Size = new Size(800, 450);
            DataPanel.TabIndex = 0;
            // 
            // ExportForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(DataPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ExportForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Export";
            ResumeLayout(false);
        }

        #endregion

        private Panel DataPanel;
    }
}