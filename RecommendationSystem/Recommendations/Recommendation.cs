using RecommendationSystem.Entities;

namespace RecommendationSystem.Recommendations
{
    public class Recommendation : IRecommendation
    {
        public IArtist Artist { get; set; }
        public float Value { get; set; }

        public Recommendation(IArtist artist, float rating)
        {
            Artist = artist;
            Value = rating;
        }

        public int CompareTo(IRecommendation other)
        {
            return Value > other.Value ? -1 : (Value < other.Value ? 1 : 0);
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Artist, Value);
        }
    }
}