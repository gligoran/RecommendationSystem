using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Recommendations;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd
{
    public interface ISimpleSvdRecommendationSystem<TSvdModel> : IRecommendationSystem<TSvdModel, IUser, ISvdTrainer<TSvdModel>, ISvdRecommender<TSvdModel>>
        where TSvdModel : ISvdModel
    {}
}