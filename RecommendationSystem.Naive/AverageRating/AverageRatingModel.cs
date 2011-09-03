namespace RecommendationSystem.Naive.AverageRating
{
    public class AverageRatingModel : IAverageRatingModel
    {
        public float AverageRating { get; set; }

        public AverageRatingModel(float average)
        {
            AverageRating = average;
        }
    }
}