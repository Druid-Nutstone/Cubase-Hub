using Cubase.Hub.Forms.BaseForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Message
{
    public partial class NonBlockingMessage : BaseWindows11Form
    {
        public NonBlockingMessage()
        {
            InitializeComponent();
            
            // this.ClientSize = new Size(1, 1);

            // Optional: remove border if you want a “popup” look
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Keep on top
            this.TopMost = true;

            // Optional: do not show in taskbar
            this.ShowInTaskbar = false;
            ThemeApplier.ApplyDarkTheme(this);
        }

        public void SetMessage(string message)
        {
            this.Text = message;
        }
    }
}
