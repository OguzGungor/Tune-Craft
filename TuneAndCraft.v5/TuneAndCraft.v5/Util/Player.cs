

using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using TuneAndCraft.v5.Data;
using TuneAndCraft.v5.Models.SourceConcepts;

namespace TuneAndCraft.v5.Util
{
    public class Player
    {
        public const int _SHUFFLE = 1;
        public const int _IN_ORDER = 0;

        public static Song getNewSongPlaylist(int option,int? playlistID = null, int? currentSongID = null )
        {


            SourceContext temp = new SourceContext();
            List<int> songIDs = new List<int>();
            


            if (playlistID != null) {
                int id = (int)playlistID;
                temp.Songs.Where(s=> s.PlaylistID == id).ForEach(s => songIDs.Add(s.SongID));
                var nextSongID = currentSongID;
                if (currentSongID != null)
                {
                    if (songIDs.Count == 1) {
                        nextSongID = currentSongID;
                    }
                    else
                    {
                        if (option == _SHUFFLE)
                        {
                            nextSongID = currentSongID;
                            var randomGenerator = new Random();

                            while (nextSongID == currentSongID)
                            {
                                nextSongID = randomGenerator.Next(songIDs.Count);
                                nextSongID = songIDs[(int)nextSongID];
                            }

                            

                        }
                        else if (option == _IN_ORDER)
                        {

                            var index = songIDs.IndexOf((int)currentSongID) + 1;
                            if (index == songIDs.Count) {
                                index = 0;
                            }
                            nextSongID = songIDs[index];
                            
                        }
                    }
                }
                else
                {
                    nextSongID = songIDs[0];
                    
                }

                var song = temp.Songs.Find(nextSongID);
                if (song == null) {
                    song = new Song() { Playlist = null, PlaylistID = 0, SongName = "", SongID = 0, SourceData = "asd", SourceType = (int)playlistID };
                }
                
                return song;


            }
            else {

                temp.Songs.ForEach(s => songIDs.Add(s.SongID));
                int nextSongID = 0;
                if (currentSongID != null)
                {

                    if (option == _SHUFFLE)
                    {
                        nextSongID = (int)currentSongID;
                        var randomGenerator = new Random();

                        while (nextSongID == currentSongID)
                        {
                            nextSongID = randomGenerator.Next(songIDs.Count);
                            nextSongID = songIDs[nextSongID];
                        }
                        

                    }
                    else if (option == _IN_ORDER)
                    {

                        int index = songIDs.IndexOf((int)currentSongID) + 1;
                        if (index == songIDs.Count)
                        {
                            index = 0;
                        }
                        nextSongID = songIDs[index];
                    }

                }
                else
                {
                    var randomGenerator = new Random();
                    nextSongID = randomGenerator.Next(songIDs.Count);
                    nextSongID = songIDs[nextSongID];
                    
                }
                var song = temp.Songs.Find(nextSongID);

                if (song == null)
                {
                    song = new Song() { Playlist = null, PlaylistID = 0, SongName = "", SongID = 0, SourceData = "asd", SourceType = 1 };
                }
                
                return song;
            }

        }

        public static Song getPrevSongPlaylist(int option, int? playlistID = null, int? currentSongID = null)
        {


            SourceContext temp = new SourceContext();
            List<int> songIDs = new List<int>();



            if (playlistID != null)
            {
                int id = (int)playlistID;
                temp.Songs.Where(s => s.PlaylistID == id).ForEach(s => songIDs.Add(s.SongID));
                var nextSongID = currentSongID;
                if (currentSongID != null)
                {
                    if (songIDs.Count == 1)
                    {
                        nextSongID = currentSongID;
                    }
                    else
                    {
                        if (option == _SHUFFLE)
                        {
                            nextSongID = currentSongID;
                            var randomGenerator = new Random();

                            while (nextSongID == currentSongID)
                            {
                                nextSongID = randomGenerator.Next(songIDs.Count);
                                nextSongID = songIDs[(int)nextSongID];
                            }



                        }
                        else if (option == _IN_ORDER)
                        {

                            var index = songIDs.IndexOf((int)currentSongID) - 1;
                            if (index == -1)
                            {
                                index = songIDs.Count -1;
                            }
                            nextSongID = songIDs[index];

                        }
                    }
                }
                else
                {
                    nextSongID = songIDs[0];

                }

                var song = temp.Songs.Find(nextSongID);
                if (song == null)
                {
                    song = new Song() { Playlist = null, PlaylistID = 0, SongName = "", SongID = 0, SourceData = "asd", SourceType = (int)playlistID };
                }

                return song;


            }
            else
            {

                temp.Songs.ForEach(s => songIDs.Add(s.SongID));
                int nextSongID = 0;
                if (currentSongID != null)
                {

                    if (option == _SHUFFLE)
                    {
                        nextSongID = (int)currentSongID;
                        var randomGenerator = new Random();

                        while (nextSongID == currentSongID)
                        {
                            nextSongID = randomGenerator.Next(songIDs.Count);
                            nextSongID = songIDs[nextSongID];
                        }


                    }
                    else if (option == _IN_ORDER)
                    {

                        int index = songIDs.IndexOf((int)currentSongID) + 1;
                        if (index == -1)
                        {
                            index = songIDs.Count - 1;
                        }
                        nextSongID = songIDs[index];
                    }

                }
                else
                {
                    var randomGenerator = new Random();
                    nextSongID = randomGenerator.Next(songIDs.Count);
                    nextSongID = songIDs[nextSongID];

                }
                var song = temp.Songs.Find(nextSongID);

                if (song == null)
                {
                    song = new Song() { Playlist = null, PlaylistID = 0, SongName = "", SongID = 0, SourceData = "asd", SourceType = 1 };
                }

                return song;
            }

        }

        public static Song getSong(int? songID, int? playListID)
        {

            SourceContext temp = new SourceContext();

            if (songID != null)
                return temp.Songs.Find(songID);
            else {
                if (playListID != null)
                    return temp.Songs.Where(s => s.PlaylistID == playListID).First();
                else
                    return null;
            }

        }

        /*public static void play(Song song) { 
        
        
        }*/
        
    }
}