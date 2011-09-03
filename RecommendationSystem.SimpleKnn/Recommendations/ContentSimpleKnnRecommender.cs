using RecommendationSystem.SimpleKnn.Recommendations.RecommendationGeneration;
using RecommendationSystem.SimpleKnn.Similarity;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Recommendations
{
    public class ContentSimpleKnnRecommender : SimpleKnnRecommender, IContentSimpleKnnRecommender
    {
        public IContentSimilarityEstimator ContentSimilarityEstimator { get; set; }
        public float RatingSimilarityWeight { get; set; }
        public float ContentSimilarityWeight { get; set; }

        #region Consturctor
        public ContentSimpleKnnRecommender(int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentSimpleKnnRecommender(IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentSimpleKnnRecommender(IRecommendationGenerator recommendationGenerator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(recommendationGenerator, new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentSimpleKnnRecommender(IRecommendationGenerator recommendationGenerator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(recommendationGenerator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentSimpleKnnRecommender(ISimilarityEstimator similarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(similarityEstimator, new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentSimpleKnnRecommender(ISimilarityEstimator similarityEstimator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(similarityEstimator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentSimpleKnnRecommender(ISimilarityEstimator similarityEstimator, IRecommendationGenerator recommendationGenerator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(similarityEstimator, recommendationGenerator, new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentSimpleKnnRecommender(ISimilarityEstimator similarityEstimator, IRecommendationGenerator recommendationGenerator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(similarityEstimator, recommendationGenerator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentSimpleKnnRecommender(ISimilarityEstimator similarityEstimator, IRecommendationGenerator recommendationGenerator, int nearestNeighboursCount = 3)
            : this(similarityEstimator, recommendationGenerator, new ContentSimilarityEstimator(), nearestNeighboursCount)
        {}
        #endregion

        protected override float CalculateSimilarity(ISimpleKnnUser user, ISimpleKnnUser neighbour)
        {
            return base.CalculateSimilarity(user, neighbour) * ContentSimilarityEstimator.GetSimilarity(user, neighbour);
        }

        public override string ToString()
        {
            return "Content";
        }
    }
}