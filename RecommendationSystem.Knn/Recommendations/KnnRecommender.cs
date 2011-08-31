using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Recommendations
{
    public class KnnRecommender : IRecommender<IKnnModel>
    {
        public ISimilarityEstimator SimilarityEstimator { get; set; }
        public IRecommendationGenerator RecommendationGenerator { get; set; }
        public int NearestNeighboursCount { get; set; }

        #region Consturctor
        public KnnRecommender(int nearestNeighboursCount = 3)
            : this(new CosineSimilarityEstimator(), new RatingAggregationRecommendationGenerator(), nearestNeighboursCount)
        {}

        public KnnRecommender(ISimilarityEstimator similarityEstimator, int nearestNeighboursCount = 3)
            : this(similarityEstimator, new RatingAggregationRecommendationGenerator(), nearestNeighboursCount)
        {}

        public KnnRecommender(IRecommendationGenerator recommendationGenerator, int nearestNeighboursCount = 3)
            : this(new CosineSimilarityEstimator(), recommendationGenerator, nearestNeighboursCount)
        {}

        public KnnRecommender(ISimilarityEstimator similarityEstimator, IRecommendationGenerator recommendationGenerator, int nearestNeighboursCount = 3)
        {
            SimilarityEstimator = similarityEstimator;
            RecommendationGenerator = recommendationGenerator;
            NearestNeighboursCount = nearestNeighboursCount;
        }
        #endregion

        #region PredictRatingForArtist
        public float PredictRatingForArtist(IUser user, IKnnModel model, List<IArtist> artists, int artistIndex)
        {
            return PredictRatingForArtist(user, model, artists, artistIndex, NearestNeighboursCount);
        }

        public float PredictRatingForArtist(IUser user, IKnnModel model, List<IArtist> artists, int artistIndex, int nearestNeighboursCount)
        {
            var knnUser = KnnUser.FromIUser(user);
            var neighbours = CalculateKNearestNeighbours(knnUser, model.Users, nearestNeighboursCount);
            if (neighbours.Count == 0)
                return 1.0f;

            return RecommendationGenerator.PredictRatingForArtist(knnUser, neighbours, model, artists, artistIndex);
        }
        #endregion

        #region GenerateRecommendations
        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IKnnModel model, List<IArtist> artists)
        {
            return GenerateRecommendations(user, model, artists, NearestNeighboursCount);
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, IKnnModel model, List<IArtist> artists, int nearestNeighboursCount)
        {
            var knnUser = KnnUser.FromIUser(user);

            var neighbours = CalculateKNearestNeighbours(knnUser, model.Users, nearestNeighboursCount);
            if (neighbours.Count == 0)
                return null;

            return RecommendationGenerator.GenerateRecommendations(knnUser, neighbours, model, artists);
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