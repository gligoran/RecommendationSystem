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

        #region Consturctor
        public ContentKnnRecommender(int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentKnnRecommender(IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentKnnRecommender(IRatingAggregator ratingAggregator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(ratingAggregator, new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentKnnRecommender(IRatingAggregator ratingAggregator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(ratingAggregator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentKnnRecommender(ISimilarityEstimator similarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(similarityEstimator, new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentKnnRecommender(ISimilarityEstimator similarityEstimator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(similarityEstimator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentKnnRecommender(ISimilarityEstimator similarityEstimator, IRatingAggregator ratingAggregator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(similarityEstimator, ratingAggregator, new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentKnnRecommender(ISimilarityEstimator similarityEstimator, IRatingAggregator ratingAggregator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(similarityEstimator, ratingAggregator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentKnnRecommender(ISimilarityEstimator similarityEstimator, IRatingAggregator ratingAggregator, int nearestNeighboursCount = 3)
            : this(similarityEstimator, ratingAggregator, new ContentSimilarityEstimator(), nearestNeighboursCount)
        {}
        #endregion

        protected override float CalculateSimilarity(IKnnUser user, IKnnUser neighbour)
        {
            return base.CalculateSimilarity(user, neighbour) * ContentSimilarityEstimator.GetSimilarity(user, neighbour);
        }

        public override string ToString()
        {
            return "Content";
        }
    }
}