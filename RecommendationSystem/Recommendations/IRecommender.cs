using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;

namespace RecommendationSystem.Recommendations
{
    public interface IRecommender<in TModel>
        where TModel : IModel
    {
        List<IRecommendation> GenerateRecommendations(IUser user, TModel model, List<IArtist> artists);
        float PredictRatingForArtist(IUser user, TModel model, List<IArtist> artists, int artist);
    }
}