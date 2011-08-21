using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Recommendations
{
    public interface IRatingAggregator
    {
        float Aggregate(User user, string artist);
    }
}