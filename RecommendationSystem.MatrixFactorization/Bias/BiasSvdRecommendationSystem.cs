using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Bias.Models;
using RecommendationSystem.MatrixFactorization.Bias.Recommendations;
using RecommendationSystem.MatrixFactorization.Bias.Training;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.MatrixFactorization.Bias
{
    class BiasSvdRecommendationSystem : ISvdRecommendationSystem<IBiasSvdModel>
    {
        public ITrainer<IBiasSvdModel, IUser> Trainer { get; set; }
        public IRecommender<IBiasSvdModel> Recommender { get; set; }

        public BiasSvdRecommendationSystem()
        {
            Trainer = new BiasSvdTrainer();
            Recommender = new BiasSvdRecommender();
        }

        public BiasSvdRecommendationSystem(ITrainer<IBiasSvdModel, IUser> trainer)
            : this(trainer, new BiasSvdRecommender())
        { }

        public BiasSvdRecommendationSystem(IRecommender<IBiasSvdModel> recommender)
            : this(new BiasSvdTrainer(), recommender)
        { }

        public BiasSvdRecommendationSystem(ITrainer<IBiasSvdModel, IUser> trainer, IRecommender<IBiasSvdModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}