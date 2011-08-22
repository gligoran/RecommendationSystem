using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Training;

namespace RecommendationSystem
{
    public interface IRecommendationSystem<TModel, TUser, TTrainer, TRecommender>
        where TModel : IModel
        where TUser : IUser
        where TTrainer : ITrainer<TModel, TUser>
        where TRecommender : IRecommender<TModel>
    {
        TTrainer Trainer { get; set; }
        TRecommender Recommender { get; set; }
    }
}