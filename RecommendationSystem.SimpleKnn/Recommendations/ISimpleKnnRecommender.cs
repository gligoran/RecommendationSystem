using RecommendationSystem.Knn.Foundation.Recommendations;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Recommendations
{
    public interface ISimpleKnnRecommender : IKnnRecommender<ISimpleKnnModel, ISimpleKnnUser>
    {}
}