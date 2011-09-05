namespace RecommendationSystem.Svd.Foundation.Bias.Models
{
    public class BiasSvdBiasBinsModel : BiasSvdModel, IBiasSvdBiasBinsModel
    {
        public float[] BiasBins { get; set; }

        public BiasSvdBiasBinsModel()
        {}

        public BiasSvdBiasBinsModel(float[,] userFeatures, float[,] artistFeatures, float globalAverage, float[] userBias, float[] artistBias, float[] biasBins)
            : base(userFeatures, artistFeatures, globalAverage, userBias, artistBias)
        {
            BiasBins = biasBins;
        }
    }
}