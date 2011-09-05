using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.Svd.Foundation.Bias.Prediction;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Bias.Prediction
{
    public class BiasSimpleSvdPredictor : SvdPredictorBase<IBiasSvdModel>
    {
        public BiasSimpleSvdPredictor()
            : this(new BiasNewUserFeatureGenerator())
        {}

        public BiasSimpleSvdPredictor(INewUserFeatureGenerator<IBiasSvdModel> newUserFeatureGenerator)
            : base(newUserFeatureGenerator)
        {}

        public override float PredictRatingForArtist(IUser user, IBiasSvdModel model, List<IArtist> artists, int artist)
        {
            var userBias = user.Ratings.Average(rating => rating.Value - model.GlobalAverage);
            var newUserFeatures = NewUserFeatureGenerator.GetNewUserFeatures(model, user);

            var userRating = 0.0f;
            for (var f = 0; f < model.FeatureCount; f++)
                userRating += newUserFeatures[f] * model.ArtistFeatures[f, artist];

            userRating = CapUserRatings(model.GlobalAverage + userBias + model.ArtistBias[artist]);

            return userRating;
        }
    }
}