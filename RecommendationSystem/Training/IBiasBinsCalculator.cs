using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Models;
using RecommendationSystem.Prediction;

namespace RecommendationSystem.Training
{
    public interface IBiasBinsCalculator<TBiasBinsModel>
        where TBiasBinsModel : IBiasBinsModel
    {
        IPredictor<TBiasBinsModel> Predictor { get; set; }
        void CalculateBiasBins(TBiasBinsModel model, List<IRating> ratings, List<IUser> users, List<IArtist> artists, int biasBinCount);
        int GetBiasBinIndex(float predictedRating, int biasBinCount);
    }
}