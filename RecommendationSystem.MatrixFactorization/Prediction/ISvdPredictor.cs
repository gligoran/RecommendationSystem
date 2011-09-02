using System.Collections.Generic;
using RecommendationSystem.Entities;

namespace RecommendationSystem.MatrixFactorization.Prediction
{
    public interface ISvdPredictor<in TSvdModel>
    {
        float PredictRatingForArtist(IUser user, TSvdModel model, List<IArtist> artists, int artistIndex, bool useBiasBins = false);
        int GetBiasBinIndex(float predictedRating, int biasBinCount);
    }
}