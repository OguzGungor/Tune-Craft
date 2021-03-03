using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuneAndCraft.v5.Util;

namespace TuneAndCraft.v5.Controllers
{
    public class SpotifyController : Controller
    {
        // GET: Spotify
        [Authorize]
        public ActionResult Index()
        {
            if (DBHelper.GetUser(Request, User).spotifyAccessToken != null)
            {

                SpotifyHelper temp = new SpotifyHelper()
                {
                    accessTokenString = DBHelper.getTokenString(Request, User),
                    accessTokenType = DBHelper.getTokenType(Request,User)
                };
               if (temp.checkAuth() == false) {
                    ViewBag.Error = temp.checkAuth();

                    ViewBag.AuthValue = 0;
                    //temp.authenticate();
                }
                else
                {

                    ViewBag.AuthValue = 1;
                }
            }
            else {
                ViewBag.AuthValue = 0;
            }
            return View();
        }

        public ActionResult Connect() {

            SpotifyHelper temp = new SpotifyHelper();
            temp.authenticate();
            return RedirectToAction("Index","Spotify");
        
        }

        public ActionResult SpotifyAuth()
        {

            return View();

        }
        public ActionResult SpotifyAuthConfirm(string access_token, string token_type, string expires_in)
        {

            DBHelper.GetUser(Request,User).spotifyAccessToken = access_token;
            DBHelper.GetUser(Request, User).spotifyTokenType = token_type;
            DBHelper.UpdateUserApplicationDB(Request, User);

            return RedirectToAction("Index");
        }
    }
}