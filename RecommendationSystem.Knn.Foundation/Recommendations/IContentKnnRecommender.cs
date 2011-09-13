using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Foundation.Recommendations
{
    public interface IContentKnnRecommender<TModel> : IRecommender<TModel>
        where TModel : IModel
    {
        IContentSimilarityEstimator ContentSimilarityEstimator { get; set; }
        float RatingSimilarityWeight { get; set; }
        float ContentSimilarityWeight { get; set; }
    }
}