using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Basic.Models;
using RecommendationSystem.MatrixFactorization.Prediction;

namespace RecommendationSystem.MatrixFactorization.Basic.Prediction
{
    public class BasicSvdPredictor : SvdPredictorBase<IBasicSvdModel>
    {
        public override float PredictRatingForArtist(IUser user, IBasicSvdModel model, List<IArtist> artists, int artistIndex, bool useBiasBins)
        {
            var newUserFeatures = GetUserFeatures(model, user);

            var userRating = 0.0f;
            for (var f = 0; f < model.FeatureCount; f++)
                userRating += newUserFeatures[f] * model.ArtistFeatures[f, artistIndex];

            userRating = CapUserRatings(userRating);

            if (useBiasBins)
            {
                userRating -= model.BiasBins[GetBiasBinIndex(userRating, model.BiasBins.Length)];
                userRating = CapUserRatings(userRating);
            }

            return userRating;
        }

        public float[] GetUserFeatures(IBasicSvdModel model, IUser user)
        {
            var ratingSum = user.Ratings.Sum(r => r.Value);

            var newUserFeatures = new float[model.FeatureCount];
            for (var f = 0; f < model.FeatureCount; f++)
            {
                newUserFeatures[f] = 0.0f;
                foreach (var rating in user.Ratings)
                    newUserFeatures[f] += rating.Value * model.ArtistFeatures[f, rating.ArtistIndex];

                newUserFeatures[f] /= ratingSum;
            }

            return newUserFeatures;
        }
    }
}