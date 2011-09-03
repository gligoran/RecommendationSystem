using RecommendationSystem.Entities;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd
{
    public interface ISimpleSvdRecommendationSystem<TSvdModel> : IRecommendationSystem<TSvdModel, IUser, ISvdTrainer<TSvdModel>, ISimpleSvdRecommender<TSvdModel>>
        where TSvdModel : ISvdModel
    {
        void SaveModel(string filename, TSvdModel model);
        TSvdModel LoadModel(string filename);
    }
}