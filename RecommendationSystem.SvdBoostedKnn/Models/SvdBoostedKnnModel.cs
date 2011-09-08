using System.Collections.Generic;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Models
{
    public class SvdBoostedKnnModel : ISvdBoostedKnnModel
    {
        public float[,] UserFeatures { get; set; }
        public float[,] ArtistFeatures { get; set; }

        public int FeatureCount
        {
            get { return UserFeatures.GetUpperBound(0) + 1; }
        }

        public List<ISvdBoostedKnnUser> Users { get; set; }

        public SvdBoostedKnnModel()
        {
            Users = new List<ISvdBoostedKnnUser>();
        }

        public SvdBoostedKnnModel(float[,] userFeatures, float[,] artistFeatures, List<ISvdBoostedKnnUser> users)
        {
            UserFeatures = userFeatures;
            ArtistFeatures = artistFeatures;
            Users = users;
        }
    }
}