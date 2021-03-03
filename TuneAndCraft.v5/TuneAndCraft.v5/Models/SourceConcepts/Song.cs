using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace TuneAndCraft.v5.Models.SourceConcepts
{
    public class Song
    {
        [Key]
        public int SongID { get; set; }
        public string SongName { get; set; }
        public int SourceType { get; set; }
        public string SourceData { get; set; }


        public int PlaylistID { get; set; }
        public Playlist Playlist { get; set; }

    }
}