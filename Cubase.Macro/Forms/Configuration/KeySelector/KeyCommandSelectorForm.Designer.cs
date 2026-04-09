namespace Cubase.Macro.Forms.Configuration.KeySelector
{
    partial class KeyCommandSelectorForm
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
            keyCommandListView = new KeyCommandListView();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(keyCommandListView);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(724, 450);
            panel1.TabIndex = 0;
            // 
            // keyCommandListView
            // 
            keyCommandListView.Dock = DockStyle.Fill;
            keyCommandListView.FullRowSelect = true;
            keyCommandListView.Location = new Point(0, 0);
            keyCommandListView.MultiSelect = false;
            keyCommandListView.Name = "keyCommandListView";
            keyCommandListView.Size = new Size(724, 450);
            keyCommandListView.TabIndex = 0;
            keyCommandListView.UseCompatibleStateImageBehavior = false;
            keyCommandListView.View = View.Details;
            // 
            // KeyCommandSelectorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(724, 450);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "KeyCommandSelectorForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Key Commands";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private KeyCommandListView keyCommandListView;
    }
}