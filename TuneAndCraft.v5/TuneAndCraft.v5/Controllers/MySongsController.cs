using Microsoft.Ajax.Utilities;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuneAndCraft.v5.Data;
using TuneAndCraft.v5.Models.SourceConcepts;
using TuneAndCraft.v5.Util;

namespace TuneAndCraft.v5.Controllers
{
    [Authorize]
    public class MySongsController : Controller
    {
        private SourceContext db = new SourceContext();

        // GET: Playlist
        public ActionResult Index()
        {

            /*var songs = db.Songs.Include(s => s.Playlist);
            return View(songs.ToList());*/
            var songs = DBHelper.getSongs(Request, User, db);
            return View(songs);
        }

        // GET: Playlist/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // GET: Playlist/Create
        /*public ActionResult CreateSpotify()
        {
            ViewBag.PlaylistID = new SelectList(db.Playlists, "PlaylistID", "PlaylistName");
            return View();
        }*/

        public ActionResult SearchSpotify()
        {
            ViewBag.PlaylistID = new SelectList(db.Playlists, "PlaylistID", "PlaylistName");
            return View();
        }

        // POST: Playlist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SearchSpotify([Bind(Include = "SongName")] Song song)
        {
            SpotifyHelper temp = new SpotifyHelper();
            //temp.authanticate();
            List<FullTrack> songList = await temp.searchSong(song.SongName,DBHelper.getTokenString(Request,User),DBHelper.getTokenType(Request,User));
            if (songList == null) {
                ViewBag.error = "cannot access Spotify account";
            }
            //return RedirectToAction("ViewSpotifySearchResults","Playlist", new { songs = songList , count = songList.Count }));
            return View("ViewSpotifySearchResults", songList);
        }

        public ActionResult AddSong(string songName, int sourceType, string sourceData)
        {
            var id = DBHelper.GetLibraryID(Request, User);
            ViewBag.PlaylistID = new SelectList(db.Playlists.Where(p=> p.LibraryID == id), "PlaylistID", "PlaylistName");
            Song temp = new Song()
            {
                
                SongName = songName,
                SourceType = sourceType,
                SourceData = sourceData
            };
            return View(temp);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSong([Bind(Include = "SongName,SourceType,SourceData,PlaylistID")] Song song)
        {
            if (ModelState.IsValid)
            {
                db.Songs.Add(song);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaylistID = new SelectList(db.Playlists, "PlaylistID", "PlaylistName", song.PlaylistID);
            return View(song);
        }

        /* public ActionResult AddSong(string songName, int sourceType, string sourceData) {
             if (ModelState.IsValid) {
                 Song temp = new Song()
                 {
                     SongName = songName,
                     SourceType = sourceType,
                     SourceData = sourceData
                 };
                 db.Songs.Add(temp);
                 db.SaveChanges();


             }
             return RedirectToAction("Index");

         }*/
        public ActionResult ViewSpotifySearchResults(List<FullTrack> songs , int count) {
            ViewBag.count = count;
            //ViewBag.temp = songs.ElementAt(0).Name;
            return View(songs);
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSpotify([Bind(Include = "SongName,SourceType,SourceData,PlaylistID")] Song song)
        {
            if (ModelState.IsValid)
            {
                db.Songs.Add(song);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaylistID = new SelectList(db.Playlists, "PlaylistID", "PlaylistName", song.PlaylistID);
            return View(song);
        }

        public ActionResult CreateYoutube(int? id )
        {
            if (id == null) {
                ViewBag.PlaylistID = new SelectList(db.Playlists, "PlaylistID", "PlaylistName");
            }
            return View();
        }

        // POST: Playlist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateYoutube([Bind(Include = "SongID,SongName,SourceType,SourceData,PlaylistID")] Song song)
        {
            if (ModelState.IsValid)
            {
                song.SourceType = 1;
                db.Songs.Add(song);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlaylistID = new SelectList(db.Playlists, "PlaylistID", "PlaylistName", song.PlaylistID);
            return View(song);
        }

        // GET: Playlist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlaylistID = new SelectList(db.Playlists, "PlaylistID", "PlaylistName", song.PlaylistID);
            return View(song);
        }

        // POST: Playlist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SongID,SongName,SourceType,SourceData,PlaylistID")] Song song)
        {
            if (ModelState.IsValid)
            {
                db.Entry(song).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlaylistID = new SelectList(db.Playlists, "PlaylistID", "PlaylistName", song.PlaylistID);
            return View(song);
        }

        // GET: Playlist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Playlist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Song song = db.Songs.Find(id);
            db.Songs.Remove(song);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
