using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuneAndCraft.v5.Data;
using TuneAndCraft.v5.Models.SourceConcepts;
using TuneAndCraft.v5.Util;

namespace TuneAndCraft.v5.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private SourceContext db = new SourceContext();

        // GET: Library
        
        public ActionResult Index()
        {


            /* DBHelper temp = new DBHelper();            
             var id = temp.getLibraryID(Request,User);
            */

            var id = DBHelper.GetLibraryID(Request, User);
            var playlists = db.Playlists.Where(p => p.Library.LibraryID == id).Include(p => p.Library);

            return View(playlists.ToList());
        }

        // GET: Library/Details/5
       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Song> playlist = DBHelper.getPlaylist(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlaylistID = id;
            ViewBag.playListName = db.Playlists.Find(id).PlaylistName;
            return View(playlist);
        }

        // GET: Library/Create
        public ActionResult Create()
        {
            //ViewBag.LibraryID = new SelectList(db.Libraries, "LibraryID", "LibraryID");
            return View();
        }

        // POST: Library/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlaylistName,SongQuantity")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                playlist.PlaylistID = DBHelper.GetNewPlaylistID(db);
                playlist.LibraryID = db.Libraries.Find(DBHelper.GetUserManager(Request).FindById(User.Identity.GetUserId()).LibraryID).LibraryID;
                                             

                //db.Libraries.f

                db.Playlists.Add(playlist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LibraryID = new SelectList(db.Libraries, "LibraryID", "LibraryID", playlist.LibraryID);
            return View(playlist);
        }

        // GET: Library/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playlist playlist = db.Playlists.Find(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            ViewBag.LibraryID = new SelectList(db.Libraries, "LibraryID", "LibraryID", playlist.LibraryID);
            return View(playlist);
        }

        // POST: Library/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlaylistID,PlaylistName,SongQuantity,LibraryID")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playlist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LibraryID = new SelectList(db.Libraries, "LibraryID", "LibraryID", playlist.LibraryID);
            return View(playlist);
        }

        // GET: Library/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playlist playlist = db.Playlists.Find(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            return View(playlist);
        }

        // POST: Library/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Playlist playlist = db.Playlists.Find(id);
            db.Playlists.Remove(playlist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddSong() {

            return View();
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
