using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Recommendations;
using RecommendationSystem.SimpleKnn.Models;
using RecommendationSystem.SimpleKnn.Similarity;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Recommendations.RecommendationGeneration
{
    public interface IRecommendationGenerator
    {
        float PredictRatingForArtist(ISimpleKnnUser simpleKnnUser, List<SimilarUser> neighbours, ISimpleKnnModel model, List<IArtist> artists, int artistIndex);
        IEnumerable<IRecommendation> GenerateRecommendations(ISimpleKnnUser simpleKnnUser, List<SimilarUser> neighbours, ISimpleKnnModel model, List<IArtist> artists);
    }
}