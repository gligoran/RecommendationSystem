using System.Collections.Generic;
using RecommendationSystem.SimpleKnn.Users;

namespace RecommendationSystem.SimpleKnn.Models
{
    public class SimpleKnnModel : ISimpleKnnModel
    {
        public List<ISimpleKnnUser> Users { get; set; }

        public SimpleKnnModel()
        {
            Users = new List<ISimpleKnnUser>();
        }

        public SimpleKnnModel(List<ISimpleKnnUser> users)
        {
            Users = users;
        }
    }
}