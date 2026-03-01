using Cubase.Hub.Services.Distributers;
using Cubase.Hub.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cubse.Hub.Tests.SoundCloud.Tests
{
    [TestClass]
    public class SoundCloudTests : BaseTest
    {
        [TestMethod]
        public void Can_Connect_To_SoundCloud()
        {
            var provider = this.serviceProvider.GetKeyedService<IDistributionProvider>(DistributionProvider.SoundCloud);
            provider?.Initialise();
            var mixCollection = provider?.GetTracks((err) => 
            {
                var error = err;
            });
        }
    }
}
