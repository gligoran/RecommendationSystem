using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Prediction;

namespace RecommendationSystem.Naive.AverageRating
{
    public class AverageRatingPredictor : IPredictor<IAverageRatingModel>
    {
        public float PredictRatingForArtist(IUser user, IAverageRatingModel model, List<IArtist> artists, int artistIndex)
        {
            return model.AverageRating;
        }
    }
}