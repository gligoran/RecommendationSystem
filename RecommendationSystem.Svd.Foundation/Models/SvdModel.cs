namespace RecommendationSystem.Svd.Foundation.Models
{
    public class SvdModel : ISvdModel
    {
        public float[,] UserFeatures { get; set; }
        public float[,] ArtistFeatures { get; set; }

        public int FeatureCount
        {
            get { return UserFeatures.GetUpperBound(0) + 1; }
        }

        public SvdModel()
        {}

        public SvdModel(float[,] userFeatures, float[,] artistFeatures)
        {
            UserFeatures = userFeatures;
            ArtistFeatures = artistFeatures;
        }
    }
}