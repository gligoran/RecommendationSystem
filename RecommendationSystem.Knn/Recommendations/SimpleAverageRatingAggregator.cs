using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Recommendations
{
    public class SimpleAverageRatingAggregator : IRatingAggregator
    {
        public float Aggregate(User user, string artist)
        {
            var r = 0.0f;
            foreach (var u in user.Neighbours)
            {
                if (u.SimilarUser.Ratings.ContainsKey(artist))
                    r += u.SimilarUser.Ratings[artist];
                else
                    r += u.SimilarUser.AverageRating;
            }

            return r / user.Neighbours.Count * user.TotalPlays;
        }
    }
}