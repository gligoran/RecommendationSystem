using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Knn.Foundation.Users;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Foundation.Recommendations
{
    public interface IKnnRecommender<TModel, TKnnUser> : IRecommender<TModel>
        where TModel : IModel
        where TKnnUser : IKnnUser
    {
        ISimilarityEstimator<TKnnUser> SimilarityEstimator { get; set; }
        IRecommendationGenerator<TModel, TKnnUser> RecommendationGenerator { get; set; }
        int NearestNeighboursCount { get; set; }
        float PredictRatingForArtist(IUser user, TModel model, List<IArtist> artists, int artistIndex, int nearestNeighboursCount);
        IEnumerable<IRecommendation> GenerateRecommendations(IUser user, TModel model, List<IArtist> artists, int nearestNeighboursCount);
        List<SimilarUser<TKnnUser>> CalculateKNearestNeighbours(TKnnUser user, IEnumerable<TKnnUser> users, int nearestNeighboursCount);
        float CalculateSimilarity(TKnnUser user, TKnnUser neighbour);
    }
}