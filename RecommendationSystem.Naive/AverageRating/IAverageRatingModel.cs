using RecommendationSystem.Models;

namespace RecommendationSystem.Naive.AverageRating
{
    public interface IAverageRatingModel : IModel
    {
        float AverageRating { get; set; }
    }
}