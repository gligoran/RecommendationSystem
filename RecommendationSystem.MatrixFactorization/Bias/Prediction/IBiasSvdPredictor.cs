using RecommendationSystem.MatrixFactorization.Bias.Models;
using RecommendationSystem.MatrixFactorization.Prediction;

namespace RecommendationSystem.MatrixFactorization.Bias.Prediction
{
    public interface IBiasSvdPredictor : ISvdPredictor<IBiasSvdModel>
    {}
}