using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecommenderSystem.Knn.Recommendations
{
    public class Recommendation : IComparable<Recommendation>
    {
        public string Artist { get; set; }
        public double Rating { get; set; }

        public Recommendation(string artist, double rating)
        {
            this.Artist = artist;
            this.Rating = rating;
        }

        public int CompareTo(Recommendation other)
        {
            if (this.Rating > other.Rating)
                return -1;

            if (this.Rating < other.Rating)
                return 1;

            return 0;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Artist, Rating);
        }
    }
}
