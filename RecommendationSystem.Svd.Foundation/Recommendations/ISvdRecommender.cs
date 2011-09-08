using RecommendationSystem.Recommendations;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.Svd.Foundation.Recommendations
{
    public interface ISvdRecommender<TSvdModel> : IRecommender<TSvdModel>
        where TSvdModel : ISvdModel
    {
        INewUserFeatureGenerator<TSvdModel> NewUserFeatureGenerator { get; set; }
        void LoadModel(TSvdModel model, string filename);
    }
}