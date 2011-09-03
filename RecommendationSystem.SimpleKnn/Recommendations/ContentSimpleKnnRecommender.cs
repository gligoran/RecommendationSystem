using RecommendationSystem.Knn.Foundation.Recommendations.RecommendationGeneration;
using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.SimpleKnn.Models;
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

        public ContentSimpleKnnRecommender(IRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser> recommendationGenerator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(recommendationGenerator, new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentSimpleKnnRecommender(IRecommendationGenerator<ISimpleKnnModel, ISimpleKnnUser> recommendationGenerator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(recommendationGenerator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentSimpleKnnRecommender(ISimilarityEstimator<ISimpleKnnUser> similarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(similarityEstimator, new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentSimpleKnnRecommender(ISimilarityEstimator<ISimpleKnnUser> similarityEstimator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(similarityEstimator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentSimpleKnnRecommender(ISimilarityEstimator<ISimpleKnnUser> similarityEstimator, IRecommendationGenerator<ISimpleKnnModel,ISimpleKnnUser> recommendationGenerator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : this(similarityEstimator, recommendationGenerator, new ContentSimilarityEstimator(), nearestNeighboursCount, ratingSimilarityWeight, contentSimilarityWeight)
        {}

        public ContentSimpleKnnRecommender(ISimilarityEstimator<ISimpleKnnUser> similarityEstimator, IRecommendationGenerator<ISimpleKnnModel,ISimpleKnnUser> recommendationGenerator, IContentSimilarityEstimator contentSimilarityEstimator, int nearestNeighboursCount = 3, float ratingSimilarityWeight = 0.5f, float contentSimilarityWeight = 0.5f)
            : base(similarityEstimator, recommendationGenerator, nearestNeighboursCount)
        {
            ContentSimilarityEstimator = contentSimilarityEstimator;
            RatingSimilarityWeight = ratingSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
            ContentSimilarityWeight = contentSimilarityWeight / (ratingSimilarityWeight + contentSimilarityWeight);
        }

        public ContentSimpleKnnRecommender(ISimilarityEstimator<ISimpleKnnUser> similarityEstimator, IRecommendationGenerator<ISimpleKnnModel,ISimpleKnnUser> recommendationGenerator, int nearestNeighboursCount = 3)
            : this(similarityEstimator, recommendationGenerator, new ContentSimilarityEstimator(), nearestNeighboursCount)
        {}
        #endregion

        public override float CalculateSimilarity(ISimpleKnnUser user, ISimpleKnnUser neighbour)
        {
            return base.CalculateSimilarity(user, neighbour) * ContentSimilarityEstimator.GetSimilarity(user, neighbour);
        }

        public override string ToString()
        {
            return "Content";
        }
    }
}