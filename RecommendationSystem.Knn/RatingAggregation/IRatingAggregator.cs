using System.Collections.Generic;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.RatingAggregation
{
    public interface IRatingAggregator
    {
        float Aggregate(IKnnUser user, List<SimilarUser> neighbours, int artistIndex);
    }
}