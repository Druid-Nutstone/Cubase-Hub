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
            ButtonAdd.Location = new Point(374, 3);
            ButtonAdd.Name = "ButtonAdd";
            ButtonAdd.Size = new Size(55, 29);
            ButtonAdd.TabIndex = 1;
            ButtonAdd.Text = "Add";
            ButtonAdd.UseVisualStyleBackColor = true;
            // 
            // ButtonDel
            // 
            ButtonDel.Location = new Point(374, 38);
            ButtonDel.Name = "ButtonDel";
            ButtonDel.Size = new Size(55, 29);
            ButtonDel.TabIndex = 2;
            ButtonDel.Text = "Del";
            ButtonDel.UseVisualStyleBackColor = true;
            // 
            // MacroKeySelectorControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ButtonDel);
            Controls.Add(ButtonAdd);
            Controls.Add(MacroCommandListView);
            Name = "MacroKeySelectorControl";
            Size = new Size(441, 128);
            ResumeLayout(false);
        }

        #endregion

        private MacroKeyCommandListView MacroCommandListView;
        private Button ButtonAdd;
        private Button ButtonDel;
    }
}
