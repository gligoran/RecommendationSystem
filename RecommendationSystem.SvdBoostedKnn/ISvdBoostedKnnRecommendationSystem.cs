using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Recommendations;
using RecommendationSystem.SvdBoostedKnn.Training;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn
{
    public interface ISvdBoostedKnnRecommendationSystem<TSvdBoostedKnnModel, TSvdBoostedKnnUser> : IRecommendationSystem<TSvdBoostedKnnModel, TSvdBoostedKnnUser, ISvdBoostedKnnTrainer<TSvdBoostedKnnModel>, ISvdBoostedKnnRecommender<TSvdBoostedKnnModel, TSvdBoostedKnnUser>>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
        where TSvdBoostedKnnUser : ISvdBoostedKnnUser
    {}
}