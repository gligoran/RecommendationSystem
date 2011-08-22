using RecommendationSystem.Models;

namespace RecommendationSystem.Simple.AverageRating
{
    public interface IAverageRatingModel : IModel
    {
        float AverageRating { get; set; }
    }
}