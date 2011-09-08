namespace RecommendationSystem.Svd.Foundation.Models
{
    public class SvdBiasBinsModel : SvdModel, ISvdBiasBinsModel
    {
        public float[] BiasBins { get; set; }

        public SvdBiasBinsModel()
        {}

        public SvdBiasBinsModel(float[,] userFeatures, float[,] artistFeatures, float[] biasBins)
            : base(userFeatures, artistFeatures)
        {
            BiasBins = biasBins;
        }
    }
}