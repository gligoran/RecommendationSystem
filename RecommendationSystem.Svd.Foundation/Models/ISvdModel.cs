using RecommendationSystem.Models;

namespace RecommendationSystem.Svd.Foundation.Models
{
    public interface ISvdModel : IModel
    {
        float[,] UserFeatures { get; set; }
        float[,] ArtistFeatures { get; set; }
        int FeatureCount { get; }
    }
}