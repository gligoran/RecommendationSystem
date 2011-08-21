using RecommendationSystem.Entities;

namespace RecommendationSystem.Knn.Users
{
    public interface IKnnUser : IUser
    {
        float AverageRating { get; set; }
    }
}