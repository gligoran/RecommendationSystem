using RecommendationSystem.Knn.Foundation.Users;

namespace RecommendationSystem.Knn.Foundation.Similarity
{
    public interface ISimilarityEstimator<in TKnnUser>
        where TKnnUser : IKnnUser
    {
        float GetSimilarity(TKnnUser first, TKnnUser second);
    }
}