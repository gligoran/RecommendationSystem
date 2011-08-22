using RecommendationSystem.Models;

namespace RecommendationSystem.Simple.MedianRating
{
    public interface IMedianRatingModel : IModel
    {
        float MedianRating { get; set; }
    }
}