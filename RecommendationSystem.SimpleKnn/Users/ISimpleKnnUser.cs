using System.Collections.Generic;
using RecommendationSystem.Entities;

namespace RecommendationSystem.SimpleKnn.Users
{
    public interface ISimpleKnnUser : IKnnUser
    {
        Dictionary<int, IRating> RatingsByArtistIndexLookupTable { get; set; }
        List<int> ArtistIndices { get; set; }
    }
}