using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Knn.Foundation.Users;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration
{
    public class EqualDescentSimpleRecommendationGenerator<TModel, TKnnUSer> : IRecommendationGenerator<TModel, TKnnUSer>
        where TModel : IModel
        where TKnnUSer : IKnnUser
    {
        public float PredictRatingForArtist(TKnnUSer simpleKnnUser, List<SimilarUser<TKnnUSer>> neighbours, TModel model, List<IArtist> artists, int artistIndex)
        {
            var recommendations = GenerateRecommendations(simpleKnnUser, neighbours, model, artists);
            var rating = recommendations.Where(r => r.Artist == artists[artistIndex]).Select(r => r.Value).FirstOrDefault();
            if (rating < 1.0f)
                return 1.0f;

            return rating;
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(TKnnUSer simpleKnnUser, List<SimilarUser<TKnnUSer>> neighbours, TModel model, List<IArtist> artists)
        {
            var artistIndices = new List<int>();
            artistIndices = neighbours.Aggregate((IEnumerable<int>)artistIndices, (current, neighbour) => current.Union(neighbour.User.Ratings.Select(rating => rating.ArtistIndex))).Except(simpleKnnUser.Ratings.Select(rating => rating.ArtistIndex)).ToList();

            var recommendations = new List<Recommendation>();
            for (var i = 0; i < artistIndices.Count; i++)
            {
                var artistIndex = artistIndices[i];
                var rating = 0.0f;
                var count = 0;
                foreach (var neighbour in neighbours.Where(neighbour => neighbour.User.Ratings.Select(r => r.ArtistIndex).Contains(artistIndex)))
                {
                    rating += neighbour.User.Ratings.Where(r => r.ArtistIndex == artistIndex).First().Value;
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