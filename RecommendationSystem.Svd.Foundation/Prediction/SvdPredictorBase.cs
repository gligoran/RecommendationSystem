using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Prediction
{
    public abstract class SvdPredictorBase<TSvdModel> : ISvdPredictor<TSvdModel>
        where TSvdModel : ISvdModel
    {
        public INewUserFeatureGenerator<TSvdModel> NewUserFeatureGenerator { get; set; }

        protected SvdPredictorBase(INewUserFeatureGenerator<TSvdModel> newUserFeatureGenerator)
        {
            NewUserFeatureGenerator = newUserFeatureGenerator;
        }

        public abstract float PredictRatingForArtist(IUser user, TSvdModel model, List<IArtist> artists, int artistIndex);

        protected static float CapUserRatings(float userRating)
        {
            if (userRating < 1.0f)
                return 1.0f;

            if (userRating > 5.0f)
                return 5.0f;

            return userRating;
        }
    }
}