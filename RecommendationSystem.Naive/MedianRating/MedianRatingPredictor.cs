using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Prediction;

namespace RecommendationSystem.Naive.MedianRating
{
    public class MedianRatingPredictor : IPredictor<IMedianRatingModel>
    {
        public float PredictRatingForArtist(IUser user, IMedianRatingModel model, List<IArtist> artists, int artistIndex)
        {
            return model.MedianRating;
        }
    }
}