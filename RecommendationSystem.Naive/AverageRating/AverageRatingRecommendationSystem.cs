using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.AverageRating
{
    public class AverageRatingRecommendationSystem : IAverageRatingRecommendationSystem
    {
        public ITrainer<IAverageRatingModel> Trainer { get; set; }
        public IRecommender<IAverageRatingModel> Recommender { get; set; }

        public AverageRatingRecommendationSystem()
        {
            Trainer = new AverageRatingTrainer();
            Recommender = new AverageRatingRecommender();
        }

        public AverageRatingRecommendationSystem(ITrainer<IAverageRatingModel> trainer, IRecommender<IAverageRatingModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}