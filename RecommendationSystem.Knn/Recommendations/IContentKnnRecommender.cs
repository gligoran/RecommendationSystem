using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Recommendations
{
    public interface IContentKnnRecommender : IRecommender<IKnnModel>
    {
        IContentSimilarityEstimator ContentSimilarityEstimator { get; set; }
        float RatingSimilarityWeight { get; set; }
        float ContentSimilarityWeight { get; set; }
    }
}