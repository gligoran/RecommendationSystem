using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommenderSystem.Data
{
    public class PlayCount : IEquatable<PlayCount>
    {
        public string Artist { get; set; }
        public int Plays { get; set; }

        public PlayCount() { }

        public PlayCount(string artist, int plays)
        {
            this.Artist = artist;
            this.Plays = plays;
        }

        public bool Equals(PlayCount other)
        {
            return this.Artist == other.Artist;
        }
    }
}
