using RecommendationSystem.Knn.Foundation.Models;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Models
{
    public interface ISimpleKnnModel : IKnnModel<ISimpleKnnUser>
    {}
}