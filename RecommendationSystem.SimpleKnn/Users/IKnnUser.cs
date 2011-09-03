using RecommendationSystem.Entities;

namespace RecommendationSystem.SimpleKnn.Users
{
    public interface IKnnUser : IUser
    {
        float AverageRating { get; set; }
    }
}