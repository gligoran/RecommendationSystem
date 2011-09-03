using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.Knn.Foundation.Users;
using RecommendationSystem.Models;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration
{
    public interface IRecommendationGenerator<in TModel, TKnnUser>
        where TModel : IModel
        where TKnnUser : IKnnUser
    {
        float PredictRatingForArtist(TKnnUser simpleKnnUser, List<SimilarUser<TKnnUser>> neighbours, TModel model, List<IArtist> artists, int artistIndex);
        IEnumerable<IRecommendation> GenerateRecommendations(TKnnUser simpleKnnUser, List<SimilarUser<TKnnUser>> neighbours, TModel model, List<IArtist> artists);
    }
}