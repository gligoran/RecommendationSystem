using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.MostCommonRating
{
    public class MostCommonRatingRecommendationSystem : IMostCommonRatingRecommendationSystem
    {
        public ITrainer<IMostCommonRatingModel> Trainer { get; set; }
        public IRecommender<IMostCommonRatingModel> Recommender { get; set; }

        public MostCommonRatingRecommendationSystem()
        {
            Trainer = new MostCommonRatingTrainer();
            Recommender = new MostCommonRatingRecommender();
        }

        public MostCommonRatingRecommendationSystem(ITrainer<IMostCommonRatingModel> trainer, IRecommender<IMostCommonRatingModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}