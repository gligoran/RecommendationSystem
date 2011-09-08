using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Prediction;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Recommendations
{
    public class SvdBoostedKnnRecommender : ISvdBoostedKnnRecommender<ISvdBoostedKnnModel, ISvdBoostedKnnUser>
    {
        #region Properties
        public IPredictor<ISvdBoostedKnnModel> Predictor { get; set; }
        public INewUserFeatureGenerator<ISvdBoostedKnnModel> NewUserFeatureGenerator { get; set; }
        public ISimilarityEstimator<ISvdBoostedKnnUser> SimilarityEstimator { get; set; }
        public IRecommendationGenerator<ISvdBoostedKnnModel, ISvdBoostedKnnUser> RecommendationGenerator { get; set; }
        public int NearestNeighboursCount { get; set; }
        #endregion

        public SvdBoostedKnnRecommender(ISimilarityEstimator<ISvdBoostedKnnUser> similarityEstimator, IRecommendationGenerator<ISvdBoostedKnnModel, ISvdBoostedKnnUser> recommendationGenerator, INewUserFeatureGenerator<ISvdBoostedKnnModel> newUserFeatureGenerator, int nearestNeighboursCount = 3)
        {
            SimilarityEstimator = similarityEstimator;
            RecommendationGenerator = recommendationGenerator;
            NewUserFeatureGenerator = newUserFeatureGenerator;
            NearestNeighboursCount = nearestNeighboursCount;
        }

        #region PredictRatingForArtist
        public float PredictRatingForArtist(IUser user, ISvdBoostedKnnModel model, List<IArtist> artists, int artistIndex)
        {
            return PredictRatingForArtist(user, model, artists, artistIndex, NearestNeighboursCount);
        }

        public float PredictRatingForArtist(IUser user, ISvdBoostedKnnModel model, List<IArtist> artists, int artistIndex, int nearestNeighboursCount)
        {
            var svdBoostedKnnUser = SvdBoostedKnnUser.FromIUser(user, NewUserFeatureGenerator.GetNewUserFeatures(model, user));
            var neighbours = CalculateKNearestNeighbours(svdBoostedKnnUser, model.Users, nearestNeighboursCount);
            if (neighbours.Count == 0)
                return 1.0f;

            return RecommendationGenerator.PredictRatingForArtist(svdBoostedKnnUser, neighbours, model, artists, artistIndex);
        }
        #endregion

        #region GenerateRecommendations
        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, ISvdBoostedKnnModel model, List<IArtist> artists)
        {
            return GenerateRecommendations(user, model, artists, NearestNeighboursCount);
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, ISvdBoostedKnnModel model, List<IArtist> artists, int nearestNeighboursCount)
        {
            var svdBoostedKnnUser = SvdBoostedKnnUser.FromIUser(user, NewUserFeatureGenerator.GetNewUserFeatures(model, user));
            var neighbours = CalculateKNearestNeighbours(svdBoostedKnnUser, model.Users, nearestNeighboursCount);
            if (neighbours.Count == 0)
                return null;

            return RecommendationGenerator.GenerateRecommendations(svdBoostedKnnUser, neighbours, model, artists);
        }
        #endregion

        #region CalculateKNearestNeighbours
        public List<SimilarUser<ISvdBoostedKnnUser>> CalculateKNearestNeighbours(ISvdBoostedKnnUser user, IEnumerable<ISvdBoostedKnnUser> users, int nearestNeighboursCount)
        {
            var neighbours = new List<SimilarUser<ISvdBoostedKnnUser>>();
            foreach (var neighbour in users)
            {
                if (neighbour == user)
                    continue;

                var s = CalculateSimilarity(user, neighbour);
                if (s <= 0.0)
                    continue;

                neighbours.Add(new SimilarUser<ISvdBoostedKnnUser>(neighbour, s));

                neighbours.Sort();
                while (neighbours.Count > nearestNeighboursCount)
                    neighbours.RemoveAt(neighbours.Count - 1);
            }

            return neighbours;
        }
        #endregion

        #region CalculateSimilarity
        public float CalculateSimilarity(ISvdBoostedKnnUser user, ISvdBoostedKnnUser neighbour)
        {
            var s = SimilarityEstimator.GetSimilarity(user, neighbour);
            return s;
        }
        #endregion

        #region LoadModel
        public void LoadModel(ISvdBoostedKnnModel model, string filename)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}