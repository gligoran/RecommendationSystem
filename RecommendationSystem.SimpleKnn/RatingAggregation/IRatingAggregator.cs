using System.Collections.Generic;
using RecommendationSystem.SimpleKnn.Similarity;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.RatingAggregation
{
    public interface IRatingAggregator
    {
        float Aggregate(ISimpleKnnUser user, List<SimilarUser> neighbours, int artistIndex);
    }
}