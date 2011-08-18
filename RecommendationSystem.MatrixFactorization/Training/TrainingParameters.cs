namespace RecommendationSystem.MatrixFactorization.Training
{
    public class TrainingParameters
    {
        public float EpochLimit { get; set; }
        public float RmseDiffLimit { get; set; }
        public float K { get; set; }
        public float LRate { get; set; }
        public int FeatureCount { get; set; }

        public TrainingParameters(int featureCount, float lRate, float k, float rmseDiffLimit, float epochLimit)
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
