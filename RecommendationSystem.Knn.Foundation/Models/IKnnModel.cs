using System.Collections.Generic;
using RecommendationSystem.Knn.Foundation.Users;
using RecommendationSystem.Models;

namespace RecommendationSystem.Knn.Foundation.Models
{
    public interface IKnnModel<TKnnUser> : IModel
        where TKnnUser : IKnnUser
    {
        List<TKnnUser> Users { get; set; }
    }
}