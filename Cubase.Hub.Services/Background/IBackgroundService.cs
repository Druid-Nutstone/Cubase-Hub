namespace Cubase.Hub.Services.Background
{
    public interface IBackgroundService
    {
        void Start();

        void Pause();

        void Stop();

        void Resume();
    }
}
