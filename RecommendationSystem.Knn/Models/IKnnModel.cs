using System.Collections.Generic;
using RecommendationSystem.Knn.Users;
using RecommendationSystem.Models;

namespace RecommendationSystem.Knn.Models
{
    public interface IKnnModel : IModel
    {
        List<IKnnUser> Users { get; set; }
    }
}