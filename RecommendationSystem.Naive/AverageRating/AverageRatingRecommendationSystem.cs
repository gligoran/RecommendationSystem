using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.AverageRating
{
    public class AverageRatingRecommendationSystem : IAverageRatingRecommendationSystem
    {
        public ITrainer<IAverageRatingModel, IUser> Trainer { get; set; }
        public IRecommender<IAverageRatingModel> Recommender { get; set; }

        public AverageRatingRecommendationSystem()
        {
            Trainer = new AverageRatingTrainer();
            Recommender = new AverageRatingRecommender();
        }

        public AverageRatingRecommendationSystem(ITrainer<IAverageRatingModel, IUser> trainer, IRecommender<IAverageRatingModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}