namespace RecommendationSystem.MatrixFactorization.Training
{
    public class TrainingParameters
    {
        public float EpochLimit { get; set; }
        public float RmseDiffLimit { get; set; }
        public float K { get; set; }
        public float LRate { get; set; }
        public int FeatureCount { get; set; }

        public TrainingParameters(int featureCount = 100, float lRate = 0.001f, float k = 0.02f, float rmseDiffLimit = 0.00001f, float epochLimit = 100)
        {
            FeatureCount = featureCount;
            LRate = lRate;
            K = k;
            RmseDiffLimit = rmseDiffLimit;
            EpochLimit = epochLimit;
        }

        public static TrainingParameters DefaultTrainingParameters = new TrainingParameters(25, 0.001f, 0.02f, 0.00001f, 70);
    }
}
