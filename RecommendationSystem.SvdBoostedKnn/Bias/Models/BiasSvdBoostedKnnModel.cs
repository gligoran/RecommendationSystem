using System.Collections.Generic;
using RecommendationSystem.Svd.Foundation.Bias.Models;
using RecommendationSystem.SvdBoostedKnn.Users;

namespace RecommendationSystem.SvdBoostedKnn.Bias.Models
{
    public class BiasSvdBoostedKnnModel : BiasSvdModel, IBiasSvdBoostedKnnModel
    {
        public List<ISvdBoostedKnnUser> Users { get; set; }

        public BiasSvdBoostedKnnModel()
        {
            Users = new List<ISvdBoostedKnnUser>();
        }

        public BiasSvdBoostedKnnModel(float[,] userFeatures, float[,] artistFeatures, float globalAverage, float[] userBias, float[] artistBias, List<ISvdBoostedKnnUser> users)
            : base(userFeatures, artistFeatures, globalAverage, userBias, artistBias)
        {
            Users = users;
        }
    }
}