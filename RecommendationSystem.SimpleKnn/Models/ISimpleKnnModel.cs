using System.Collections.Generic;
using RecommendationSystem.Models;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Models
{
    public interface ISimpleKnnModel : IModel
    {
        List<ISimpleKnnUser> Users { get; set; }
    }
}