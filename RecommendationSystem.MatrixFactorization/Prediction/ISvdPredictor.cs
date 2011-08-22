using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.MatrixFactorization.Models;

namespace RecommendationSystem.MatrixFactorization.Prediction
{
    public interface ISvdPredictor<in TSvdModel>
        where TSvdModel : ISvdModel
    {
        List<string> Users { get; set; }
        List<string> Artists { get; set; }
        float PredictRating(TSvdModel model, IUser user);
    }
}