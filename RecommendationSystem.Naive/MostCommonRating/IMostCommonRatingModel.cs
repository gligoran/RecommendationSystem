using RecommendationSystem.Models;

namespace RecommendationSystem.Naive.MostCommonRating
{
    public interface IMostCommonRatingModel : IModel
    {
        float MostCommonRating { get; set; }
    }
}