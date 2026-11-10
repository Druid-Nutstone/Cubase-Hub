using NAudio.Wave;

namespace Cubase.Hub.Services.Models
{
    public class AudioCaptureRunner
    {
        private WasapiLoopbackCapture Capture = new WasapiLoopbackCapture();

        private WaveFileWriter WaveWriter;

        private AudioCaptureProgress progress;

        private Action<AudioCaptureProgress> OnProgress;

        public AudioCaptureRunner(string fileName, Action<AudioCaptureProgress> onProgress)
        {
            this.progress = new AudioCaptureProgress() { Status = AudioCaptureStatus.Waiting };
            this.OnProgress = onProgress;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            this.WaveWriter = new WaveFileWriter(fileName, Capture.WaveFormat);
            this.Capture.DataAvailable += this.OnCaptureData;
            this.Capture.RecordingStopped += this.OnCaptureEnded;
        }

        private void OnCaptureEnded(object? sender, StoppedEventArgs e)
        {
            this.WaveWriter.Dispose();
            this.Capture.Dispose();
            this.SendProgress(AudioCaptureStatus.Complete);
        }

        private void OnCaptureData(object? e, WaveInEventArgs args)
        {
            this.WaveWriter.Write(args.Buffer, 0, args.BytesRecorded);
            this.progress.TotalBytes += args.BytesRecorded;
            this.SendProgress(AudioCaptureStatus.Recording);
        }

        private void SendProgress(AudioCaptureStatus status)
        {
            this.progress.Status = status;
            this.OnProgress?.Invoke(this.progress);
        }

        public void StartRecording()
        {
            this.Capture.StartRecording();
            this.SendProgress(AudioCaptureStatus.Recording);
        }

        public void StopRecording()
        {
            this.Capture.StopRecording();
        }

    }

    public enum AudioCaptureStatus
    {
        Waiting,
        Recording,
        Error,
        Complete
    }

    public class AudioCaptureProgress
    {
        public int TotalBytes { get; set; } = 0;

        public AudioCaptureStatus Status { get; set; }

        public string Message { get; set; }

    }
}
