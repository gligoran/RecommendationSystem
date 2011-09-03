using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Similarity;

namespace RecommendationSystem.SimpleKnn.Recommendations
{
    public interface IContentSimpleKnnRecommender : IRecommender<ISimpleKnnModel>
    {
        IContentSimilarityEstimator ContentSimilarityEstimator { get; set; }
        float RatingSimilarityWeight { get; set; }
        float ContentSimilarityWeight { get; set; }
    }
}