using RecommendationSystem.Recommendations;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Prediction;

namespace RecommendationSystem.SimpleSvd.Recommendation
{
    public interface ISimpleSvdRecommender<TSvdModel> : IRecommender<TSvdModel>
        where TSvdModel : ISvdModel
    {
        INewUserFeatureGenerator<TSvdModel> NewUserFeatureGenerator { get; set; }
        void LoadModel(TSvdModel model, string filename);
    }
}