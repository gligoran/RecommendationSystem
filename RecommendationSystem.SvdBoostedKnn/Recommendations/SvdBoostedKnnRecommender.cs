using System;
using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Recommendations;
using RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Prediction;
using RecommendationSystem.Recommendations;
using RecommendationSystem.Svd.Foundation.Prediction;
using RecommendationSystem.SvdBoostedKnn.Models;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Recommendations
{
    public class SvdBoostedKnnRecommender<TSvdBoostedKnnModel> : ISvdBoostedKnnRecommender<TSvdBoostedKnnModel>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        #region Properties
        public IPredictor<TSvdBoostedKnnModel> Predictor { get; set; }
        public INewUserFeatureGenerator<TSvdBoostedKnnModel> NewUserFeatureGenerator { get; set; }
        public ISimilarityEstimator<ISvdBoostedKnnUser> SimilarityEstimator { get; set; }
        public IRecommendationGenerator<TSvdBoostedKnnModel, ISvdBoostedKnnUser> RecommendationGenerator { get; set; }
        public int NearestNeighboursCount { get; set; }
        #endregion

        #region Constructor
        public SvdBoostedKnnRecommender(ISimilarityEstimator<ISvdBoostedKnnUser> similarityEstimator, IRecommendationGenerator<TSvdBoostedKnnModel, ISvdBoostedKnnUser> recommendationGenerator, INewUserFeatureGenerator<TSvdBoostedKnnModel> newUserFeatureGenerator, int nearestNeighboursCount = 3)
        {
            SimilarityEstimator = similarityEstimator;
            RecommendationGenerator = recommendationGenerator;
            NewUserFeatureGenerator = newUserFeatureGenerator;
            NearestNeighboursCount = nearestNeighboursCount;
        }
        #endregion

        #region PredictRatingForArtist
        public float PredictRatingForArtist(IUser user, TSvdBoostedKnnModel model, List<IArtist> artists, int artistIndex)
        {
            return PredictRatingForArtist(user, model, artists, artistIndex, NearestNeighboursCount);
        }

        public float PredictRatingForArtist(IUser user, TSvdBoostedKnnModel model, List<IArtist> artists, int artistIndex, int nearestNeighboursCount)
        {
            var svdBoostedKnnUser = SvdBoostedKnnUser.FromIUser(user, NewUserFeatureGenerator.GetNewUserFeatures(model, user));
            var neighbours = CalculateKNearestNeighbours(svdBoostedKnnUser, model.Users, nearestNeighboursCount);
            if (neighbours.Count == 0)
                return 1.0f;

            return RecommendationGenerator.PredictRatingForArtist(svdBoostedKnnUser, neighbours, model, artists, artistIndex);
        }
        #endregion

        #region GenerateRecommendations
        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, TSvdBoostedKnnModel model, List<IArtist> artists)
        {
            return GenerateRecommendations(user, model, artists, NearestNeighboursCount);
        }

        public IEnumerable<IRecommendation> GenerateRecommendations(IUser user, TSvdBoostedKnnModel model, List<IArtist> artists, int nearestNeighboursCount)
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
        public virtual float CalculateSimilarity(ISvdBoostedKnnUser user, ISvdBoostedKnnUser neighbour)
        {
            var s = SimilarityEstimator.GetSimilarity(user, neighbour);
            return s;
        }
        #endregion

        #region LoadModel
        public void LoadModel(TSvdBoostedKnnModel model, string filename)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return "NoCo";
        }
        #endregion
    }

    public class ContentSvdBoostedKnnRecommender<TSvdBoostedKnnModel> : SvdBoostedKnnRecommender<TSvdBoostedKnnModel>, IContentKnnRecommender<TSvdBoostedKnnModel>
        where TSvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        #region Properties
        public IContentSimilarityEstimator ContentSimilarityEstimator { get; set; }
        public float RatingSimilarityWeight { get; set; }
        public float ContentSimilarityWeight { get; set; }
        #endregion

        #region Constructor
        public ContentSvdBoostedKnnRecommender(ISimilarityEstimator<ISvdBoostedKnnUser> similarityEstimator, IRecommendationGenerator<TSvdBoostedKnnModel, ISvdBoostedKnnUser> recommendationGenerator, INewUserFeatureGenerator<TSvdBoostedKnnModel> newUserFeatureGenerator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(similarityEstimator, recommendationGenerator, newUserFeatureGenerator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight;
            ContentSimilarityWeight = contentSimilarityWeight;
        }
        #endregion

        #region CalculateSimilarity
        public override float CalculateSimilarity(ISvdBoostedKnnUser user, ISvdBoostedKnnUser neighbour)
        {
            return base.CalculateSimilarity(user, neighbour) * ContentSimilarityEstimator.GetSimilarity(user, neighbour);
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return "Content";
        }
        #endregion
    }
}