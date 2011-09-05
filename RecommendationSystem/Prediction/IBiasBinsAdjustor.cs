using RecommendationSystem.Models;

namespace RecommendationSystem.Prediction
{
    public interface IBiasBinsAdjustor<in TBiasBinsModel>
        where TBiasBinsModel : IBiasBinsModel
    {
        float AdjustRating(float rating, TBiasBinsModel model);
        int GetBiasBinIndex(float predictedRating, int biasBinCount);
    }
}