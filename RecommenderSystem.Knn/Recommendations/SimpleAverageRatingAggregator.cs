        namespace RecommenderSystem.Knn.Recommendations
{
    public class SimpleAverageRatingAggregator : IRatingAggregator
    {
        public double Aggregate(User user, string artist)
        {
            double r = 0.0;
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
