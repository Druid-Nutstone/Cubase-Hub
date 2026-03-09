using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Audio;
using Cubase.Hub.Services.Config;
using Cubase.Hub.Services.Distributers;
using Cubase.Hub.Services.Distributers.SoundCloud;
using Cubase.Hub.Services.FileAndDirectory;
using Cubase.Hub.Services.Models;
using Cubase.Hub.Services.Projects;
using Cubase.Hub.Services.Track;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubse.Hub.Tests
{
    public class BaseTest
    {
        protected IConfigurationService configurationService;

        protected IProjectService projectService;

        protected IServiceProvider serviceProvider;

        protected ITrackService trackService;

        protected IAlbumService albumService;

        
        protected SoundCloudDistributionProvider soundCloudDistributionProvider;



        public BaseTest()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<IDirectoryService, DirectoryService>()
                .AddSingleton<SoundCloudDistributionProvider>()
                .AddSingleton<IAlbumService, AlbumService>()
                .AddSingleton<ITrackService, TrackService>()
                .AddSingleton<IAudioService, AudioService>()
                .AddSingleton<IProjectService, ProjectService>();
                
            this.serviceProvider = serviceCollection.BuildServiceProvider();    
            this.configurationService = serviceProvider.GetRequiredService<IConfigurationService>();   
            this.projectService = serviceProvider.GetRequiredService<IProjectService>();
            this.trackService = serviceProvider.GetRequiredService<ITrackService>();
            this.albumService = serviceProvider.GetRequiredService<IAlbumService>();    
            this.soundCloudDistributionProvider = serviceProvider.GetRequiredService<SoundCloudDistributionProvider>();
        }
    }
}
