using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.MedianRating
{
    public interface IMedianRatingRecommendationSystem : IRecommendationSystem<IMedianRatingModel, IUser, ITrainer<IMedianRatingModel, IUser>, IRecommender<IMedianRatingModel>>
    {}
}