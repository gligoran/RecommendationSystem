using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public interface ISimilarityEstimator
    {
        float GetSimilarity(IUser first, IKnnUser second);
    }
}