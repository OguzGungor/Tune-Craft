using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuneAndCraft.v5.Models.ViewModel
{
    public class HomeModel
    {
        public List<SavedTrack> favouriteSongs;
        public List<SimplePlaylist> playlists;
    }
}