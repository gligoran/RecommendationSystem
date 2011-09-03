using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Recommendations;
using RecommendationSystem.SimpleKnn.Training;
using RecommendationSystem.Training;

namespace RecommendationSystem.SimpleKnn
{
    public class SimpleKnnRecommendationSystem : ISimpleKnnRecommendationSystem
    {
        public ITrainer<ISimpleKnnModel, IUser> Trainer { get; set; }
        public IRecommender<ISimpleKnnModel> Recommender { get; set; }

        public SimpleKnnRecommendationSystem()
        {
            Trainer = new SimpleKnnTrainer();
            Recommender = new SimpleKnnRecommender();
        }

        public SimpleKnnRecommendationSystem(ITrainer<ISimpleKnnModel, IUser> trainer)
            : this(trainer, new SimpleKnnRecommender())
        {}

        public SimpleKnnRecommendationSystem(IRecommender<ISimpleKnnModel> recommender)
            : this(new SimpleKnnTrainer(), recommender)
        {}

        public SimpleKnnRecommendationSystem(ITrainer<ISimpleKnnModel, IUser> trainer, IRecommender<ISimpleKnnModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}