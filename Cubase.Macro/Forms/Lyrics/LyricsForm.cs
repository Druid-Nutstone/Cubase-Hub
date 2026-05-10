using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Lyrics
{
    public partial class LyricsForm : BaseWindows11Form
    {
        public LyricsForm()
        {
            this.Text = "Create and amend lyrics and setlists";
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            var menuItems = new TopMenuItems(this.TopMenu, this.lyricEditor);
        }
    }
}
