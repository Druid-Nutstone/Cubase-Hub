using Cubase.Hub.Services.Models;
using System.IO;

namespace Cubase.Macro.Forms.AudioCapture
{
    public partial class AudioCaptureForm : BaseWindows11Form
    {
        private string StartRecordingText = "Start Recording";

        private string StopRecordingText = "Stop Recording";

        private AudioCaptureRunner audioCapture;

        public AudioCaptureForm()
        {
            InitializeComponent();
            ThemeApplier.ApplyDarkTheme(this);
            this.SetRecordingStartButton();
            this.RecordingButton.Click += RecordingButton_Click;
            this.FolderBrowseButton.Click += FolderBrowseButton_Click;
        }

        private void FolderBrowseButton_Click(object? sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() != DialogResult.Cancel)
            {
                this.FolderName.Text = folderDialog.SelectedPath;
            }
        }

        private void RecordingButton_Click(object? sender, EventArgs e)
        {
            if (this.RecordingButton.Text == StartRecordingText)
            {
                this.StartRecording();

            }
            else
            {
                this.StopRecording();
            }
        }

        private void StartRecording()
        {
            if (string.IsNullOrEmpty(this.FileName.Text) || string.IsNullOrEmpty(this.FolderName.Text))
            {
                MessageBox.Show("You must enter the file name and folder path");
                this.SetRecordingStartButton();
                return;
            }

            var wavFile = this.FileName.Text;
            var wavDirectory = this.FolderName.Text;

            if (!wavFile.ToLower().EndsWith(".wav"))
            {
                wavFile = wavFile += ".wav";
            }
            this.SetRecordingStopButton();
            var targetFile = Path.Combine(this.FolderName.Text, wavFile);
            audioCapture = new AudioCaptureRunner(targetFile, this.OnAudioCaptureProgress);
            audioCapture.StartRecording();
        }

        private void OnAudioCaptureProgress(AudioCaptureProgress progress)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<AudioCaptureProgress>(OnAudioCaptureProgress), progress);
                return; // Important: Stop execution here so the background thread exits
            }

            // --- Safe to update UI elements below this line ---
            this.Progress.Text = $"🎵 Bytes captured: {progress.TotalBytes} Status: {progress.Status.ToString()}";
            this.Progress.Refresh();

            if (progress.Status == AudioCaptureStatus.Complete || progress.Status == AudioCaptureStatus.Error)
            {
                this.SetRecordingStartButton();
            }
        }

        private void StopRecording()
        {
            this.audioCapture.StopRecording();
        }

        private void SetRecordingStartButton()
        {
            this.RecordingButton.Text = StartRecordingText;
            this.RecordingButton.BackColor = Color.Green;
            this.RecordingButton.ForeColor = Color.Black;
            this.FileName.Text = string.Empty;
            this.FolderName.Text = string.Empty;
        }

        private void SetRecordingStopButton()
        {
            this.RecordingButton.Text = StopRecordingText;
            this.RecordingButton.BackColor = Color.Red;
            this.RecordingButton.ForeColor = Color.White;
        }

    }
}
