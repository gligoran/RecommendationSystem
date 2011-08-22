using RecommendationSystem.Models;

namespace RecommendationSystem.Simple.MostCommonRating
{
    public interface IMostCommonRatingModel : IModel
    {
        float MostCommonRating { get; set; }
    }
}