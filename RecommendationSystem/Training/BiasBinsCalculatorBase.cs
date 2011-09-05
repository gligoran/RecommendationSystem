using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Prediction;

namespace RecommendationSystem.Training
{
    public abstract class BiasBinsCalculatorBase<TBiasBinsModel> : IBiasBinsCalculator<TBiasBinsModel>
        where TBiasBinsModel : IBiasBinsModel
    {
        public IPredictor<TBiasBinsModel> Predictor { get; set; }

        protected BiasBinsCalculatorBase(IPredictor<TBiasBinsModel> predictor)
        {
            Predictor = predictor;
        }

        public abstract void CalculateBiasBins(TBiasBinsModel model, List<IRating> ratings, List<IUser> users, List<IArtist> artists, int biasBinCount);

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
    }
}