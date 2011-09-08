using RecommendationSystem.Knn.Foundation.Recommendations;
using RecommendationSystem.Svd.Foundation.Recommendations;
using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Recommendations
{
    public interface ISvdBoostedKnnRecommender<TSvdBoostedKnnModel, TSvdBoostedKnnUser> : IKnnRecommender<TSvdBoostedKnnModel, TSvdBoostedKnnUser>, ISvdRecommender<TSvdBoostedKnnModel>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
        where TSvdBoostedKnnUser : ISvdBoostedKnnUser
    {}
}