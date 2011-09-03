using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Similarity;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Recommendations.RecommendationGeneration
{
    public class FifthsRecommendationGenerator : IRecommendationGenerator
    {
        public float PredictRatingForArtist(ISimpleKnnUser simpleKnnUser, List<SimilarUser> neighbours, ISimpleKnnModel model, List<IArtist> artists, int artistIndex)
        {
            var recommendations = GenerateRecommendations(simpleKnnUser, neighbours, model, artists);
            var rating = recommendations.Where(r => r.Artist == artists[artistIndex]).Select(r => r.Value).FirstOrDefault();
            if (rating < 1.0f)
                return 1.0f;

            return rating;
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(ISimpleKnnUser simpleKnnUser, List<SimilarUser> neighbours, ISimpleKnnModel model, List<IArtist> artists)
        {
            var artistIndices = new List<int>();
            artistIndices = neighbours.Aggregate((IEnumerable<int>)artistIndices, (current, neighbour) => current.Union(neighbour.User.Ratings.Select(rating => rating.ArtistIndex))).Except(simpleKnnUser.Ratings.Select(rating => rating.ArtistIndex)).ToList();

            var recommendations = new List<Recommendation>();
            for (var i = 0; i < artistIndices.Count; i++)
            {
                var artistIndex = artistIndices[i];
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
            for (var i = 0; i < recommendations.Count; i++)
                recommendations[i].Value = 5 - i * 5 / recommendations.Count;

            return recommendations;
        }

        public override string ToString()
        {
            return "FRG";
        }
    }
}