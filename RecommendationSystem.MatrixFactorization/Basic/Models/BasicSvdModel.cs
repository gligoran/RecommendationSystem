using RecommendationSystem.MatrixFactorization.Models;
using RecommendationSystem.MatrixFactorization.Training;

namespace RecommendationSystem.MatrixFactorization.Basic.Models
{
    public class BasicSvdModel : IBasicSvdModel
    {
        public float[,] UserFeatures { get; set; }
        public float[,] ArtistFeatures { get; set; }
        public TrainingParameters UserTrainingParameters { get; set; }

        internal BasicSvdModel()
        {}

        public BasicSvdModel(float[,] userFeatures, float[,] artistFeatures, TrainingParameters trainingParameters)
        {
            UserFeatures = userFeatures;
            ArtistFeatures = artistFeatures;
            UserTrainingParameters = trainingParameters;
        }
    }
}