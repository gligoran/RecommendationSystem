using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem.Naive.MostCommonRating
{
    public interface IMostCommonRatingRecommendationSystem : IRecommendationSystem<IMostCommonRatingModel, IUser, ITrainer<IMostCommonRatingModel>, IRecommender<IMostCommonRatingModel>>
    {}
}