using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.RatingAggregation
{
    public class WeightedSumRatingAggregator : IRatingAggregator
    {
        public float Aggregate(IKnnUser user, List<SimilarUser> neighbours, int artistIndex)
        {
            if (neighbours == null || neighbours.Count == 0)
                return 0.0f;

            var k = 0.0f;
            var r = 0.0f;

            foreach (var neighbour in neighbours)
            {
                k += neighbour.Similarity;

                var rating = neighbour.User.Ratings.FirstOrDefault(nr => nr.ArtistIndex == artistIndex);
                if (rating == null)
                    r += neighbour.Similarity * neighbour.User.AverageRating;
                else
                    r += neighbour.Similarity * rating.Value;
            }

            return r / k;
        }

        public override string ToString()
        {
            return "WSRA";
        }
    }
}