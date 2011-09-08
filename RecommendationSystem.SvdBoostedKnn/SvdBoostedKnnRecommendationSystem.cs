using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Recommendations;
using RecommendationSystem.SvdBoostedKnn.Training;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn
{
    public class SvdBoostedKnnRecommendationSystem<TSvdBoostedKnnModel, TSvdBoostedKnnUser> : ISvdBoostedKnnRecommendationSystem<TSvdBoostedKnnModel, TSvdBoostedKnnUser>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
        where TSvdBoostedKnnUser : ISvdBoostedKnnUser
    {
        public ISvdBoostedKnnTrainer<TSvdBoostedKnnModel> Trainer { get; set; }
        public ISvdBoostedKnnRecommender<TSvdBoostedKnnModel, TSvdBoostedKnnUser> Recommender { get; set; }

        public SvdBoostedKnnRecommendationSystem(ISvdBoostedKnnTrainer<TSvdBoostedKnnModel> trainer, ISvdBoostedKnnRecommender<TSvdBoostedKnnModel, TSvdBoostedKnnUser> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}