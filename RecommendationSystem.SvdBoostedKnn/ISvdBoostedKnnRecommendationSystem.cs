using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Recommendations;
using RecommendationSystem.SvdBoostedKnn.Training;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn
{
    public interface ISvdBoostedKnnRecommendationSystem<TSvdBoostedKnnModel> : IRecommendationSystem<TSvdBoostedKnnModel, ISvdBoostedKnnUser, ISvdTrainer<TSvdBoostedKnnModel>, ISvdBoostedKnnRecommender<TSvdBoostedKnnModel>>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        IKnnTrainerForSvdModels<TSvdBoostedKnnModel> KnnTrainerForSvdModels { get; set; }
    }
}