using RecommendationSystem.Knn.Similarity;

namespace RecommendationSystem.Knn.Recommendations
{
    public interface IContentKnnRecommender : IKnnRecommender
    {
        IContentSimilarityEstimator ContentSimilarityEstimator { get; set; }
        float RatingSimilarityWeight { get; set; }
        float ContentSimilarityWeight { get; set; }
    }
}