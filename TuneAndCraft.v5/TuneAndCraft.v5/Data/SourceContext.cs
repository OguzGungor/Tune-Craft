using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TuneAndCraft.v5.Models.SourceConcepts;

namespace TuneAndCraft.v5.Data
{
    public class SourceContext : DbContext
    {
        public SourceContext()
        : base("DefaultConnection")
        {
        }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<Library> Libraries { get; set; }
    }
}