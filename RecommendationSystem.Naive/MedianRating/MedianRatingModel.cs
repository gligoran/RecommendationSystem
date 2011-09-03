namespace RecommendationSystem.Naive.MedianRating
{
    public class MedianRatingModel : IMedianRatingModel
    {
        public float MedianRating { get; set; }

        public MedianRatingModel(float medianRating)
        {
            MedianRating = medianRating;
        }
    }
}