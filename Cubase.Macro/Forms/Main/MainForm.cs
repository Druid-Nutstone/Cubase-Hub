using Cubase.Macro.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro
{
    public partial class MainForm : BaseWindows11Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action ActionComplete { get; set; } = () => { }; 

        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            ThemeApplier.ApplyDarkTheme(this);
        }

        protected override void OnClick(EventArgs e)
        {
            this.TopMost = false;
            this.WindowState = FormWindowState.Minimized;
            if (this.ActionComplete != null)
            {
                this.ActionComplete.Invoke();
            }
        }

        public void ShowMacros()
        {

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            // Use WorkingArea to avoid taskbar overlap
            // Use Bounds if you WANT to cover taskbar

            this.SuspendLayout();

            this.StartPosition = FormStartPosition.Manual;
            // this.FormBorderStyle = FormBorderStyle.None; // optional (for panel look)

            this.Location = new Point(screen.Left, screen.Top);
            this.Size = new Size(this.Width, screen.Height);

            this.ResumeLayout();

            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            this.TopMost = true;                  // Always on top
            this.Show();                           // Show the form if hidden
            this.BringToFront();                   // Bring to front
            this.Activate();                       // Give focus to form

            this.Focus();
        }
    }
}
