using RecommendationSystem.Models;

namespace RecommendationSystem.MatrixFactorization.Models
{
    public interface ISvdModel : IModel
    {
        float[,] UserFeatures { get; set; }
        float[,] ArtistFeatures { get; set; }
        float[] BiasBins { get; set; }
        int FeatureCount { get; }
    }
}