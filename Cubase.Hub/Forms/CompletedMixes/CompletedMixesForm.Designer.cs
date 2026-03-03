namespace Cubase.Hub.Forms.CompletedMixes
{
    partial class CompletedMixesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompletedMixesForm));
            MenuPanel = new Panel();
            MenuTreePanel = new Panel();
            panel1 = new Panel();
            label1 = new Label();
            splitter1 = new Splitter();
            DataPanel = new Panel();
            Distributer = new Cubase.Hub.Controls.BoundControls.BoundEnumComboBox();
            MenuPanel.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // MenuPanel
            // 
            MenuPanel.Controls.Add(MenuTreePanel);
            MenuPanel.Controls.Add(panel1);
            MenuPanel.Dock = DockStyle.Left;
            MenuPanel.Location = new Point(0, 0);
            MenuPanel.Name = "MenuPanel";
            MenuPanel.Size = new Size(250, 450);
            MenuPanel.TabIndex = 0;
            // 
            // MenuTreePanel
            // 
            MenuTreePanel.Dock = DockStyle.Fill;
            MenuTreePanel.Location = new Point(0, 86);
            MenuTreePanel.Name = "MenuTreePanel";
            MenuTreePanel.Size = new Size(250, 364);
            MenuTreePanel.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(Distributer);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 86);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(85, 20);
            label1.TabIndex = 0;
            label1.Text = "Distributer";
            // 
            // splitter1
            // 
            splitter1.Location = new Point(250, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(4, 450);
            splitter1.TabIndex = 1;
            splitter1.TabStop = false;
            // 
            // DataPanel
            // 
            DataPanel.Dock = DockStyle.Fill;
            DataPanel.Location = new Point(254, 0);
            DataPanel.Name = "DataPanel";
            DataPanel.Size = new Size(546, 450);
            DataPanel.TabIndex = 2;
            // 
            // Distributer
            // 
            Distributer.FormattingEnabled = true;
            Distributer.Location = new Point(12, 42);
            Distributer.Name = "Distributer";
            Distributer.Size = new Size(151, 28);
            Distributer.TabIndex = 1;
            // 
            // CompletedMixesForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(DataPanel);
            Controls.Add(splitter1);
            Controls.Add(MenuPanel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CompletedMixesForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Completed Mixes";
            MenuPanel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel MenuPanel;
        private Splitter splitter1;
        private Panel DataPanel;
        private Panel MenuTreePanel;
        private Panel panel1;
        private Label label1;
        private Controls.BoundControls.BoundEnumComboBox Distributer;
    }
}