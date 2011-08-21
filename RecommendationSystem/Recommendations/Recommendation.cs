using RecommendationSystem.Entities;

namespace RecommendationSystem.Recommendations
{
    public class Recommendation : IRecommendation
    {
        public IArtist Artist { get; set; }
        public float Rating { get; set; }

        public Recommendation(IArtist artist, float rating)
        {
            Artist = artist;
            Rating = rating;
        }

        public int CompareTo(IRecommendation other)
        {
            return Rating > other.Rating ? -1 : (Rating < other.Rating ? 1 : 0);
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Artist, Rating);
        }
    }
}