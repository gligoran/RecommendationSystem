using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.SvdBoostedKnn.Models;

namespace RecommendationSystem.SvdBoostedKnn.Bias.Models
{
    public interface IBiasSvdBoostedKnnModel : IBiasSvdModel, ISvdBoostedKnnModel
    {}
}