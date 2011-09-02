using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.MatrixFactorization.Prediction;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.MatrixFactorization.Recommendation
{
    public interface ISvdRecommender<TSvdModel> : IRecommender<TSvdModel>
        where TSvdModel : ISvdModel
    {
        ISvdPredictor<TSvdModel> Predictor { get; set; }
        bool UseBiasBins { get; set; }
    }
}