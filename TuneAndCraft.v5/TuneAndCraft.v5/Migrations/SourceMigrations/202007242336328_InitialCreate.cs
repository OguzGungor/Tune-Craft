namespace TuneAndCraft.v5.Migrations.SourceMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Libraries",
                c => new
                    {
                        LibraryID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.LibraryID);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        PlaylistID = c.Int(nullable: false, identity: true),
                        PlaylistName = c.String(),
                        SongQuantity = c.Int(nullable: false),
                        LibraryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlaylistID)
                .ForeignKey("dbo.Libraries", t => t.LibraryID, cascadeDelete: true)
                .Index(t => t.LibraryID);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        SongID = c.Int(nullable: false, identity: true),
                        SongName = c.String(),
                        SourceType = c.Int(nullable: false),
                        SourceData = c.String(),
                        PlaylistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SongID)
                .ForeignKey("dbo.Playlists", t => t.PlaylistID, cascadeDelete: true)
                .Index(t => t.PlaylistID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Songs", "PlaylistID", "dbo.Playlists");
            DropForeignKey("dbo.Playlists", "LibraryID", "dbo.Libraries");
            DropIndex("dbo.Songs", new[] { "PlaylistID" });
            DropIndex("dbo.Playlists", new[] { "LibraryID" });
            DropTable("dbo.Songs");
            DropTable("dbo.Playlists");
            DropTable("dbo.Libraries");
        }
    }
}
