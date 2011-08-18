using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization.Model
{
    public class BiasSvdModel : SvdModel, IBiasSvdModel
    {
        public float GlobalAverage { get; set; }
        public float[] UserBias { get; set; }
        public float[] ArtistBias { get; set; }

        public BiasSvdModel(float[,] userFeatures, float[,] artistFeatures, float globalAverage, float[] userBias, float[] artistBias, TrainingParameters trainingParameters)
            : base(userFeatures, artistFeatures, trainingParameters)
        {
            GlobalAverage = globalAverage;
            UserBias = userBias;
            ArtistBias = artistBias;
        }
    }
}