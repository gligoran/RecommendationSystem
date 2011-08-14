namespace RecommendationSystem.Knn.Recommendations
{
    public class WeightedSumRatingAggregator : IRatingAggregator
    {
        public double Aggregate(User user, string artist)
        {
            double k = 0.0;
            double r = 0.0;
            foreach (var u in user.Neighbours)
            {
                k += u.Estimate;
                if (u.SimilarUser.Ratings.ContainsKey(artist))
                    r += u.Estimate * u.SimilarUser.Ratings[artist];
                else
                    r += u.Estimate * u.SimilarUser.AverageRating;
            }

            return r / k * user.TotalPlays;
        }
    }
}
