using RecommendationSystem.Entities;
using RecommendationSystem.Knn.RatingAggregation;
using RecommendationSystem.Knn.Similarity;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Recommendations
{
    public class ContentKnnRecommender : KnnRecommender, IContentKnnRecommender
    {
        public IContentSimilarityEstimator ContentSimilarityEstimator { get; set; }
        public float RatingSimilarityWeight { get; set; }
        public float ContentSimilarityWeight { get; set; }

        public ContentKnnRecommender(ISimilarityEstimator similarityEstimator, IRatingAggregator ratingAggregator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(similarityEstimator, ratingAggregator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;

            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        protected override float CalculateSimilarity(IUser user, IKnnUser neighbour)
        {
            return base.CalculateSimilarity(user, neighbour) * ContentSimilarityEstimator.GetSimilarity(user, neighbour);
        }
    }
}