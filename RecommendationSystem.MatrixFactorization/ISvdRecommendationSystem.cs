using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.MatrixFactorization
{
    public interface ISvdRecommendationSystem<TSvdModel> : IRecommendationSystem<TSvdModel, IUser, ITrainer<TSvdModel, IUser>, IRecommender<TSvdModel>>
        where TSvdModel : ISvdModel
    {}
}