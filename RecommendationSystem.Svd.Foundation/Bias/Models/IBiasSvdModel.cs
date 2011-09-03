using RecommendationSystem.Svd.Foundation.Models;

namespace RecommendationSystem.Svd.Foundation.Bias.Models
{
    public interface IBiasSvdModel : ISvdModel
    {
        float GlobalAverage { get; set; }
        float[] UserBias { get; set; }
        float[] ArtistBias { get; set; }
    }
}