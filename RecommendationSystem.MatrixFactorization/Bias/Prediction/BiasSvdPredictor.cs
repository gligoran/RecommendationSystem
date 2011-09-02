using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Bias.Models;
using RecommendationSystem.MatrixFactorization.Prediction;

namespace RecommendationSystem.MatrixFactorization.Bias.Prediction
{
    public class BiasSvdPredictor : SvdPredictorBase<IBiasSvdModel>
    {
        public override float PredictRatingForArtist(IUser user, IBiasSvdModel model, List<IArtist> artists, int artist, bool useBiasBins)
        {
            float userBias;
            var newUserFeatures = GetUserFeatures(model, user, out userBias);

            var userRating = 0.0f;
            for (var f = 0; f < model.FeatureCount; f++)
                userRating += newUserFeatures[f] * model.ArtistFeatures[f, artist];

            userRating = CapUserRatings(model.GlobalAverage + userBias + model.ArtistBias[artist]);

            if (useBiasBins)
            {
                userRating -= model.BiasBins[GetBiasBinIndex(userRating, model.BiasBins.Count())];
                userRating = CapUserRatings(userRating);
            }

            return userRating;
        }

        private float[] GetUserFeatures(IBiasSvdModel model, IUser user, out float userBias)
        {
            userBias = user.Ratings.Average(rating => rating.Value - model.GlobalAverage);

            var ratingSum = user.Ratings.Sum(r => r.Value);
            var newUserFeatures = new float[model.FeatureCount];
            for (var f = 0; f < model.FeatureCount; f++)
            {
                newUserFeatures[f] = 0.0f;
                foreach (var rating in user.Ratings)
                    newUserFeatures[f] += (rating.Value - model.GlobalAverage - userBias - model.ArtistBias[rating.ArtistIndex]) * model.ArtistFeatures[f, rating.ArtistIndex];

                newUserFeatures[f] /= ratingSum;
            }

            return newUserFeatures;
        }
    }
}