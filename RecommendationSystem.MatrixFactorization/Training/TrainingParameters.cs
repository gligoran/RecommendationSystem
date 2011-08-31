namespace RecommendationSystem.MatrixFactorization.Training
{
    public class TrainingParameters
    {
        public float MinEpochTreshold { get; set; }
        public float MaxEpochTreshold { get; set; }
        public double RmseImprovementTreshold { get; set; }
        public float K { get; set; }
        public float LRate { get; set; }
        public int FeatureCount { get; set; }

        public TrainingParameters(int featureCount = 10, float lRate = 0.001f, float k = 0.02f, float rmseImprovementTreshold = 0.000001f, int minEpochTreshold = 120, int maxEpochTreshold = 200)
        {
            FeatureCount = featureCount;
            LRate = lRate;
            K = k;
            RmseImprovementTreshold = rmseImprovementTreshold;
            MinEpochTreshold = minEpochTreshold;
            MaxEpochTreshold = maxEpochTreshold;
        }
    }
}