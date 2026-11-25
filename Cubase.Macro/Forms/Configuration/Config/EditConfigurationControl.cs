using Cubase.Macro.Forms.Configuration.KeySelector;
using Cubase.Macro.Models;

namespace Cubase.Macro.Forms.Configuration.Config
{
    public partial class EditConfigurationControl : UserControl
    {
        private readonly CubaseMacroConfiguration cubaseMacroConfiguration;

        public EditConfigurationControl(CubaseMacroConfiguration cubaseMacroConfiguration)
        {
            InitializeComponent();
            this.SelectVisibilityKey.Click += SelectVisibilityKey_Click;
            this.cubaseMacroConfiguration = cubaseMacroConfiguration;
            ThemeApplier.ApplyDarkTheme(this);
            ResetVisibilityKey.Bind(nameof(CubaseMacroConfiguration.ResetVisibilityKey), this.cubaseMacroConfiguration);
            MenuHeight.Bind(nameof(CubaseMacroConfiguration.MenuHeight), this.cubaseMacroConfiguration);
            MenuHeight.LostFocus += SaveConfig;
            KeyHeight.Bind(nameof(CubaseMacroConfiguration.KeyHeight), this.cubaseMacroConfiguration);
            KeyHeight.LostFocus += SaveConfig;
            CubaseExecutableName.Bind(nameof(CubaseMacroConfiguration.CubaseExecutable), this.cubaseMacroConfiguration);
            CubaseExecutableName.LostFocus += SaveConfig;
            CubaseProjectWindowStartsWith.Bind(nameof(CubaseMacroConfiguration.CubaseProjectWindowName), this.cubaseMacroConfiguration);
            CubaseProjectWindowStartsWith.LostFocus += SaveConfig;
            CubaseRestartWindowsMidiService.Bind(nameof(CubaseMacroConfiguration.ReloadWindowsMidiService), this.cubaseMacroConfiguration);
            CubaseRestartWindowsMidiService.CheckedChanged += SaveConfig;
            LyricFontSize.Bind(nameof(CubaseMacroConfiguration.LyricFontSize), this.cubaseMacroConfiguration);
            LyricFontSize.LostFocus += SaveConfig;
            lyricEditorPath.Bind(nameof(CubaseMacroConfiguration.LyricViewerFilePath), this.cubaseMacroConfiguration);
            lyricEditorPath.LostFocus += SaveConfig;
            this.AddSourceLocation.Click += AddSourceLocation_Click;
            this.PopulateSourceLocations();
        }

        private void PopulateSourceLocations()
        {
            foreach (var item in cubaseMacroConfiguration.SourceCubaseFolders)
            {
                this.CubaseSourceFolderView.AddSourceDirectory(item);
            }
        }

        private void AddSourceLocation_Click(object? sender, EventArgs e)
        {
            var folderbrowser = new FolderBrowserDialog();
            folderbrowser.ShowNewFolderButton = true;
            folderbrowser.UseDescriptionForTitle = true;
            folderbrowser.Description = "Select NEW cubase project source";
            if (folderbrowser.ShowDialog() != DialogResult.Cancel)
            {
                this.cubaseMacroConfiguration.SourceCubaseFolders.Add(folderbrowser.SelectedPath);
                this.cubaseMacroConfiguration.Save();
                this.CubaseSourceFolderView.AddSourceDirectory(folderbrowser.SelectedPath);
            }
        }

        private void SaveConfig(object? sender, EventArgs e)
        {
            this.cubaseMacroConfiguration.Save();
        }

        private void SelectVisibilityKey_Click(object? sender, EventArgs e)
        {
            var keySelector = new KeyCommandSelectorForm((cubaseMacro) =>
            {
                this.ResetVisibilityKey.Text = cubaseMacro.Serialise();
                this.cubaseMacroConfiguration.Save();
            });
            keySelector.ShowDialog();
        }

    }
}
