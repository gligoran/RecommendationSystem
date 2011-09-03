using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Similarity
{
    public interface ISimilarityEstimator
    {
        float GetSimilarity(ISimpleKnnUser first, ISimpleKnnUser second);
    }
}