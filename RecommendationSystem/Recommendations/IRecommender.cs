using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Prediction;

namespace RecommendationSystem.Recommendations
{
    public interface IRecommender<TModel>
        where TModel : IModel
    {
        IPredictor<TModel> Predictor { get; set; }
        float PredictRatingForArtist(IUser user, TModel model, List<IArtist> artists, int artistIndex);
        IEnumerable<IRecommendation> GenerateRecommendations(IUser user, TModel model, List<IArtist> artists);
    }
}