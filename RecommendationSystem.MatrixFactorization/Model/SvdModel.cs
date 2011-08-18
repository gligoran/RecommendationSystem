using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization.Model
{
    public class SvdModel : ISvdModel
    {
        public float[,] UserFeatures { get; set; }
        public float[,] ArtistFeatures { get; set; }
        public TrainingParameters TrainingParameters { get; set; }

        public SvdModel(float[,] userFeatures, float[,] artistFeatures, TrainingParameters trainingParameters)
        {
            UserFeatures = userFeatures;
            ArtistFeatures = artistFeatures;
            TrainingParameters = trainingParameters;
        }
    }
}