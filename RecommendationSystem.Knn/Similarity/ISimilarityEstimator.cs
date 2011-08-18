namespace RecommendationSystem.Knn.Similarity
{
    public interface ISimilarityEstimator
    {
        float Similarity(User.User first, User.User second);
    }
}
