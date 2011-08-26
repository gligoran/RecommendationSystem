using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.MatrixFactorization.Prediction;

namespace RecommendationSystem.MatrixFactorization.Basic.Prediction
{
    public interface IBasicSvdPredictor : ISvdPredictor<ISvdModel>
    {}
}