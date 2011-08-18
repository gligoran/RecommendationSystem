using System.Collections.Generic;
using RecommendationSystem.MatrixFactorization.Model;

namespace RecommendationSystem.MatrixFactorization.Prediction
{
    public interface ISvdPredictor<in TSvdModel>
        where TSvdModel : ISvdModel
    {
        List<string> Users { get; set; }
        List<string> Artists { get; set; }
        float PredictRating(TSvdModel model, int userIndex, int artistIndex);
    }
}