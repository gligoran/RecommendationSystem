using System.Collections.Generic;
using RecommendationSystem.Entities;

namespace RecommendationSystem.QualityTesting
{
    public class RatingByArtistComparer : IComparer<IRating>, IEqualityComparer<IRating>
    {
        public int Compare(IRating first, IRating second)
        {
            return first.ArtistIndex.CompareTo(second.ArtistIndex);
        }

        public bool Equals(IRating first, IRating second)
        {
            return first.ArtistIndex.Equals(second.ArtistIndex);
        }

        public int GetHashCode(IRating rating)
        {
            return (rating != null ? rating.ArtistIndex.GetHashCode() : 0);
        }
    }
}