using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Recommendations
{
    public class WeightedSumRatingAggregator : IRatingAggregator
    {
        public float Aggregate(User user, string artist)
        {
            var k = 0.0f;
            var r = 0.0f;
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
