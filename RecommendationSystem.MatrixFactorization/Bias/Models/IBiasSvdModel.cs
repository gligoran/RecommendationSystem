using RecommendationSystem.MatrixFactorization.Models;

namespace RecommendationSystem.MatrixFactorization.Bias.Models
{
    public interface IBiasSvdModel : ISvdModel
    {
        float GlobalAverage { get; set; }
        float[] UserBias { get; set; }
        float[] ArtistBias { get; set; }
    }
}