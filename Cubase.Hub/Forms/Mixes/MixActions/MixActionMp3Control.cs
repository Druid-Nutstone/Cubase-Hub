using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Messages;
using Cubase.Hub.Services.Models;
using FFMpegCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cubase.Hub.Forms.Mixes.MixActions
{
    public partial class MixActionMp3Control : UserControl, IMixActionControl
    {
        private AudioQuality quality = AudioQuality.Ultra; 

        public MixActionMp3Control()
        {
            InitializeComponent();
            this.QualityComboBox.Items.Clear();
            this.QualityComboBox.Items.AddRange(Enum.GetNames(typeof(AudioQuality)));
            this.QualityComboBox.SelectedIndexChanged += QualityComboBox_SelectedIndexChanged;
            this.QualityComboBox.SelectedItem = quality.ToString();
        }

        private void QualityComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.quality = Enum.Parse<AudioQuality>(this.QualityComboBox.SelectedItem as string, true);
        }

        public void RunAction(MixDownCollection mixDowns, 
                              string targetDirectory,
                              IAudioService audioService, 
                              IMessageService messageService, 
                              IDirectoryService directoryService,
                              Func<int, string, bool> onProgress)
        {
           for (var i = 0; i < mixDowns.Count; i++)
           {
               var state = onProgress.Invoke((i+1), Path.GetFileNameWithoutExtension(mixDowns[i].FileName));
               audioService.ConvertToMp3(mixDowns[i], targetDirectory, this.quality);
           }
           onProgress.Invoke(0, "All Done");
        }
    }
}
