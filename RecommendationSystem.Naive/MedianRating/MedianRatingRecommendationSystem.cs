using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.MedianRating
{
    public class MedianRatingRecommendationSystem : IMedianRatingRecommendationSystem
    {
        public ITrainer<IMedianRatingModel> Trainer { get; set; }
        public IRecommender<IMedianRatingModel> Recommender { get; set; }

        public MedianRatingRecommendationSystem()
        {
            Trainer = new MedianRatingTrainer();
            Recommender = new MedianRatingRecommender();
        }

        public MedianRatingRecommendationSystem(ITrainer<IMedianRatingModel> trainer, IRecommender<IMedianRatingModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}