using System.Collections.Generic;
using RecommendationSystem.Entities;

namespace RecommendationSystem.Knn.Users
{
    public interface IKnnUser : IUser
    {
        float AverageRating { get; set; }
        Dictionary<int, IRating> RatingsByArtistIndexLookupTable { get; set; }
        List<int> ArtistIndices { get; set; }
    }
}