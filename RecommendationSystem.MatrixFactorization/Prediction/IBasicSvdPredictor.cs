using RecommendationSystem.MatrixFactorization.Model;

namespace RecommendationSystem.MatrixFactorization.Prediction
{
    public interface IBasicSvdPredictor : ISvdPredictor<ISvdModel>
    {}
}