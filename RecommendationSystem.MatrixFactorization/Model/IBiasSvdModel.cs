namespace RecommendationSystem.MatrixFactorization.Model
{
    public interface IBiasSvdModel : ISvdModel
    {
        float GlobalAverage { get; set; }
        float[] UserBias { get; set; }
        float[] ArtistBias { get; set; }
    }
}