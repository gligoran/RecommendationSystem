using RecommendationSystem.Recommendations;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Recommendations
{
    public interface ISvdBiasBinsRecommender<TSvdBiasBinsModel> : ISvdRecommender<TSvdBiasBinsModel>, IBiasBinsRecommender<TSvdBiasBinsModel>
        where TSvdBiasBinsModel : ISvdBiasBinsModel
    {}
}