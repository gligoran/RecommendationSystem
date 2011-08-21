using System.Collections.Generic;
using RecommendationSystem.Knn.Users;

namespace RecommendationSystem.Knn.Models
{
    public class KnnModel : IKnnModel
    {
        public List<IKnnUser> Users { get; set; }

        public KnnModel()
        {
            Users = new List<IKnnUser>();
        }

        public KnnModel(List<IKnnUser> users)
        {
            Users = users;
        }
    }
}