using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Recommendations;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd
{
    public class SimpleSvdRecommendationSystem<TSvdModel> : ISimpleSvdRecommendationSystem<TSvdModel>
        where TSvdModel : ISvdModel
    {
        public ISvdTrainer<TSvdModel> Trainer { get; set; }
        public ISvdRecommender<TSvdModel> Recommender { get; set; }

        public SimpleSvdRecommendationSystem(ISvdTrainer<TSvdModel> trainer, ISvdRecommender<TSvdModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}