using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Bias.Prediction
{
    public class BiasSvdPredictor : SvdPredictorBase<IBiasSvdModel>
    {
        public override float PredictRatingForArtist(IUser user, IBiasSvdModel model, List<IArtist> artists, int artist, bool useBiasBins)
        {
            var userBias = user.Ratings.Average(rating => rating.Value - model.GlobalAverage);
            var newUserFeatures = NewUserFeatureGenerator.GetNewUserFeatures(model, user);

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
    }
}