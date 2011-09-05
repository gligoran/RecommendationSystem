using RecommendationSystem.Models;
using RecommendationSystem.Prediction;

namespace RecommendationSystem.Recommendations
{
    public interface IBiasBinsRecommender<TModel> : IRecommender<TModel>
        where TModel : IBiasBinsModel
    {
        IBiasBinsAdjustor<TModel> BiasBinsAdjustor { get; set; }
    }
}