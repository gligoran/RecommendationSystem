using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Similarity
{
    public interface ISimpleSimilarityEstimator : ISimilarityEstimator<ISimpleKnnUser>
    {}
}