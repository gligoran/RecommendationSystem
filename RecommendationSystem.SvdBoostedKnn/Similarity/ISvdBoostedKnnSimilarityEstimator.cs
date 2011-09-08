using RecommendationSystem.Knn.Foundation.Similarity;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Similarity
{
    public interface ISvdBoostedKnnSimilarityEstimator<in TSvdBoostedKnnUser> : ISimilarityEstimator<TSvdBoostedKnnUser>
        where TSvdBoostedKnnUser : ISvdBoostedKnnUser
    {}
}