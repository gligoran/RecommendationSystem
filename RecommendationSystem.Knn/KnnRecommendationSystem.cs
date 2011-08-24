using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.Recommendations;
using RecommendationSystem.Knn.Training;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Knn
{
    public class KnnRecommendationSystem : IKnnRecommendationSystem
    {
        public ITrainer<IKnnModel, IUser> Trainer { get; set; }
        public IRecommender<IKnnModel> Recommender { get; set; }

        public KnnRecommendationSystem()
        {
            Trainer = new KnnTrainer();
            Recommender = new KnnRecommender();
        }

        public KnnRecommendationSystem(ITrainer<IKnnModel, IUser> trainer)
            : this(trainer, new KnnRecommender())
        {}

        public KnnRecommendationSystem(IRecommender<IKnnModel> recommender)
            : this(new KnnTrainer(), recommender)
        {}

        public KnnRecommendationSystem(ITrainer<IKnnModel, IUser> trainer, IRecommender<IKnnModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}