using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Prediction
{
    public class SimpleSvdPredictor : SvdPredictorBase<ISvdModel>
    {
        public SimpleSvdPredictor()
            : this(new NewUserFeatureGenerator())
        {}

        public SimpleSvdPredictor(INewUserFeatureGenerator<ISvdModel> newUserFeatureGenerator)
            : base(newUserFeatureGenerator)
        {}

        public override float PredictRatingForArtist(IUser user, ISvdModel model, List<IArtist> artists, int artistIndex)
        {
            var newUserFeatures = NewUserFeatureGenerator.GetNewUserFeatures(model, user);

            var userRating = 0.0f;
            for (var f = 0; f < model.FeatureCount; f++)
                userRating += newUserFeatures[f] * model.ArtistFeatures[f, artistIndex];

            userRating = CapUserRatings(userRating);

            return userRating;
        }
    }
}