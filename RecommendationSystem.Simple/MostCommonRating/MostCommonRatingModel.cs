namespace RecommendationSystem.Simple.MostCommonRating
{
    public class MostCommonRatingModel : IMostCommonRatingModel
    {
        public float MostCommonRating { get; set; }

        public MostCommonRatingModel(float mostCommonRating)
        {
            MostCommonRating = mostCommonRating;
        }
    }
}