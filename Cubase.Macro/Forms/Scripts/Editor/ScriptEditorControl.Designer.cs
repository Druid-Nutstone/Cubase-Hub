namespace Cubase.Macro.Forms.Scripts.Editor
{
    partial class ScriptEditorControl
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
            panel1 = new Panel();
            RunScript = new Button();
            MainPanel = new Panel();
            panel2 = new Panel();
            ErrorMessage = new Label();
            NewScript = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(NewScript);
            panel1.Controls.Add(RunScript);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(474, 42);
            panel1.TabIndex = 0;
            // 
            // RunScript
            // 
            RunScript.Location = new Point(17, 7);
            RunScript.Name = "RunScript";
            RunScript.Size = new Size(94, 29);
            RunScript.TabIndex = 0;
            RunScript.Text = "▶ Run";
            RunScript.UseVisualStyleBackColor = true;
            // 
            // MainPanel
            // 
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Location = new Point(0, 42);
            MainPanel.Name = "MainPanel";
            MainPanel.Size = new Size(474, 173);
            MainPanel.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.Controls.Add(ErrorMessage);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 215);
            panel2.Name = "panel2";
            panel2.Size = new Size(474, 40);
            panel2.TabIndex = 1;
            // 
            // ErrorMessage
            // 
            ErrorMessage.AutoSize = true;
            ErrorMessage.ForeColor = Color.IndianRed;
            ErrorMessage.Location = new Point(12, 10);
            ErrorMessage.Name = "ErrorMessage";
            ErrorMessage.Size = new Size(51, 20);
            ErrorMessage.TabIndex = 0;
            ErrorMessage.Text = "[error]";
            // 
            // NewScript
            // 
            NewScript.Location = new Point(132, 7);
            NewScript.Name = "NewScript";
            NewScript.Size = new Size(109, 29);
            NewScript.TabIndex = 1;
            NewScript.Text = "＋New Script ";
            NewScript.UseVisualStyleBackColor = true;
            // 
            // ScriptEditorControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(MainPanel);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "ScriptEditorControl";
            Size = new Size(474, 255);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel MainPanel;
        private Panel panel2;
        private Label ErrorMessage;
        private Button RunScript;
        private Button NewScript;
    }
}
