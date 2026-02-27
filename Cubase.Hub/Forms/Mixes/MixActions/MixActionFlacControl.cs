using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Track;
using FFMpegCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Mixes.MixActions
{
    public partial class MixActionFlacControl : UserControl, IMixActionControl
    {
        private FlacConfiguration FlacConfiguration = new FlacConfiguration();
        
        public MixActionFlacControl()
        {
            InitializeComponent();
            this.CompressionComboBox.Items.Clear();
            this.CompressionComboBox.Items.AddRange(Enum.GetNames(typeof(CompressionLevel)));
            this.CompressionComboBox.SelectedIndexChanged += CompressionComboBox_SelectedIndexChanged;
            this.CompressionComboBox.SelectedItem = this.FlacConfiguration.CompressionLevel.ToString();
            this.BitRate.Items.Clear();
            this.BitRate.Items.AddRange(["8bit", "16bit", "32bit"]);
            this.BitRate.SelectedIndex = 2;
            this.BitRate.SelectedIndexChanged += BitRate_SelectedIndexChanged;
            this.SampleRate.Items.Clear();
            this.SampleRate.Items.AddRange(["16000khz","44100khz","48000khz","64000khz", "96000khz"]);
            this.SampleRate.SelectedIndex = 2;
            this.SampleRate.SelectedIndexChanged += SampleRate_SelectedIndexChanged;
        }

        private void SampleRate_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.FlacConfiguration.SampleRate = Enum.GetValues<SampleRate>()[this.SampleRate.SelectedIndex];
        }

        private void BitRate_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.FlacConfiguration.Depth = Enum.GetValues<BitDepth>()[this.BitRate.SelectedIndex];
        }

        private void CompressionComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.FlacConfiguration.CompressionLevel = Enum.Parse<CompressionLevel>(this.CompressionComboBox.SelectedItem as string, true);
        }

        public void RunAction(MixDownCollection mixDowns, 
                              string targetDirectory,
                              ITrackService trackService, 
                              IMessageService messageService, 
                              IDirectoryService directoryService,
                              Func<int, string, bool> onProgress)
        {
            for (var i = 0; i < mixDowns.Count; i++)
            {
                var state = onProgress.Invoke(i + 1, Path.GetFileNameWithoutExtension(mixDowns[i].FileName));
                trackService.ConvertToFlac(mixDowns[i], targetDirectory, this.FlacConfiguration);
            }
            onProgress.Invoke(-1, "All Done");
        }
    }
}
