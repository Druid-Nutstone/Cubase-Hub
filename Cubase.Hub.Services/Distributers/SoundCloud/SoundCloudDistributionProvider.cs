using Cubase.Hub.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public class SoundCloudDistributionProvider : HttpClient, IDistributionProvider
    {
        private static string ClientID = "udGnZB2s50obwNCXiR0DvaSpF0Jc5ncq";

        private static string ClientSecret = "M8OT0EVjBTrXpDB9HnnYUCmpDy4VLsut";

        private static string CallBack = "http://localhost:5111/callback";

        private string AuthCode = string.Empty;

        private string CodeChallenge = string.Empty;

        private string CodeVerifier = string.Empty;

        private string AccessToken = string.Empty;

        private static string BaseAddressString = "https://api.soundcloud.com";

        public MixDownCollection? GetTracks(Action<string> onError)
        {
            MixDownCollection tracks = new MixDownCollection();
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

            return tracks;
        }

        public void Initialise()
        {
            this.BaseAddress = new Uri(BaseAddressString);
            this.AuthCode = this.ConnectToSoundCloudOAuth();
            this.AccessToken = this.GetToken();
            this.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", this.AccessToken);

            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private string GetToken()
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
            var tokenResponse = response.GetModel<SoundCloudTokenResponse>((err) => { });
            return tokenResponse.AccessToken;
        }

        private string ConnectToSoundCloudOAuth()
        {
            var code = string.Empty;

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
            while (string.IsNullOrEmpty(code))
            {
                Thread.Sleep(1000);
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
    }
}
