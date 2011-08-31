using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Recommendations
{
    public class EqualDescentRecommendationGenerator : IRecommendationGenerator
    {
        public float PredictRatingForArtist(IKnnUser knnUser, List<SimilarUser> neighbours, IKnnModel model, List<IArtist> artists, int artistIndex)
        {
            var recommendations = GenerateRecommendations(knnUser, neighbours, model, artists);
            var rating = recommendations.Where(r => r.Artist == artists[artistIndex]).Select(r => r.Value).FirstOrDefault();
            if (rating < 1.0f)
                return 1.0f;

            return rating;
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IKnnUser knnUser, List<SimilarUser> neighbours, IKnnModel model, List<IArtist> artists)
        {
            var artistIndices = new List<int>();
            artistIndices = neighbours.Aggregate((IEnumerable<int>)artistIndices, (current, neighbour) => current.Union(neighbour.User.Ratings.Select(rating => rating.ArtistIndex))).Except(knnUser.Ratings.Select(rating => rating.ArtistIndex)).ToList();

            var recommendations = new List<Recommendation>();
            foreach (var artistIndex in artistIndices)
            {
                var rating = 0.0f;
                var count = 0;
                foreach (var neighbour in neighbours.Where(neighbour => neighbour.User.ArtistIndices.Contains(artistIndex)))
                {
                    rating += neighbour.User.RatingsByArtistIndexLookupTable[artistIndex].Value;
                    count++;
                }

                recommendations.Add(new Recommendation(artists[artistIndex], rating / count));
            }

            if (recommendations.Count < 1)
                return recommendations;

            recommendations.Sort();

            var interval = 4.0f / (recommendations.Count - 1);
            for (var i = 0; i < recommendations.Count; i++)
                recommendations[i].Value = 5.0f - i * interval;

            return recommendations;
        }

        public override string ToString()
        {
            return "EDRG";
        }
    }
}