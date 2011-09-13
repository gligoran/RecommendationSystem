using System.Collections.Generic;
using RecommendationSystem.Svd.Foundation.Models;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Models
{
    public class SvdBoostedKnnModel : SvdModel, ISvdBoostedKnnModel
    {
        public List<ISvdBoostedKnnUser> Users { get; set; }

        public SvdBoostedKnnModel()
        {
            Users = new List<ISvdBoostedKnnUser>();
        }

        public SvdBoostedKnnModel(float[,] userFeatures, float[,] artistFeatures, List<ISvdBoostedKnnUser> users)
            : base(userFeatures, artistFeatures)
        {
            Users = users;
        }
    }
}