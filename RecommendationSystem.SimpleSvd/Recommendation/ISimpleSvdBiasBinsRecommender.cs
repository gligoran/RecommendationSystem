using RecommendationSystem.Recommendations;
using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.SimpleSvd.Recommendation
{
    public interface ISimpleSvdBiasBinsRecommender<TSvdBiasBinsModel> : ISimpleSvdRecommender<TSvdBiasBinsModel>, IBiasBinsRecommender<TSvdBiasBinsModel>
        where TSvdBiasBinsModel : ISvdBiasBinsModel
    {}
}