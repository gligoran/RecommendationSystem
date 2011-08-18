namespace RecommendationSystem.Knn.Recommendations
{
    public interface IRatingAggregator
    {
        float Aggregate(User.User user, string artist);
    }
}
