using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Users;

namespace RecommendationSystem.SimpleKnn.Users
{
    public interface ISimpleKnnUser : IKnnUser
    {
        Dictionary<int, IRating> RatingsByArtistIndexLookupTable { get; set; }
        List<int> ArtistIndices { get; set; }
    }
}