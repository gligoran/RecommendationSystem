using RecommendationSystem.Entities;
using RecommendationSystem.Models;

namespace RecommendationSystem.Prediction
{
    public interface IPredictor<in TModel, in TUser>
        where TModel : IModel
        where TUser : IUser
    {
        float PredictRating(TModel model, TUser user);
    }
}