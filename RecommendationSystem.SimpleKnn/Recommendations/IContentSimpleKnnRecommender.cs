using RecommendationSystem.Knn.Foundation.Recommendations;
using RecommendationSystem.SimpleKnn.Models;

namespace RecommendationSystem.SimpleKnn.Recommendations
{
    public interface IContentSimpleKnnRecommender : IContentKnnRecommender<ISimpleKnnModel>
    {}
}