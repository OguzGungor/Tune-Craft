using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace TuneAndCraft.v5.Util
{
    public class SpotifyHelper
    {

        private const string clientID = "acb41942113b40e5b1627f9d2cc386b6";

        private const string redirectURL = "https://localhost:44395/Spotify/SpotifyAuth";
        public string accessTokenString;
        public string accessTokenType;

        private SpotifyWebAPI client;
        public SpotifyHelper() {
            client = new SpotifyWebAPI();
        }

        public void authenticate() {
            ImplicitGrantAuth auth = new ImplicitGrantAuth(
                          clientID,
                           redirectURL,
                           redirectURL,
                            Scope.UserReadPlaybackState | Scope.UserReadPrivate | Scope.AppRemoteControl | Scope.UserReadCurrentlyPlaying | Scope.UserModifyPlaybackState | Scope.UserLibraryRead
                         );
            auth.AuthReceived += async (sender, payload) =>
            {
                auth.Stop();
                client = new SpotifyWebAPI()
                {
                    TokenType = payload.TokenType,
                    AccessToken = payload.AccessToken,

                };
                accessTokenString = client.AccessToken;
                accessTokenType = client.TokenType;


            };

            auth.OpenBrowser();

        }

        public async Task<List<FullTrack>> searchSong(string songName, string token, string tokenType) {
            List<FullTrack> temp = new List<FullTrack>();
            client.AccessToken = token;
            client.TokenType = tokenType;
            var tracks = await client.SearchItemsAsync(songName, SearchType.Track);


            /* if (tracks.HasError())
             {
                 if (tracks.Error.Message == "The access token expired") {

                 }
             }*/

            if (tracks.Tracks != null)
            {

                tracks.Tracks.Items.ForEach(t => temp.Add(t));

                //var asd = client.SearchItemsEscaped(songName, SearchType.Track);

                return temp;
            }
            else {
                return null;
            }

        }

        public Boolean checkAuth() {
            client.AccessToken = accessTokenString;
            client.TokenType = accessTokenType;
            var temp = client.GetPlayback();

            if (temp.HasError()) {
                /*if (temp.Error.Message == "The access token expired") {
                   return false;
                }*/
                if (temp.Error.Message == "The access token expired") {
                    return false;
                }
                //return temp.Error.Message;

            }
            //return true;
            //return "done!";
            return true;
        }

        public async Task<string> PauseSong() {

            client.AccessToken = accessTokenString;
            client.TokenType = accessTokenType;
            var response = await client.PausePlaybackAsync();
            if (response.HasError())
                return response.Error.Message;
            else
                return "done";
        }

        public async Task<string> PlaySong(string source = null) {
            client.AccessToken = accessTokenString;
            client.TokenType = accessTokenType;
            ErrorResponse response;

            if (source != null)
            {
                response = await client.AddToQueueAsync(source);
                response = await client.SkipPlaybackToNextAsync();
            }
            else
            {
                response = await client.ResumePlaybackAsync(offset: (int?)null);
            }

            if (response.HasError())
            {
                return response.Error.Message;
            }
            else
            {
                return "done";
            }
        }

        public async Task<string> getTime() {

            client.AccessToken = accessTokenString;
            client.TokenType = accessTokenType;
            var context = await client.GetPlaybackAsync();
            int seconds = context.ProgressMs / 1000;
            int minutes = seconds / 60;
            seconds = seconds % 60;

            string result = minutes.ToString("00") + " : " + seconds.ToString("00");

            return result;


        }

        public async Task<String> checkStatus() {
            client.AccessToken = accessTokenString;
            client.TokenType = accessTokenType;
            var context = await client.GetPlaybackAsync();
            if(context.Item != null) { 
                if (context.ProgressMs < context.Item.DurationMs - 10000)
                {
                    return "false";
                }
                else
                {
                    return "True";
                }
            }
            else{
                return "Spotify player is not active";
                
            }

        }

        public async Task<string> getImage(string source) {

            client.AccessToken = accessTokenString;
            client.TokenType = accessTokenType;
            var context = await client.GetTrackAsync(source.Substring(14));
            if(context.Album != null) { 
                return context.Album.Images.First().Url;
            }
            else {
                return "no Spotify connection";
            }
        }

        public async Task<List<SavedTrack>> getSavedTracks(){
            client.AccessToken = accessTokenString;
            client.TokenType = accessTokenType;
            Paging<SavedTrack> savedTracks = await client.GetSavedTracksAsync();
            return savedTracks.Items;

        }
        

            public async Task<List<SimplePlaylist>> getPlaylists(){
            client.AccessToken = accessTokenString;
            client.TokenType = accessTokenType;
            PrivateProfile user = await client.GetPrivateProfileAsync();
            Paging<SimplePlaylist> savedAlbums= await client.GetUserPlaylistsAsync(user.Id);
            return savedAlbums.Items;

        }

        public async Task<FullPlaylist> getPlaylist(string id)
        {
            client.AccessToken = accessTokenString;
            client.TokenType = accessTokenType;
            FullPlaylist playlist = await client.GetPlaylistAsync(id);
            return playlist;

        }


    }
}