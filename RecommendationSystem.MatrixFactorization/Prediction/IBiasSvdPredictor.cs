using RecommendationSystem.MatrixFactorization.Models;

namespace RecommendationSystem.MatrixFactorization.Prediction
{
    public interface IBiasSvdPredictor : ISvdPredictor<IBiasSvdModel>
    {}
}