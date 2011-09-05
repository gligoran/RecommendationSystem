using RecommendationSystem.Models;

namespace RecommendationSystem.Prediction
{
    public class BiasBinsAdjustor<TBiasBinsModel> : IBiasBinsAdjustor<TBiasBinsModel>
        where TBiasBinsModel : IBiasBinsModel
    {
        public float AdjustRating(float rating, TBiasBinsModel model)
        {
            rating -= model.BiasBins[GetBiasBinIndex(rating, model.BiasBins.Length)];
            return CapUserRatings(rating);
        }

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