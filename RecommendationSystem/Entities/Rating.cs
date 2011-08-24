using System;

namespace RecommendationSystem.Entities
{
    public class Rating : IRating
    {
        public int UserIndex { get; set; }
        public int ArtistIndex { get; set; }
        public float Value { get; set; }

        public Rating()
        {}

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

#if DEBUG
        public override string ToString()
        {
            return string.Format("-\t{0}\t{1}{2}", ArtistIndex, Value, Environment.NewLine);
        }
#endif
    }
}