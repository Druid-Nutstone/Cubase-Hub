namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public class SoundCloudAutoDiscoveryService : IDistributerAutoDiscoveryService
    {
        private IServiceProvider serviceProvider;

        public SoundCloudAutoDiscoveryService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

        }

        public void StartAutoDiscovery()
        {
            Task.Run(() =>
            {
                this.Start();
            });
        }

        private void Start()
        {

        }
    }
}
