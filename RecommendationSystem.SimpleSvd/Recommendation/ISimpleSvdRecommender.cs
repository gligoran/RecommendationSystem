using RecommendationSystem.Recommendations;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Recommendation
{
    public interface ISimpleSvdRecommender<TSvdModel> : IRecommender<TSvdModel>
        where TSvdModel : ISvdModel
    {
        ISvdPredictor<TSvdModel> Predictor { get; set; }
        bool UseBiasBins { get; set; }
    }
}