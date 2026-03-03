using Cubase.Hub.Forms.BaseForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Distributers.SoundCloud
{
    public partial class SoundCloudMessageForm : BaseWindows11Form
    {
        public SoundCloudMessageForm()
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.Font = new Font(this.Font.FontFamily, this.Font.Size + 3, FontStyle.Bold);
            this.Cursor = Cursors.WaitCursor;
        }

        public void ShowMessage(string message)
        {
            this.TopMost = true;
            this.MessageLabel.Text = message;
            this.Refresh();
        }
    }
}
