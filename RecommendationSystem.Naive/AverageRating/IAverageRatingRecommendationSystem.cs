using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.AverageRating
{
    public interface IAverageRatingRecommendationSystem : IRecommendationSystem<IAverageRatingModel, IUser, ITrainer<IAverageRatingModel, IUser>, IRecommender<IAverageRatingModel>>
    {}
}