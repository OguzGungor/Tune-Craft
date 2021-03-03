using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuneAndCraft.v5.Models.ViewModel;
using TuneAndCraft.v5.Util;

namespace TuneAndCraft.v5.Controllers
{

    public class HomeController : Controller
    {

        public async Task<ActionResult> Index()
        {

            if (Request.IsAuthenticated)
            {

                if (DBHelper.GetUser(Request, User).spotifyAccessToken != null)
                {

                    SpotifyHelper temp = new SpotifyHelper()
                    {
                        accessTokenString = DBHelper.getTokenString(Request, User),
                        accessTokenType = DBHelper.getTokenType(Request, User)
                    };
                    if (temp.checkAuth() == false)
                    {
                        ViewBag.view = 2;
                        ViewBag.homeMessage = "Spotify connection needs to be refreshed";
                        return View();
                    }
                    else
                    {
                        var songs = await temp.getSavedTracks();
                        var albums = await temp.getPlaylists();
                        HomeModel model = new HomeModel();
                        model.playlists = albums;
                        model.favouriteSongs = songs;
                        ViewBag.view = 3;

                        return View(model);


                    }
                }
                else
                {

                    ViewBag.homeMessage = "No Spotify connection";
                    ViewBag.view = 2;
                    return View();
                }
            }
            else
            {
                return View("Index2");
            }

        }

        [Authorize]
        public async Task<ActionResult> Playlist(string id)
        {
            SpotifyHelper temp = new SpotifyHelper()
            {
                accessTokenString = DBHelper.getTokenString(Request, User),
                accessTokenType = DBHelper.getTokenType(Request, User)
            };
            var songs = await temp.getPlaylist(id);
            ViewBag.playlist = songs.Name;

            return View(songs.Tracks.Items);
        }
    }
}