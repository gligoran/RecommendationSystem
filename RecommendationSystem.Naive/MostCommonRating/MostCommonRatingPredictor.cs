using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Prediction;

namespace RecommendationSystem.Naive.MostCommonRating
{
    public class MostCommonRatingPredictor : IPredictor<IMostCommonRatingModel>
    {
        public float PredictRatingForArtist(IUser user, IMostCommonRatingModel model, List<IArtist> artists, int artistIndex)
        {
            return model.MostCommonRating;
        }
    }
}