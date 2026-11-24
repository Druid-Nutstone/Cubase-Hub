namespace Cubase.Macro.Forms.Configuration.KeyEditors
{
    partial class MacroKeySelectorControl
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
            MacroCommandListView = new MacroKeyCommandListView();
            ButtonAdd = new Button();
            ButtonDel = new Button();
            label1 = new Label();
            AfterKeyWaitTime = new Cubase.Macro.BoundControls.BoundTextBox();
            ButtonConditional = new Button();
            SuspendLayout();
            // 
            // MacroCommandListView
            // 
            MacroCommandListView.BackColor = Color.FromArgb(32, 32, 32);
            MacroCommandListView.ForeColor = Color.FromArgb(220, 220, 220);
            MacroCommandListView.FullRowSelect = true;
            MacroCommandListView.Location = new Point(3, 3);
            MacroCommandListView.MultiSelect = false;
            MacroCommandListView.Name = "MacroCommandListView";
            MacroCommandListView.Size = new Size(357, 121);
            MacroCommandListView.TabIndex = 0;
            MacroCommandListView.UseCompatibleStateImageBehavior = false;
            MacroCommandListView.View = View.Details;
            // 
            // ButtonAdd
            // 
            ButtonAdd.Location = new Point(3, 130);
            ButtonAdd.Name = "ButtonAdd";
            ButtonAdd.Size = new Size(103, 29);
            ButtonAdd.TabIndex = 1;
            ButtonAdd.Text = "Add Key";
            ButtonAdd.UseVisualStyleBackColor = true;
            // 
            // ButtonDel
            // 
            ButtonDel.BackColor = Color.IndianRed;
            ButtonDel.ForeColor = Color.White;
            ButtonDel.Location = new Point(366, 3);
            ButtonDel.Name = "ButtonDel";
            ButtonDel.Size = new Size(55, 29);
            ButtonDel.TabIndex = 2;
            ButtonDel.Text = "Del";
            ButtonDel.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(8, 170);
            label1.Name = "label1";
            label1.Size = new Size(301, 20);
            label1.TabIndex = 3;
            label1.Text = "Time to wait in MS after key has executed";
            // 
            // AfterKeyWaitTime
            // 
            AfterKeyWaitTime.Location = new Point(8, 193);
            AfterKeyWaitTime.Name = "AfterKeyWaitTime";
            AfterKeyWaitTime.Size = new Size(125, 27);
            AfterKeyWaitTime.TabIndex = 4;
            // 
            // ButtonConditional
            // 
            ButtonConditional.Location = new Point(149, 130);
            ButtonConditional.Name = "ButtonConditional";
            ButtonConditional.Size = new Size(211, 29);
            ButtonConditional.TabIndex = 5;
            ButtonConditional.Text = "Add Conditional Key";
            ButtonConditional.UseVisualStyleBackColor = true;
            // 
            // MacroKeySelectorControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ButtonConditional);
            Controls.Add(AfterKeyWaitTime);
            Controls.Add(label1);
            Controls.Add(ButtonDel);
            Controls.Add(ButtonAdd);
            Controls.Add(MacroCommandListView);
            Name = "MacroKeySelectorControl";
            Size = new Size(441, 245);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MacroKeyCommandListView MacroCommandListView;
        private Button ButtonAdd;
        private Button ButtonDel;
        private Label label1;
        private BoundControls.BoundTextBox AfterKeyWaitTime;
        private Button ButtonConditional;
    }
}
