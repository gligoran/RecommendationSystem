using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization.Model
{
    public interface ISvdModel
    {
        float[,] UserFeatures { get; set; }
        float[,] ArtistFeatures { get; set; }
        TrainingParameters TrainingParameters { get; set; }
    }
}