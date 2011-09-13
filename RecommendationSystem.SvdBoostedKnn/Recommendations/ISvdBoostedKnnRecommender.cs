using RecommendationSystem.Knn.Foundation.Recommendations;
using RecommendationSystem.Svd.Foundation.Recommendations;
using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Recommendations
{
    public interface ISvdBoostedKnnRecommender<TSvdBoostedKnnModel> : IKnnRecommender<TSvdBoostedKnnModel, ISvdBoostedKnnUser>, ISvdRecommender<TSvdBoostedKnnModel>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {}
}