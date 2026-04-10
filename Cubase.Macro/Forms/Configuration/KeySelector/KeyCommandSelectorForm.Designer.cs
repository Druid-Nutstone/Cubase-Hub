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
            panel3 = new Panel();
            keyCommandListView = new KeyCommandListView();
            panel2 = new Panel();
            SearchForText = new TextBox();
            label1 = new Label();
            ClearButton = new Button();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(724, 450);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(keyCommandListView);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 109);
            panel3.Name = "panel3";
            panel3.Size = new Size(724, 341);
            panel3.TabIndex = 1;
            // 
            // keyCommandListView
            // 
            keyCommandListView.Dock = DockStyle.Fill;
            keyCommandListView.FullRowSelect = true;
            keyCommandListView.Location = new Point(0, 0);
            keyCommandListView.MultiSelect = false;
            keyCommandListView.Name = "keyCommandListView";
            keyCommandListView.Size = new Size(724, 341);
            keyCommandListView.TabIndex = 0;
            keyCommandListView.UseCompatibleStateImageBehavior = false;
            keyCommandListView.View = View.Details;
            // 
            // panel2
            // 
            panel2.Controls.Add(ClearButton);
            panel2.Controls.Add(SearchForText);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(724, 109);
            panel2.TabIndex = 0;
            // 
            // SearchForText
            // 
            SearchForText.Location = new Point(20, 38);
            SearchForText.Name = "SearchForText";
            SearchForText.Size = new Size(320, 27);
            SearchForText.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(20, 15);
            label1.Name = "label1";
            label1.Size = new Size(55, 20);
            label1.TabIndex = 0;
            label1.Text = "Search";
            // 
            // ClearButton
            // 
            ClearButton.Location = new Point(346, 38);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new Size(40, 29);
            ClearButton.TabIndex = 2;
            ClearButton.Text = "X";
            ClearButton.UseVisualStyleBackColor = true;
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
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel3;
        private KeyCommandListView keyCommandListView;
        private Panel panel2;
        private TextBox SearchForText;
        private Label label1;
        private Button ClearButton;
    }
}