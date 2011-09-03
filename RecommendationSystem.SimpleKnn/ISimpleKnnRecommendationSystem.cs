using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.Training;

namespace RecommendationSystem.SimpleKnn
{
    public interface ISimpleKnnRecommendationSystem : IRecommendationSystem<ISimpleKnnModel, IUser, ITrainer<ISimpleKnnModel, IUser>, IRecommender<ISimpleKnnModel>>
    {}
}