using RecommendationSystem.Models;

namespace RecommendationSystem.Naive.MedianRating
{
    public interface IMedianRatingModel : IModel
    {
        float MedianRating { get; set; }
    }
}