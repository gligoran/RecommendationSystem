using System.Collections.Generic;
using System.Linq;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.RatingAggregation;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Recommendations
{
    public class KnnRecommender : IKnnRecommender
    {
        public ISimilarityEstimator SimilarityEstimator { get; set; }
        public IRatingAggregator RatingAggregator { get; set; }
        public int NearestNeighboursCount { get; set; }

        public KnnRecommender(ISimilarityEstimator similarityEstimator, IRatingAggregator ratingAggregator, int nearestNeighboursCount = 3)
        {
            SimilarityEstimator = similarityEstimator;
            RatingAggregator = ratingAggregator;
            NearestNeighboursCount = nearestNeighboursCount;
        }

        public List<IRecommendation> GenerateRecommendations(IUser user, IKnnModel model, List<IArtist> artists)
        {
            return GenerateRecommendations(user, model, artists, NearestNeighboursCount);
        }

        public List<IRecommendation> GenerateRecommendations(IUser user, IKnnModel model, List<IArtist> artists, int nearestNeighboursCount)
        {
            var knnUser = KnnUser.FromIUser(user);

            var neighbours = CalculateKNearestNeighbours(user, model.Users, nearestNeighboursCount);
            if (neighbours.Count == 0)
                return null;

            var artistIndices = new List<int>();
            artistIndices = neighbours.Aggregate((IEnumerable<int>)artistIndices, (current, neighbour) => current.Union(neighbour.User.Ratings.Select(rating => rating.ArtistIndex))).Except(knnUser.Ratings.Select(rating => rating.ArtistIndex)).ToList();

            var recommendations = (from artist in artistIndices
                                   let r = RatingAggregator.Aggregate(knnUser, neighbours, artist)
                                   where r > 0.0f
                                   select new Recommendation(artists[artist], r)).Cast<IRecommendation>().ToList();

            recommendations.Sort();
            return recommendations;
        }

        private List<SimilarUser> CalculateKNearestNeighbours(IUser user, IEnumerable<IKnnUser> users, int nearestNeighboursCount)
        {
            var neighbours = new List<SimilarUser>();
            foreach (var neighbour in users)
            {
                if (neighbour == user)
                    continue;

                var s = CalculateSimilarity(user, neighbour);
                if (s <= 0.0)
                    continue;

                neighbours.Add(new SimilarUser(neighbour, s));

                neighbours.Sort();
                while (neighbours.Count > nearestNeighboursCount)
                    neighbours.RemoveAt(neighbours.Count - 1);
            }

            return neighbours;
        }

        protected virtual float CalculateSimilarity(IUser user, IKnnUser neighbour)
        {
            var s = SimilarityEstimator.GetSimilarity(user, neighbour);
            return s;
        }
    }
}