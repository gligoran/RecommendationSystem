using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd
{
    public class SimpleSvdRecommendationSystem<TSvdModel> : ISimpleSvdRecommendationSystem<TSvdModel>
        where TSvdModel : ISvdModel
    {
        public SimpleSvdRecommendationSystem(ISvdTrainer<TSvdModel> trainer, ISimpleSvdRecommender<TSvdModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }

        public ISvdTrainer<TSvdModel> Trainer { get; set; }
        public ISimpleSvdRecommender<TSvdModel> Recommender { get; set; }
    }
}