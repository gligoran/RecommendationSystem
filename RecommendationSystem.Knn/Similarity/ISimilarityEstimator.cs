using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public interface ISimilarityEstimator
    {
        float GetSimilarity(IKnnUser first, IKnnUser second);
    }
}