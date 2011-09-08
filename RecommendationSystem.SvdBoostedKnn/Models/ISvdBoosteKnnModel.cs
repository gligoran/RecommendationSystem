using RecommendationSystem.Knn.Foundation.Models;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Models
{
    public interface ISvdBoostedKnnModel : ISvdModel, IKnnModel<ISvdBoostedKnnUser>
    {}
}