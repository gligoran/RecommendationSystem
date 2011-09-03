using System.Collections.Generic;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Knn.Foundation.Users;

namespace RecommendationSystem.Knn.Foundation.RatingAggregation
{
    public interface IRatingAggregator<TKnnUser>
        where TKnnUser : IKnnUser
    {
        float Aggregate(TKnnUser user, List<SimilarUser<TKnnUser>> neighbours, int artistIndex);
    }
}