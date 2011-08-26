using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Basic.Models;
using RecommendationSystem.MatrixFactorization.Basic.Recommendations;
using RecommendationSystem.MatrixFactorization.Basic.Training;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.MatrixFactorization.Basic
{
    public class BasicSvdRecommendationSystem : ISvdRecommendationSystem<IBasicSvdModel>
    {
        public ITrainer<IBasicSvdModel, IUser> Trainer { get; set; }
        public IRecommender<IBasicSvdModel> Recommender { get; set; }

        public BasicSvdRecommendationSystem()
        {
            Trainer = new BasicSvdTrainer();
            Recommender = new BasicSvdRecommender();
        }

        public BasicSvdRecommendationSystem(ITrainer<IBasicSvdModel, IUser> trainer)
            : this(trainer, new BasicSvdRecommender())
        {}

        public BasicSvdRecommendationSystem(IRecommender<IBasicSvdModel> recommender)
            : this(new BasicSvdTrainer(), recommender)
        {}

        public BasicSvdRecommendationSystem(ITrainer<IBasicSvdModel, IUser> trainer, IRecommender<IBasicSvdModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}