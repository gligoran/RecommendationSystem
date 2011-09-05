using RecommendationSystem.Prediction;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Prediction
{
    public interface ISvdPredictor<TSvdModel> : IPredictor<TSvdModel>
        where TSvdModel : ISvdModel
    {
        INewUserFeatureGenerator<TSvdModel> NewUserFeatureGenerator { get; set; }
    }
}