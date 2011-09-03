namespace RecommendationSystem.Svd.Foundation.Basic.Models
{
    public class BasicSvdModel : IBasicSvdModel
    {
        public float[,] UserFeatures { get; set; }
        public float[,] ArtistFeatures { get; set; }
        public float[] BiasBins { get; set; }

        public int FeatureCount
        {
            get { return UserFeatures.GetUpperBound(0) + 1; }
        }

        public BasicSvdModel()
        {}

        public BasicSvdModel(float[,] userFeatures, float[,] artistFeatures, float[] biasBins)
        {
            UserFeatures = userFeatures;
            ArtistFeatures = artistFeatures;
            BiasBins = biasBins;
        }
    }
}