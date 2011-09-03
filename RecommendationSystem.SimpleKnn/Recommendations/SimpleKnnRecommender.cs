using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Recommendations.RecommendationGeneration;
using RecommendationSystem.SimpleKnn.Similarity;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Recommendations
{
    public class SimpleKnnRecommender : IRecommender<ISimpleKnnModel>
    {
        public ISimilarityEstimator SimilarityEstimator { get; set; }
        public IRecommendationGenerator RecommendationGenerator { get; set; }
        public int NearestNeighboursCount { get; set; }

        #region Consturctor
        public SimpleKnnRecommender(int nearestNeighboursCount = 3)
            : this(new CosineSimilarityEstimator(), new RatingAggregationRecommendationGenerator(), nearestNeighboursCount)
        {}

        public SimpleKnnRecommender(ISimilarityEstimator similarityEstimator, int nearestNeighboursCount = 3)
            : this(similarityEstimator, new RatingAggregationRecommendationGenerator(), nearestNeighboursCount)
        {}

        public SimpleKnnRecommender(IRecommendationGenerator recommendationGenerator, int nearestNeighboursCount = 3)
            : this(new CosineSimilarityEstimator(), recommendationGenerator, nearestNeighboursCount)
        {}

        public SimpleKnnRecommender(ISimilarityEstimator similarityEstimator, IRecommendationGenerator recommendationGenerator, int nearestNeighboursCount = 3)
        {
            SimilarityEstimator = similarityEstimator;
            RecommendationGenerator = recommendationGenerator;
            NearestNeighboursCount = nearestNeighboursCount;
        }
        #endregion

        #region PredictRatingForArtist
        public float PredictRatingForArtist(IUser user, ISimpleKnnModel model, List<IArtist> artists, int artistIndex)
        {
            return PredictRatingForArtist(user, model, artists, artistIndex, NearestNeighboursCount);
        }

        public float PredictRatingForArtist(IUser user, ISimpleKnnModel model, List<IArtist> artists, int artistIndex, int nearestNeighboursCount)
        {
            var knnUser = SimpleKnnUser.FromIUser(user);
            var neighbours = CalculateKNearestNeighbours(knnUser, model.Users, nearestNeighboursCount);
            if (neighbours.Count == 0)
                return 1.0f;

            return RecommendationGenerator.PredictRatingForArtist(knnUser, neighbours, model, artists, artistIndex);
        }
        #endregion

        #region GenerateRecommendations
        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, ISimpleKnnModel model, List<IArtist> artists)
        {
            return GenerateRecommendations(user, model, artists, NearestNeighboursCount);
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, ISimpleKnnModel model, List<IArtist> artists, int nearestNeighboursCount)
        {
            var knnUser = SimpleKnnUser.FromIUser(user);

            var neighbours = CalculateKNearestNeighbours(knnUser, model.Users, nearestNeighboursCount);
            if (neighbours.Count == 0)
                return null;

            return RecommendationGenerator.GenerateRecommendations(knnUser, neighbours, model, artists);
        }

        #region CalculateKNearestNeighbours
        private List<SimilarUser> CalculateKNearestNeighbours(ISimpleKnnUser user, IEnumerable<ISimpleKnnUser> users, int nearestNeighboursCount)
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
        protected virtual float CalculateSimilarity(ISimpleKnnUser user, ISimpleKnnUser neighbour)
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