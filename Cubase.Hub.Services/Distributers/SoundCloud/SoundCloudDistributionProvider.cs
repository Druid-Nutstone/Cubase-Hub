using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using TagLib.Matroska;
using Cubase.Hub.Services.Track;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public class SoundCloudDistributionProvider : HttpClient
    {
        private readonly IAlbumService albumService;

        private readonly ITrackService trackService;

        private static string ClientID = "udGnZB2s50obwNCXiR0DvaSpF0Jc5ncq";

        private static string ClientSecret = "M8OT0EVjBTrXpDB9HnnYUCmpDy4VLsut";

        private static string CallBack = "http://localhost:5111/callback";

        private string? AuthCode = null;

        private string CodeChallenge = string.Empty;

        private string CodeVerifier = string.Empty;

        private string? AccessToken = null;

        private SoundCloudTokenResponse? AccessTokenResponse;

        private static string BaseAddressString = "https://api.soundcloud.com";

        public bool Connected { get; set; } = false;

        public SoundCloudDistributionProvider(IAlbumService albumService,
                                              ITrackService trackService)
        {
            this.albumService = albumService;
            this.trackService = trackService;
        }

        public bool OrderAlbumTracks(string albumName, Action<string> onError, Action<string> onProgress)
        {
            onProgress.Invoke("Locading Tracks ..");
            var tracks = this.GetTracks(onError);
            if (tracks != null)
            {
                onProgress.Invoke("Loading PlayLists ..");
                var playlists = this.GetPlayLists(onError);
                if (playlists == null)
                {
                    return false;
                }
                var playListAlbum = playlists.GetAlbum(albumName);
                if (playListAlbum != null)
                {
                    onProgress.Invoke($"Locating Album {albumName}");

                    // get all albums 
                    var allMixAlbums = this.albumService.GetAlbumList(onError);
                    if (allMixAlbums != null)
                    {
                        onProgress.Invoke("Re-ordering tracks in the album ..");
                        var targetAlbum = allMixAlbums.FirstOrDefault(x => x.AlbumName == albumName);
                        if (targetAlbum != null)
                        {
                            var trackRefs = new List<TrackRef>();
                            foreach (var item in this.albumService.GetMixesForAlbum(targetAlbum))
                            {
                                var playListTrack = tracks.FirstOrDefault(x => x.Title == item.Title);
                                if (playListTrack != null)
                                {
                                    trackRefs.Add(new TrackRef() { Urn = playListTrack.Urn });
                                }
                            }
                            var trackRequest = new UpdatePlaylistRequest()
                            {
                                Playlist = new PlaylistUpdate()
                                {
                                    Tracks = trackRefs
                                }
                            };
                            var response = this.PutAsJsonAsync<UpdatePlaylistRequest>($"/playlists/{playListAlbum.Urn}", trackRequest).Result;
                            if (!response.IsSuccessStatusCode)
                            {
                                onError.Invoke(response.GetErrorResponse());
                                return false;
                            }
                            return true;
                        }
                    }

                }
            }
            return false;
        }


        public SoundCloudTrack? UploadTrack(MixDown mixDown, Action<string> onError)
        {
            if (!this.EnsureConnectionAndToken(onError))
            {
                return null;
            }

            // delete track if already exists 
            this.DeleteTrack(mixDown, onError);


            // upload the track 

            using var content = new MultipartFormDataContent();

            content?.Add(new StringContent(mixDown.Title), "track[title]");

            var fileStream = System.IO.File.OpenRead(mixDown.FileName);
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(mixDown.GetContentType());

            content?.Add(fileContent, "track[asset_data]", Path.GetFileName(mixDown.FileName));

            HttpResponseMessage? response = null;

            try
            {
                response = this.PostAsync("/tracks", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    onError.Invoke(response.GetErrorResponse());
                    return null;
                }
            } catch (Exception e)
            {
                onError($"Error uploading track {mixDown.Title} {e.Message} ");
                return null;    
            } 

            var newTrack = response?.GetModel<SoundCloudTrack>(onError);

            if (newTrack == null)
            {
                return null;
            }

            // update meta data  

            newTrack = this.UpdateTrackMetaData(mixDown, onError, newTrack);

            if (newTrack == null)
            {
                return null;
            }

            // update cover art 

            var trackArt = this.trackService.GetTrackCoverArt(mixDown);

            if (trackArt != null)
            {
                newTrack = this.UploadTrackArtWork(newTrack, trackArt, onError);
            }

            // add trackref to album

            var playlists = this.GetPlayLists(onError);

            if (playlists != null)
            {
                var playListAlbum = playlists.GetAlbum(mixDown.Album);
                if (playListAlbum == null)
                {
                    onError.Invoke($"Cannot find album {mixDown.Album} so cannot add this track to it");
                    return null;
                }
                var updatedPlaylist = this.AddTrackToAlbum(playListAlbum, newTrack, onError);
                if (updatedPlaylist == null)
                {
                    return null;
                }
                playlists.AddOrUpdatePlayList(updatedPlaylist);
            }
            else
            {
                return null;
            }
            return newTrack;
        }

        public SoundCloudPlaylist? GetAlbum(MixDown mixDown, Action<string> onError)
        {
            var playlists = this.GetPlayLists(onError);
            if (playlists == null)
            {
                return null;
            }
            return playlists.GetAlbum(mixDown.Album);
        }


        public SoundCloudPlaylist? AddTrackToAlbum(SoundCloudPlaylist album, SoundCloudTrack track, Action<string> onError)
        {
            var trackRequest = new UpdatePlaylistRequest()
            {
                Playlist = new PlaylistUpdate()
                {
                    Tracks = album.Tracks.Select(x => new TrackRef() { Urn = x.Urn }).ToList()
                }
            };

            if (trackRequest.Playlist.Tracks.FirstOrDefault(x => x.Urn == track.Urn) == null)
            {

                trackRequest.Playlist.Tracks.Add(new TrackRef() { Urn = track.Urn });

                var response = this.PutAsJsonAsync<UpdatePlaylistRequest>($"/playlists/{album.Urn}", trackRequest).Result;

                if (!response.IsSuccessStatusCode)
                {
                    onError.Invoke(response.GetErrorResponse());
                    return null;
                }
                return response.GetModel<SoundCloudPlaylist>(onError);
            }
            else
            {
                onError.Invoke($"Playlist {album.Title} already contains the track {track.Title}");
                return null;
            }
        }

        public SoundCloudTrack? UpdateTrackMetaData(MixDown mixDown, Action<string> onError, SoundCloudTrack? soundCloudTrack = null)
        {
            if (soundCloudTrack == null)
            {
                var playlists = this.GetPlayLists(onError);
                if (playlists != null)
                {
                    soundCloudTrack = playlists.GetTrack(mixDown.Title);
                }
                else
                {
                    return null;
                }
            }
            if (soundCloudTrack != null)
            {
                var request = UpdateTrackMetaDataRequest.CreateFromMixdown(mixDown);
                var response = this.PutAsJsonAsync<UpdateTrackMetaDataRequest>($"/tracks/{soundCloudTrack.Urn}", request).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.GetModel<SoundCloudTrack>(onError);
                }
                else
                {
                    onError.Invoke(response.GetErrorResponse());
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public SoundCloudPlaylist? CreateAlbum(AlbumConfiguration albumConfiguration, Action<string> onError)
        {
            if (!this.EnsureConnectionAndToken(onError))
            {
                return null;
            }

            var postData = CreatePlaylistRequest.CreateFromAlbum(albumConfiguration);

            var albumArtLocation = this.albumService.GetAlbumArt(albumConfiguration);

            var newPlayList = this.PostAndGetSync<SoundCloudPlaylist, CreatePlaylistRequest>("/playlists", postData, onError);

            if (!string.IsNullOrEmpty(albumArtLocation))
            {
                var checkPlayList = this.UploadPlaylistArtwork(newPlayList.Id.Value, albumArtLocation, onError);
                if (checkPlayList != null)
                {
                    newPlayList = checkPlayList;
                }
            }
            var playLists = this.GetPlayLists(onError);
            playLists.AddOrUpdatePlayList(newPlayList);
            return newPlayList;
        }

        public SoundCloudTrack? UploadTrackArtWork(SoundCloudTrack soundCloudTrack, string artworkPath, Action<string> onError)
        {
            using var form = new MultipartFormDataContent();

            // Artwork file
            var artworkStream = System.IO.File.OpenRead(artworkPath);
            var artworkContent = new StreamContent(artworkStream);
            artworkContent.Headers.ContentType =
                new MediaTypeHeaderValue(artworkPath.GetMimeType());

            form.Add(artworkContent, "track[artwork_data]", Path.GetFileName(artworkPath));

            var response = this.PutAsync($"/tracks/{soundCloudTrack.Id}", form).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.GetModel<SoundCloudTrack>(onError);
            }
            else
            {
                onError.Invoke(response.GetErrorResponse());
                return null;
            }
        }

        public SoundCloudPlaylist? UploadPlaylistArtwork(
                  long playlistId,
                  string imagePath,
                  Action<string>? onError)
        {
            var url = $"/playlists/{playlistId}";
            if (!this.EnsureConnectionAndToken(onError))
            {
                return null;
            }
            try
            {
                using var content = new MultipartFormDataContent();

                // Open image file stream
                using var fileStream = System.IO.File.OpenRead(imagePath);
                var fileContent = new StreamContent(fileStream);

                // Optional but recommended
                fileContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(imagePath.GetMimeType());

                // THIS NAME IS IMPORTANT
                content.Add(fileContent, "playlist[artwork_data]", Path.GetFileName(imagePath));

                var response = this.PutAsync(url, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    onError?.Invoke($"Artwork upload failed: {response.StatusCode}");
                    return null;
                }

                var playList = response.GetModel<SoundCloudPlaylist>(onError);

                return playList;
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
                return null;
            }
        }

        public bool DeleteAlbum(SoundCloudPlaylist soundCloudPlaylist, Action<string> onError, bool deleteTracks = false)
        {
            if (!this.EnsureConnectionAndToken(onError))
            {
                return false;
            }
            var url = $"/playlists/{soundCloudPlaylist.Urn}";
            var response = this.DeleteAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                onError.Invoke(response.GetErrorResponse());
                return false;
            }
            if (deleteTracks)
            {
                return this.DeleteTracks(soundCloudPlaylist.Tracks, onError);
            }
            return true;
        }

        public bool DeleteTrack(MixDown mixDown, Action<string> onError)
        {
            var allTracks = this.GetTracks(onError);
            if (allTracks != null)
            {
                var targetTrack = allTracks.GetTrackByTitle(mixDown.Title);
                if (targetTrack != null)
                {
                    this.DeleteTracks(new List<SoundCloudTrack>() { targetTrack }, onError);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteTracks(List<SoundCloudTrack> tracks, Action<string> onError)
        {
            if (!this.EnsureConnectionAndToken(onError))
            {
                return false;
            }
            foreach (var track in tracks)
            {
                var url = $"/tracks/{track.Urn}";
                var response = this.DeleteAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                {
                    onError?.Invoke($"could not delete track {track.Title} {response.GetErrorResponse()}");
                    return false;
                }
            }
            return true;
        }

        public SoundCloudPlaylistCollection? GetPlayLists(Action<string> onError)
        {
            if (!this.EnsureConnectionAndToken(onError))
            {
                return null;
            }

            var response = this.GetSync("/me/playlists");
            if (!response.IsSuccessStatusCode)
            {
                onError(response.GetErrorResponse());
                return null;
            }
            var playLists = response.GetModel<SoundCloudPlaylistCollection>(onError);

            return playLists;
        }

        public SoundCloudTrackCollection? GetTracks(Action<string> onError)
        {
            if (!this.EnsureConnectionAndToken(onError))
            {
                return default;
            }
            var response = this.GetSync("/me/tracks");
            if (!response.IsSuccessStatusCode)
            {
                onError(response.GetErrorResponse());
                return null;
            }

            string error = string.Empty;
            var soundCloudCollection = response.GetModel<SoundCloudTrackCollection>((err) =>
            {
                error = err;
            });

            if (soundCloudCollection == null)
            {
                onError(error);
                return null;
            }

            return soundCloudCollection;
        }

        public bool Connect(Action<string> onError)
        {
            this.BaseAddress = new Uri(BaseAddressString);

            if (!this.Connected)
            {
                this.AccessTokenResponse = SoundCloudTokenResponse.Load();

                if (this.AccessTokenResponse == null)
                {

                    this.AuthCode = this.ConnectToSoundCloudOAuth(onError);
                    if (this.AuthCode == null) return false;

                    this.AccessToken = this.GetToken(onError);
                    if (this.AccessToken == null) return false;

                }
                else
                {
                    if (this.AccessTokenResponse.HasExpired)
                        this.AccessToken = this.RefreshAccessToken(this.AccessTokenResponse.RefreshToken, onError);
                    else
                        this.AccessToken = this.AccessTokenResponse.AccessToken;
                }



                this.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", this.AccessToken);

                this.DefaultRequestHeaders.Accept.Clear();
                this.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                this.Connected = true;
            }
            return true;
        }

        private string? GetToken(Action<string> onError)
        {

            var content = new FormUrlEncodedContent(new[]
            {
               new KeyValuePair<string, string>("grant_type", "authorization_code"),
               new KeyValuePair<string, string>("client_id", ClientID),
               new KeyValuePair<string, string>("client_secret", ClientSecret),
               new KeyValuePair<string, string>("redirect_uri", CallBack),
               new KeyValuePair<string, string>("code_verifier", this.CodeVerifier),
               new KeyValuePair<string, string>("code", this.AuthCode)
            });

            // Set Accept header
            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Send POST request
            HttpResponseMessage response = this.PostAsync("https://secure.soundcloud.com/oauth/token", content).Result;
            if (response.IsSuccessStatusCode)
            {
                this.AccessTokenResponse = response.GetModel<SoundCloudTokenResponse>((err) => { });
                this.AccessTokenResponse.SetExpires();
                this.AccessTokenResponse.Save();
                return this.AccessTokenResponse.AccessToken;
            }
            else
            {
                onError(response.GetErrorResponse());
                return null;
            }
        }

        private string? RefreshAccessToken(string refreshToken, Action<string> onError)
        {
            var content = new FormUrlEncodedContent(new[]
            {
               new KeyValuePair<string, string>("grant_type", "refresh_token"),
               new KeyValuePair<string, string>("client_id", ClientID),
               new KeyValuePair<string, string>("client_secret", ClientSecret),
               new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            var response = this.PostAsync("https://secure.soundcloud.com/oauth/token", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                onError(response.GetErrorResponse());
                return null;
            }

            var tokenResponse = response.GetModel<SoundCloudTokenResponse>(onError);
            tokenResponse.SetExpires();
            this.AccessTokenResponse = tokenResponse;
            this.AccessTokenResponse.Save();
            this.AccessToken = tokenResponse.AccessToken;

            return this.AccessToken;
        }
        private string? ConnectToSoundCloudOAuth(Action<string> onError)
        {
            string? code = null;

            Task.Run(() =>
            {
                this.StartCallbackListener((result) =>
                {
                    code = result;
                });
            });

            Thread.Sleep(500);

            this.CodeVerifier = PkceHelper.GenerateCodeVerifier();
            this.CodeChallenge = PkceHelper.GenerateCodeChallenge(this.CodeVerifier);
            string state = Guid.NewGuid().ToString("N");

            string authorizeUrl =
                   "https://secure.soundcloud.com/authorize" +
                   "?client_id=" + Uri.EscapeDataString(ClientID) +
                   "&redirect_uri=" + Uri.EscapeDataString(CallBack) +
                   "&response_type=code" +
                   "&code_challenge=" + Uri.EscapeDataString(this.CodeChallenge) +
                   "&code_challenge_method=S256" +
                   "&state=" + state;

            var startBrowser = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = authorizeUrl,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Maximized
                }
            };
            startBrowser.Start();
            var count = 0;
            while (string.IsNullOrEmpty(code))
            {
                Thread.Sleep(1000);
                count++;
                if (count > 30)
                {
                    onError("Could not get authentication code");
                    break;
                }
            }
            return code;
        }

        private void StartCallbackListener(Action<string> OnCode)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5111/");
            listener.Start();

            // This blocks until a request arrives
            var context = listener.GetContext();
            var request = context.Request;

            string code = request.QueryString["code"];

            string responseString = "<html><body>You can close this window.</body></html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();

            listener.Stop();

            OnCode.Invoke(code);
        }

        private HttpResponseMessage GetSync(string url)
        {
            return this.GetAsync(url).Result;
        }

        private T? PostAndGetSync<T, I>(string url, I data, Action<string> onError)
        {
            using var jsonContent = JsonContent.Create(data);
            var response = this.PostAsync(url, jsonContent).Result;
            if (!response.IsSuccessStatusCode)
            {
                onError.Invoke(response.GetErrorResponse());
                return default;
            }
            return response.GetModel<T>(onError);
        }

        private bool EnsureConnectionAndToken(Action<string> onError)
        {
            if (!this.Connected || this.AccessTokenResponse == null)
            {
                onError("The connection to Soundcloud has not been initiated");
                return false;
            }

            if (this.AccessTokenResponse.HasExpired)
            {
                var newToken = this.RefreshAccessToken(this.AccessTokenResponse.RefreshToken, onError);

                if (newToken == null)
                {
                    SoundCloudTokenResponse.Delete();
                    this.Connected = false;
                    return false;
                }

                this.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("OAuth", newToken);
            }

            return true;
        }



    }
}
