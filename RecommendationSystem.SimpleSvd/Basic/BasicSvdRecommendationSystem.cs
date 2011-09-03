using RecommendationSystem.SimpleSvd.Basic.Recommendations;
using RecommendationSystem.SimpleSvd.Basic.Training;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd.Basic
{
    public class BasicSvdRecommendationSystem : SvdRecommendationSystemBase<IBasicSvdModel>
    {
        public BasicSvdRecommendationSystem()
        {
            Trainer = new BasicSvdTrainer();
            Recommender = new BasicSvdRecommender();
        }

        public BasicSvdRecommendationSystem(ISvdTrainer<IBasicSvdModel> trainer)
            : this(trainer, new BasicSvdRecommender())
        {}

        public BasicSvdRecommendationSystem(ISvdRecommender<IBasicSvdModel> recommender)
            : this(new BasicSvdTrainer(), recommender)
        {}

        public BasicSvdRecommendationSystem(ISvdTrainer<IBasicSvdModel> trainer, ISvdRecommender<IBasicSvdModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }

        protected override IBasicSvdModel GetNewModel()
        {
            return new BasicSvdModel();
        }
    }
}