using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.Prediction;

namespace RecommendationSystem.MatrixFactorization.Prediction
{
    public interface ISvdPredictor<in TSvdModel> : IPredictor<TSvdModel, IUser>
        where TSvdModel : ISvdModel
    {
        List<string> Users { get; set; }
        List<string> Artists { get; set; }
        new float PredictRating(TSvdModel model, IUser user);
    }
}