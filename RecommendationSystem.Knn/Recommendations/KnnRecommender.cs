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
    public class KnnRecommender : IRecommender<IKnnModel>
    {
        public ISimilarityEstimator SimilarityEstimator { get; set; }
        public IRatingAggregator RatingAggregator { get; set; }
        public int NearestNeighboursCount { get; set; }
        public float DefaultRating { get; set; }

        #region Consturctor
        public KnnRecommender(int nearestNeighboursCount = 3, float defaultRating = 0.0f)
            : this(new CosineSimilarityEstimator(), new SimpleAverageRatingAggregator(), nearestNeighboursCount, defaultRating)
        { }

        public KnnRecommender(ISimilarityEstimator similarityEstimator, int nearestNeighboursCount = 3, float defaultRating = 0.0f)
            : this(similarityEstimator, new SimpleAverageRatingAggregator(), nearestNeighboursCount, defaultRating)
        { }

        public KnnRecommender(IRatingAggregator ratingAggregator, int nearestNeighboursCount = 3, float defaultRating = 0.0f)
            : this(new CosineSimilarityEstimator(), ratingAggregator, nearestNeighboursCount, defaultRating)
        { }

        public KnnRecommender(ISimilarityEstimator similarityEstimator, IRatingAggregator ratingAggregator, int nearestNeighboursCount = 3, float defaultRating = 0.0f)
        {
            SimilarityEstimator = similarityEstimator;
            RatingAggregator = ratingAggregator;
            NearestNeighboursCount = nearestNeighboursCount;
            DefaultRating = defaultRating;
        }
        #endregion

        #region PredictRatingForArtist
        public float PredictRatingForArtist(IUser user, IKnnModel model, List<IArtist> artists, int artistIndex)
        {
            var recommendations = GenerateRecommendations(user, model, artists);
            if (recommendations != null)
                return recommendations.Where(recommendation => recommendation.Artist == artists[artistIndex]).Select(rating => rating.Value).FirstOrDefault();

            return DefaultRating;
        }
        #endregion

        #region GenerateRecommendations
        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IKnnModel model, List<IArtist> artists)
        {
            return GenerateRecommendations(user, model, artists, NearestNeighboursCount);
        }

        public List<IRecommendation> GenerateRecommendations(IUser user, IKnnModel model, List<IArtist> artists, int nearestNeighboursCount)
        {
            var knnUser = KnnUser.FromIUser(user);

            var neighbours = CalculateKNearestNeighbours(knnUser, model.Users, nearestNeighboursCount);
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

        #region CalculateKNearestNeighbours
        private List<SimilarUser> CalculateKNearestNeighbours(IKnnUser user, IEnumerable<IKnnUser> users, int nearestNeighboursCount)
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
        #endregion

        #region CalculateSimilarity
        protected virtual float CalculateSimilarity(IKnnUser user, IKnnUser neighbour)
        {
            var s = SimilarityEstimator.GetSimilarity(user, neighbour);
            return s;
        }
        #endregion

        public override string ToString()
        {
            return "NoContent";
        }
        #endregion
    }
}