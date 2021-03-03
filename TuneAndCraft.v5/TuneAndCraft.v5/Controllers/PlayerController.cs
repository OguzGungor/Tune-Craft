using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.ModelBinding;
using System.Web.Mvc;
using TuneAndCraft.v5.Data;
using TuneAndCraft.v5.Models.SourceConcepts;
using TuneAndCraft.v5.Util;

namespace TuneAndCraft.v5.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        // GET: Player
        public const int _PLAYLIST = 0;
        public const int _SONG = 1;

        public async Task<ActionResult> Index(int option = Player._IN_ORDER, int? playlistID = null , int? songID = null, string errorMessage = null)
        {
            List<Song> playlist = null;
            if (playlistID != null)
            {
                SourceContext temp = new SourceContext();
                Session["playlist"] = temp.Playlists.Find(playlistID);
                playlist = DBHelper.getPlaylist(((Playlist)(Session["playlist"])).PlaylistID);
            }
            else
            {
                if (Session["playlist"] != null)
                {
                    playlist = DBHelper.getPlaylist(((Playlist)(Session["playlist"])).PlaylistID);
                }

            }
            var song = Player.getSong(songID, playlistID);
            if (songID != null)
            {
               
                if (song != null)
                {
                    if (song.SourceType == 0)
                    {
                        if (song != ((Song)(Session["song"])))
                        {

                            Session["song"] = song;

                            SpotifyHelper temp = new SpotifyHelper();
                            temp.accessTokenString = DBHelper.getTokenString(Request, User);
                            temp.accessTokenType = DBHelper.getTokenType(Request, User);

                            var response = await temp.PlaySong(((Song)Session["song"]).SourceData);

                            if (response == "Player command failed: No active device found")
                            {
                                Session["song"] = null;
                                ViewBag.error = "Please open your spotify app and press play to activate";
                            }
                        }
                    }
                    else {
                        Pause();
                        Session["song"] = song;
                    }

                }
            }
            else {
                if (Session["song"] == null)
                {
                    if (song != null)
                    {

                        Pause();
                        Session["song"] = song;
                        SpotifyHelper temp = new SpotifyHelper();
                        temp.accessTokenString = DBHelper.getTokenString(Request, User);
                        temp.accessTokenType = DBHelper.getTokenType(Request, User);
                        var response = await temp.PlaySong(((Song)Session["song"]).SourceData);
                        if (response == "Player command failed: No active device found")
                        {
                            Session["song"] = null;
                            ViewBag.error = "Please open your spotify app and press play to activate";
                        }
                    }
                }
                else { 
                }


               


            }

            return View(playlist);
        }

        public void Pause() {

            SpotifyHelper temp = new SpotifyHelper();
            temp.accessTokenString = DBHelper.getTokenString(Request, User);
            temp.accessTokenType = DBHelper.getTokenType(Request, User);
            temp.PauseSong();

        }

        public void Play(string source =null, int? playlistID = null , int? songID = null) {
            
            SpotifyHelper temp = new SpotifyHelper();
            temp.accessTokenString = DBHelper.getTokenString(Request,User);
            temp.accessTokenType = DBHelper.getTokenType(Request, User);
            temp.PlaySong();
        }

        public async Task<ActionResult> Next(int option = Player._IN_ORDER, int? playlistID = null, int? songID = null, string errorMessage = null) {


            if (Player.getSong(songID, playlistID).SourceType == 0){
                SpotifyHelper temp = new SpotifyHelper();
                temp.accessTokenString = DBHelper.getTokenString(Request, User);
                temp.accessTokenType = DBHelper.getTokenType(Request, User);
                await Task.Run(() => temp.PauseSong());

            }
            var song = Player.getNewSongPlaylist(option, playlistID, songID);
            return RedirectToAction("Index" , new { playlistID = playlistID , songID = song.SongID });
        }

        public async Task<ActionResult> Previous(int option = Player._IN_ORDER, int? playlistID = null, int? songID = null, string errorMessage = null) {
            if (Player.getSong(songID, playlistID).SourceType == 0)
            {
                SpotifyHelper temp = new SpotifyHelper();
                temp.accessTokenString = DBHelper.getTokenString(Request, User);
                temp.accessTokenType = DBHelper.getTokenType(Request, User);
                await Task.Run(() => temp.PauseSong());

            }

            var song = Player.getPrevSongPlaylist(option, playlistID, songID);

            return RedirectToAction("Index", new { playlistID = playlistID, songID = song.SongID });
        }

        [HttpPost]
        public async Task<string> getTime() {
             SpotifyHelper temp = new SpotifyHelper();
             temp.accessTokenString = DBHelper.getTokenString(Request, User);
             temp.accessTokenType = DBHelper.getTokenType(Request, User);
            string temp1 = await temp.getTime();
            return temp1;
        }

        [HttpPost]
        public async Task<String> checkStatus()
        {
            SpotifyHelper temp = new SpotifyHelper();
            temp.accessTokenString = DBHelper.getTokenString(Request, User);
            temp.accessTokenType = DBHelper.getTokenType(Request, User);
            String temp1 = await temp.checkStatus();
            return temp1;
            

        }

        [HttpPost]
        public async Task<String> getImage()
        {

            SpotifyHelper temp = new SpotifyHelper();
            temp.accessTokenString = DBHelper.getTokenString(Request, User);
            temp.accessTokenType = DBHelper.getTokenType(Request, User);
            return await temp.getImage(((Song)Session["song"]).SourceData);

        }

    }
}