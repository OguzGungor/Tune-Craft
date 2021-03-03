using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuneAndCraft.v5.Models.SourceConcepts
{
    public class Library
    {
        [Key]
        public int LibraryID { get; set; }


        public List<Playlist> Playlist { get; set; }
    }
}