using RecommendationSystem.MatrixFactorization.Basic.Models;
using RecommendationSystem.MatrixFactorization.Basic.Recommendations;
using RecommendationSystem.MatrixFactorization.Basic.Training;
using RecommendationSystem.MatrixFactorization.Recommendation;
using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization.Basic
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