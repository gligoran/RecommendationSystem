using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization.Bias.Models
{
    public class BiasSvdModel : IBiasSvdModel
    {
        public float[,] UserFeatures { get; set; }
        public float[,] ArtistFeatures { get; set; }
        public float GlobalAverage { get; set; }
        public float[] UserBias { get; set; }
        public float[] ArtistBias { get; set; }
        public TrainingParameters TrainingParameters { get; set; }

        public BiasSvdModel(TrainingParameters trainingParameters)
        {
            TrainingParameters = trainingParameters;
        }

        public BiasSvdModel(float[,] userFeatures, float[,] artistFeatures, float globalAverage, float[] userBias, float[] artistBias, TrainingParameters trainingParameters)
            : this(trainingParameters)
        {
            UserFeatures = userFeatures;
            ArtistFeatures = artistFeatures;
            GlobalAverage = globalAverage;
            UserBias = userBias;
            ArtistBias = artistBias;
        }
    }
}