using System;

namespace RecommendationSystem.Knn.Recommendations
{
    public class Recommendation : IComparable<Recommendation>
    {
        public string Artist { get; set; }
        public float Rating { get; set; }

        public Recommendation(string artist, float rating)
        {
            Artist = artist;
            Rating = rating;
        }

        public int CompareTo(Recommendation other)
        {
            return Rating > other.Rating ? -1 : (Rating < other.Rating ? 1 : 0);
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Artist, Rating);
        }
    }
}
