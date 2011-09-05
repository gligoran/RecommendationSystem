using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;

namespace RecommendationSystem.Prediction
{
    public interface IPredictor<in TModel>
        where TModel : IModel
    {
        float PredictRatingForArtist(IUser user, TModel model, List<IArtist> artists, int artistIndex);
    }
}