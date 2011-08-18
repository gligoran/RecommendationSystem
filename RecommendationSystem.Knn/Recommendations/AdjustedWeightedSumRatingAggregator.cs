namespace RecommendationSystem.Knn.Recommendations
{
    public class AdjustedWeightedSumRatingAggregator : IRatingAggregator
    {
        public float Aggregate(User.User user, string artist)
        {
            float k = 0.0f;
            float r = 0.0f;
            foreach (var u in user.Neighbours)
            {
                k += u.Estimate;
                if (u.SimilarUser.Ratings.ContainsKey(artist))
                    r += u.Estimate * (u.SimilarUser.Ratings[artist] - u.SimilarUser.AverageRating);
            }

            return (user.AverageRating + r / k) * user.TotalPlays;
        }
    }
}
