using RecommendationSystem.Entities;
using RecommendationSystem.Knn.RatingAggregation;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Recommendations
{
    public class ContentKnnRecommender : KnnRecommender, IContentKnnRecommender
    {
        public IContentSimilarityEstimator ContentSimilarityEstimator { get; set; }

        public ContentKnnRecommender(ISimilarityEstimator similarityEstimator, IRatingAggregator ratingAggregator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3)
            : base(similarityEstimator, ratingAggregator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
        }

        protected override float CalculateSimilarity(IUser user, IKnnUser neighbour)
        {
            return base.CalculateSimilarity(user, neighbour) * ContentSimilarityEstimator.GetSimilarity(user, neighbour);
        }
    }
}