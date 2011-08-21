using System;

namespace RecommendationSystem.Data.Entities
{
    public class Rating : IRating
    {
        public int UserIndex { get; set; }
        public int ArtistIndex { get; set; }
        public float Value { get; set; }

        public Rating() { }

        public Rating(int userIndex, int artistIndex, float value)
        {
            UserIndex = userIndex;
            ArtistIndex = artistIndex;
            Value = value;
        }

        public IRating Clone()
        {
            return new Rating(UserIndex, ArtistIndex, Value);
        }
    }
}
