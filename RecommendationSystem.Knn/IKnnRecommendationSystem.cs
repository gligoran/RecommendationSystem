using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Knn
{
    public interface IKnnRecommendationSystem : IRecommendationSystem<IKnnModel, IUser, ITrainer<IKnnModel, IUser>, IRecommender<IKnnModel>>
    {}
}