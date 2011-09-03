using RecommendationSystem.SimpleSvd.Basic.Recommendations;
using RecommendationSystem.SimpleSvd.Basic.Training;
using RecommendationSystem.SimpleSvd.Recommendation;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Training;

namespace RecommendationSystem.SimpleSvd.Basic
{
    public class BasicSimpleSvdRecommendationSystem : SimpleSvdRecommendationSystemBase<IBasicSvdModel>
    {
        public BasicSimpleSvdRecommendationSystem()
        {
            Trainer = new BasicSimpleSimpleSvdTrainer();
            Recommender = new BasicSimpleSimpleSvdRecommender();
        }

        public BasicSimpleSvdRecommendationSystem(ISvdTrainer<IBasicSvdModel> trainer)
            : this(trainer, new BasicSimpleSimpleSvdRecommender())
        {}

        public BasicSimpleSvdRecommendationSystem(ISimpleSvdRecommender<IBasicSvdModel> recommender)
            : this(new BasicSimpleSimpleSvdTrainer(), recommender)
        {}

        public BasicSimpleSvdRecommendationSystem(ISvdTrainer<IBasicSvdModel> trainer, ISimpleSvdRecommender<IBasicSvdModel> recommender)
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