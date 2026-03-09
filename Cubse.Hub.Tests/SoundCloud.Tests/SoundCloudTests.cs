using Cubase.Hub.Services.Distributers;
using Cubase.Hub.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Cubse.Hub.Tests.SoundCloud.Tests
{
    [TestClass]
    public class SoundCloudTests : BaseTest
    {
        private string albumTitle = "Martin";

        private string trackLocation = "C:\\Deleteme\\AlbumMixes\\Martin\\oldnumber7-Master.wav";

        [TestMethod]
        public void Can_Connect_To_SoundCloud()
        {
            var connected = this.soundCloudDistributionProvider.Connect((err) => 
            { 
               Debug.WriteLine(err);
            });    
            Assert.IsTrue(connected);
        }

        [TestMethod]
        public void TestAlbum_Creation()
        {
            this.configurationService.LoadConfiguration(() => { });
            var albumLoc = this.albumService.GetAlbumList(this.OnError).FirstOrDefault(x => x.AlbumName == "Martin");
            var albumConfiguration = this.albumService.GetAlbumConfigurationFromAlbumLocation(albumLoc);
            var mixDowns = this.trackService.GetMixesForAlbum(albumLoc);
            var allContributers = string.Join(" ",(string.Join(' ', mixDowns.Select(x => string.Join(" ",x.Performers.Split(';'))))).Split(" ").Distinct());
            var performers = string.Join(" ", string.Join("", allContributers).Split(";").Distinct());
            var x = string.Join(Environment.NewLine, new[]
            {
               $"Performed  by: {performers}",
               $"Engineered by: {albumConfiguration.Engineer}",
               $"Produced   by: {albumConfiguration.Producer}",
               $"{albumConfiguration.Comments}"
            });
        }

        [TestMethod]
        public void Can_Compare_dates()
        {
            if (this.soundCloudDistributionProvider.Connect(this.OnError))
            {
                var tacks = this.soundCloudDistributionProvider.GetTracks(this.OnError);
                /*
                var playLists = this.soundCloudDistributionProvider.GetPlayLists(this.OnError);
                var playList = playLists.GetAlbum("In Progress");
                if (playList !=null)
                {
                    
                }
                */
            }
        }

        [TestMethod]
        public void Can_Create_Album()
        {
            if (this.soundCloudDistributionProvider.Connect(this.OnError))
            {
                var playLists = this.soundCloudDistributionProvider.GetPlayLists(this.OnError);
                var playList = playLists.GetAlbum(this.albumTitle);
                if (playList != null)
                {
                    this.soundCloudDistributionProvider.DeleteAlbum(playList, this.OnError);
                }

                var albumConfig = new AlbumConfiguration()
                {
                    Title = albumTitle,
                    Comments = "this is a nice album guv",
                    Year = 2026,
                    Genre = "Punk",
                    Artist = "David",
                };
                var response = this.soundCloudDistributionProvider.CreateAlbum(albumConfig, "", this.OnError);

                var track = this.trackService.PopulateTagsFromFile(this.trackLocation);

                this.soundCloudDistributionProvider.UploadTrack(track, this.OnError);
            }
        }

        [TestMethod]
        public void Can_Delete_Album()
        {
            if (this.soundCloudDistributionProvider.Connect(this.OnError))
            {
                var playLists = this.soundCloudDistributionProvider.GetPlayLists(this.OnError);
                var playList = playLists.GetAlbum(this.albumTitle);
                if (playList != null)
                {
                    this.soundCloudDistributionProvider.DeleteAlbum(playList, this.OnError); 
                }
            }
        }

        [TestMethod]
        public void Can_Get_playlists()
        {
            if (this.soundCloudDistributionProvider.Connect(this.OnError))
            {
                var playList = this.soundCloudDistributionProvider.GetPlayLists(this.OnError);
                var saveTo = JsonSerializer.Serialize(playList, new JsonSerializerOptions() { WriteIndented = true });
                File.WriteAllText(@"C:\deleteme\playlist.json", saveTo);
             }
        }


        private void OnError(string error)
        {
            Debug.WriteLine(error);
        }
    }
}
