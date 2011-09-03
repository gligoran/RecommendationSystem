using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Basic.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Basic.Prediction
{
    public class BasicSimpleSvdPredictor : SvdPredictorBase<IBasicSvdModel>
    {
        public override float PredictRatingForArtist(IUser user, IBasicSvdModel model, List<IArtist> artists, int artistIndex, bool useBiasBins)
        {
            var newUserFeatures = NewUserFeatureGenerator.GetNewUserFeatures(model, user);

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
    }
}