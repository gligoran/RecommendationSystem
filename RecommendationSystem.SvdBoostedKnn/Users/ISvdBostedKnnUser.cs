using RecommendationSystem.Knn.Foundation.Users;

namespace RecommendationSystem.SvdBoostedKnn.Users
{
    public interface ISvdBoostedKnnUser : IKnnUser
    {
        float[] Features { get; set; }
    }
}