using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuneAndCraft.v5.Models.SourceConcepts
{
    public class Playlist
    {
        [Key]
        public int PlaylistID { get; set; }
        public string PlaylistName { get; set; }
        public int SongQuantity { get; set; }

        public int LibraryID { get; set; }
        public Library Library { get; set; }

        
        public List<Song> songs { get; set; }
    }
}