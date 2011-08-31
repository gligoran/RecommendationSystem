using System.Collections.Generic;
using RecommendationSystem.Entities;
using RecommendationSystem.Knn.Models;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;
using RecommendationSystem.Recommendations;

namespace RecommendationSystem.Knn.Recommendations
{
    public interface IRecommendationGenerator
    {
        float PredictRatingForArtist(IKnnUser knnUser, List<SimilarUser> neighbours, IKnnModel model, List<IArtist> artists, int artistIndex);
        IEnumerable<IRecommendation> GenerateRecommendations(IKnnUser user, List<SimilarUser> neighbours, IKnnModel model, List<IArtist> artists);
    }
}