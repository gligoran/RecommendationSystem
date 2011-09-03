using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Prediction
{
    public interface ISvdPredictor<TSvdModel>
        where TSvdModel : ISvdModel
    {
        INewUserFeatureGenerator<TSvdModel> NewUserFeatureGenerator { get; set; }

        float PredictRatingForArtist(IUser user, TSvdModel model, List<IArtist> artists, int artistIndex, bool useBiasBins = false);
        int GetBiasBinIndex(float predictedRating, int biasBinCount);
    }
}