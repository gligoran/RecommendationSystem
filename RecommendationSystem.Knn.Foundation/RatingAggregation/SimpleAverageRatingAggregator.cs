using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Knn.Foundation.Users;

namespace RecommendationSystem.Knn.Foundation.RatingAggregation
{
    public class SimpleAverageRatingAggregator<TKnnUser> : IRatingAggregator<TKnnUser>
        where TKnnUser : IKnnUser
    {
        public float Aggregate(TKnnUser user, List<SimilarUser<TKnnUser>> neighbours, int artistIndex)
        {
            if (neighbours == null || neighbours.Count == 0)
                return 0.0f;

            var count = 0;
            var r = 0.0f;
            foreach (var neighbour in neighbours)
            {
                var rating = neighbour.User.Ratings.FirstOrDefault(nr => nr.ArtistIndex == artistIndex);
                //if (rating == null)
                //    r += 1.0f;
                //else
                //    r += rating.Value;

                if (rating != null)
                {
                    count++;
                    r += rating.Value;
                }
            }

            //return r / neighbours.Count;
            if (count == 0)
                return 1.0f;

            return r / count;
        }

        public override string ToString()
        {
            return "SARA";
        }
    }
}