using RecommendationSystem.Svd.Foundation.Training;
using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Recommendations;
using RecommendationSystem.SvdBoostedKnn.Training;

namespace RecommendationSystem.SvdBoostedKnn
{
    public class SvdBoostedKnnRecommendationSystem<TSvdBoostedKnnModel> : ISvdBoostedKnnRecommendationSystem<TSvdBoostedKnnModel>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        public ISvdTrainer<TSvdBoostedKnnModel> Trainer { get; set; }
        public ISvdBoostedKnnRecommender<TSvdBoostedKnnModel> Recommender { get; set; }
        public IKnnTrainerForSvdModels<TSvdBoostedKnnModel> KnnTrainerForSvdModels { get; set; }

        public SvdBoostedKnnRecommendationSystem()
        {}

        public SvdBoostedKnnRecommendationSystem(ISvdTrainer<TSvdBoostedKnnModel> trainer, IKnnTrainerForSvdModels<TSvdBoostedKnnModel> knnTrainerForSvdModels, ISvdBoostedKnnRecommender<TSvdBoostedKnnModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
            KnnTrainerForSvdModels = knnTrainerForSvdModels;
        }
    }
}