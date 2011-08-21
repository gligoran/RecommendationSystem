namespace RecommendationSystem.MatrixFactorization.Models
{
    public interface IBiasSvdModel : ISvdModel
    {
        float GlobalAverage { get; set; }
        float[] UserBias { get; set; }
        float[] ArtistBias { get; set; }
    }
}