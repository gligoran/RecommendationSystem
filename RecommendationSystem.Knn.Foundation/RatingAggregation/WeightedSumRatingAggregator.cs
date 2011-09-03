using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Knn.Foundation.Users;

namespace RecommendationSystem.Knn.Foundation.RatingAggregation
{
    public class WeightedSumRatingAggregator<TKnnUser> : IRatingAggregator<TKnnUser>
        where TKnnUser : IKnnUser
    {
        public float Aggregate(TKnnUser user, List<SimilarUser<TKnnUser>> neighbours, int artistIndex)
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
                    r += neighbour.Similarity * 1.0f;
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