using RecommendationSystem.MatrixFactorization.Models;

namespace RecommendationSystem.MatrixFactorization.Prediction
{
    public interface IBasicSvdPredictor : ISvdPredictor<ISvdModel>
    {}
}