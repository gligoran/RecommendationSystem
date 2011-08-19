using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Similarity
{
    public interface ISimilarityEstimator
    {
        float Similarity(User first, User second);
    }
}
