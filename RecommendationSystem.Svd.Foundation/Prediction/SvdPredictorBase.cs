using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Prediction
{
    public abstract class SvdPredictorBase<TSvdModel> : ISvdPredictor<TSvdModel>
        where TSvdModel : ISvdModel
    {
        public INewUserFeatureGenerator<TSvdModel> NewUserFeatureGenerator { get; set; }

        public abstract float PredictRatingForArtist(IUser user, TSvdModel model, List<IArtist> artists, int artistIndex, bool useBiasBins);

        public int GetBiasBinIndex(float predictedRating, int biasBinCount)
        {
            for (var i = 0; i < biasBinCount; i++)
            {
                if (predictedRating - 1.0f >= i * 4.0f / biasBinCount && predictedRating - 1.0f < (i + 1) * 4.0f / biasBinCount)
                    return i;
            }

            //predictedRating == 5.0f
            return biasBinCount - 1;
        }

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