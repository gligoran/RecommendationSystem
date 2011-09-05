using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Prediction;
using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Similarity;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Recommendations
{
    public class SimpleKnnRecommender : ISimpleKnnRecommender
    {
        public ISimilarityEstimator<ISimpleKnnUser> SimilarityEstimator { get; set; }
        public IRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser> RecommendationGenerator { get; set; }

        public IPredictor<ISimpleKnnModel> Predictor
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int NearestNeighboursCount { get; set; }

        #region Consturctor
        public SimpleKnnRecommender(int nearestNeighboursCount = 3)
            : this(new CosineSimilarityEstimator(), new RatingAggregationRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser>(), nearestNeighboursCount)
        {}

        public SimpleKnnRecommender(ISimilarityEstimator<ISimpleKnnUser> similarityEstimator, int nearestNeighboursCount = 3)
            : this(similarityEstimator, new RatingAggregationRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser>(), nearestNeighboursCount)
        {}

        public SimpleKnnRecommender(IRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser> simpleRecommendationGenerator, int nearestNeighboursCount = 3)
            : this(new CosineSimilarityEstimator(), simpleRecommendationGenerator, nearestNeighboursCount)
        {}

        public SimpleKnnRecommender(ISimilarityEstimator<ISimpleKnnUser> similarityEstimator, IRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser> simpleRecommendationGenerator, int nearestNeighboursCount = 3)
        {
            SimilarityEstimator = similarityEstimator;
            RecommendationGenerator = simpleRecommendationGenerator;
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
        public List<SimilarUser<ISimpleKnnUser>> CalculateKNearestNeighbours(ISimpleKnnUser user, IEnumerable<ISimpleKnnUser> users, int nearestNeighboursCount)
        {
            var neighbours = new List<SimilarUser<ISimpleKnnUser>>();
            foreach (var neighbour in users)
            {
                if (neighbour == user)
                    continue;

                var s = CalculateSimilarity(user, neighbour);
                if (s <= 0.0)
                    continue;

                neighbours.Add(new SimilarUser<ISimpleKnnUser>(neighbour, s));

                neighbours.Sort();
                while (neighbours.Count > nearestNeighboursCount)
                    neighbours.RemoveAt(neighbours.Count - 1);
            }

            return neighbours;
        }
        #endregion

        #region CalculateSimilarity
        public virtual float CalculateSimilarity(ISimpleKnnUser user, ISimpleKnnUser neighbour)
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