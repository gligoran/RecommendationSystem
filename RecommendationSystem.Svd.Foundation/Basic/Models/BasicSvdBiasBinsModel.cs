namespace RecommendationSystem.Svd.Foundation.Basic.Models
{
    public class BasicSvdBiasBinsModel : BasicSvdModel, IBasicSvdBiasBinsModel
    {
        public float[] BiasBins { get; set; }

        public BasicSvdBiasBinsModel()
        {}

        public BasicSvdBiasBinsModel(float[,] userFeatures, float[,] artistFeatures, float[] biasBins)
            : base(userFeatures, artistFeatures)
        {
            BiasBins = biasBins;
        }
    }
}