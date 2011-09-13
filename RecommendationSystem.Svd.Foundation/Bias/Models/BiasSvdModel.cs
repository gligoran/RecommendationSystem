using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Bias.Models
{
    public class BiasSvdModel : SvdModel, IBiasSvdModel
    {
        public float GlobalAverage { get; set; }
        public float[] UserBias { get; set; }
        public float[] ArtistBias { get; set; }

        public BiasSvdModel()
        {}

        public BiasSvdModel(float[,] userFeatures, float[,] artistFeatures, float globalAverage, float[] userBias, float[] artistBias)
            : base(userFeatures, artistFeatures)
        {
            GlobalAverage = globalAverage;
            UserBias = userBias;
            ArtistBias = artistBias;
        }
    }
}