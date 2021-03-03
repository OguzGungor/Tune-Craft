using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using TuneAndCraft.v5.Data;
using TuneAndCraft.v5.Models;
using TuneAndCraft.v5.Models.SourceConcepts;

namespace TuneAndCraft.v5.Util
{
    public class DBHelper
    {
        /* public ApplicationUserManager getUserManager(HttpRequestBase request) {
             return request.GetOwinContext().GetUserManager<ApplicationUserManager>();

         }

         public ApplicationUser GetUser(HttpRequestBase request , System.Security.Principal.IPrincipal user) {
             return getUserManager(request).FindById(user.Identity.GetUserId());
         }

         public int getLibraryID(HttpRequestBase request, System.Security.Principal.IPrincipal user) {

                 return GetUser(request,user).LibraryID;

             }
         */
        public static ApplicationUserManager GetUserManager(HttpRequestBase request)
        {
            return request.GetOwinContext().GetUserManager<ApplicationUserManager>();

        }

        public static ApplicationUser GetUser(HttpRequestBase request, System.Security.Principal.IPrincipal user, string testKey = null)
        {


            return GetUserManager(request).FindById(user.Identity.GetUserId());

        }

        public static int GetLibraryID(HttpRequestBase request, System.Security.Principal.IPrincipal user)
        {

            return GetUser(request, user).LibraryID;

        }

        public static int GetNewPlaylistID(SourceContext db)
        {
            return db.Playlists.Count();
        }

        public static void UpdateUserApplicationDB(HttpRequestBase request, System.Security.Principal.IPrincipal user)
        {
            GetUserManager(request).Update(GetUser(request, user));

        }

        public static string getTokenString(HttpRequestBase request, System.Security.Principal.IPrincipal user)
        {

            return GetUser(request, user).spotifyAccessToken;
        }
        public static string getTokenType(HttpRequestBase request, System.Security.Principal.IPrincipal user)
        {

            return GetUser(request, user).spotifyTokenType;
        }

        public static List<Song> getSongs(HttpRequestBase request, System.Security.Principal.IPrincipal user, SourceContext db)
        {

            var libraryID = GetLibraryID(request, user);
            var playlists = db.Playlists.Where(p => p.LibraryID == libraryID).ToList();
            List<Song> songs = new List<Song>();
            foreach (var item in playlists)
            {
                db.Songs.Where(t => t.PlaylistID == item.PlaylistID).ForEach(s => songs.Add(s));

            }
            return songs;
        }

        public static List<Song> getPlaylist(int? id)
        {
            SourceContext temp = new SourceContext();
            List<Song> songs = new List<Song>();

            temp.Songs.Where(t => t.PlaylistID == id).ForEach(s => songs.Add(s));

            return songs;
        }
    }
}