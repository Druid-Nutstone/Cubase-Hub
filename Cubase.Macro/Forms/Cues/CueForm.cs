using Cubase.Macro.Common.Models;
using Cubase.Macro.Models;
using Cubase.Macro.Services.Config;
using Cubase.Macro.Services.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Macro.Forms.Cues
{
    public partial class CueForm : BaseWindows11Form
    {
        private readonly IMidiService midiService;

        private readonly IConfigurationService configurationService;

        private CueLevelCollection cueLevels;

        public CueForm(IMidiService midiService, IConfigurationService configurationService)
        {
            this.Cursor = Cursors.WaitCursor;
            this.configurationService = configurationService;
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.midiService = midiService;
            this.midiService.ResumeCueChecking();
            this.midiService.RegisterForGetCueLevelsEndCallbacks(OnCueLevelsChanged);
            this.midiService.RegisterForUpdateCueLevelsEndCallbacks(OnCueLevelUpdated);
            this.midiService.GetCueCollection();
            this.CueControl.OnCueChanged = CueChanged;
            this.CueControl.OnRefreshMixer = RefreshMixer;
            this.LoadLocation();
        }

        private void SaveLocation()
        {
            var bounds = WindowState == FormWindowState.Normal
            ? Bounds
            : RestoreBounds;

            var settings = new WindowSettings
            {
                X = bounds.X,
                Y = bounds.Y,
                Width = bounds.Width,
                Height = bounds.Height,
            };

            settings.isMaximised = this.WindowState == FormWindowState.Maximized;
            this.configurationService?.Configuration?.CueLevelLocation = settings;
            this.configurationService?.Configuration.Save();
        }

        private void LoadLocation()
        {
            this.configurationService.ReloadConfiguration();
            if (this.configurationService.Configuration.CueLevelLocation != null)
            {
                if (this.configurationService.Configuration.CueLevelLocation.isMaximised)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    StartPosition = FormStartPosition.Manual;
                    Bounds = new Rectangle(
                        this.configurationService.Configuration.CueLevelLocation.X,
                        this.configurationService.Configuration.CueLevelLocation.Y,
                        this.configurationService.Configuration.CueLevelLocation.Width,
                        this.configurationService.Configuration.CueLevelLocation.Height);
                }
            }
        }

        public void OnCueLevelUpdated()
        {
            this.midiService.GetCueCollection();
        }

        private void OnCueLevelsChanged()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateCueLevels(this.midiService.CueCollection)));
            }
            else
            {
                UpdateCueLevels(this.midiService.CueCollection);
            }
        }

        private void UpdateCueLevels(CueLevelCollection cueLevels)
        {
            this.Cursor = Cursors.Default;
            this.cueLevels = cueLevels;
            this.CueControl.UpdateCueLevels(this.cueLevels);
        }

        private void RefreshMixer()
        {
            this.midiService.GetCueCollection();
        }

        private void CueChanged(CueLevel cue, int cueIndex)
        {
            var cueUpdateRequest = new CueUpdateRequest()
            {
                TrackIndex = cue.TrackIndex,
                CueSlotIndex = cueIndex,
                CueLevel = cue.Volume
            };
            this.midiService.SendSysExMessage(MidiCommand.UpdateCueLevel, cueUpdateRequest);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            this.SaveLocation();
            this.midiService.DeRegisterForGetCueLevelsEndCallbacks(OnCueLevelsChanged);
            this.midiService.DeRegisterForUpdateCueLevelsEndCallbacks(OnCueLevelUpdated);
            this.midiService.SuspendCueChecking();
        }


    }
}
