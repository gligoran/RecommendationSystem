using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Simple.MedianRating
{
    public class MedianRatingRecommendationSystem : IMedianRatingRecommendationSystem
    {
        public ITrainer<IMedianRatingModel, IUser> Trainer { get; set; }
        public IRecommender<IMedianRatingModel> Recommender { get; set; }

        public MedianRatingRecommendationSystem()
        {
            Trainer = new MedianRatingTrainer();
            Recommender = new MedianRatingRecommender();
        }

        public MedianRatingRecommendationSystem(ITrainer<IMedianRatingModel, IUser> trainer, IRecommender<IMedianRatingModel> recommender)
        {
            Trainer = trainer;
            Recommender = recommender;
        }
    }
}