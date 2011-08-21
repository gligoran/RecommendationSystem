using RecommendationSystem.Knn.Models;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Recommendations
{
    public interface IKnnRecommender : IRecommender<IKnnModel>
    {}
}