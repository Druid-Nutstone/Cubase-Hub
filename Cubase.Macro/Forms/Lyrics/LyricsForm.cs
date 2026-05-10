using Cubase.Macro.Services.Config;
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
        public LyricsForm(IConfigurationService configurationService)
        {
            this.Text = "Create and Amend Lyrics and Setlists";
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            configurationService.ReloadConfiguration();
            this.lyricEditor.SetDefaultFontSize(configurationService.Configuration.LyricFontSize);  
            var menuItems = new TopMenuItems(this.TopMenu, this.lyricEditor);
        }
    }
}
