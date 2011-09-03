using RecommendationSystem.Entities;

namespace RecommendationSystem.Knn.Foundation.Users
{
    public interface IKnnUser : IUser
    {
        float AverageRating { get; set; }
    }
}