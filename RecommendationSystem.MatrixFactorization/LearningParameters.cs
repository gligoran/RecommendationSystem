namespace RecommendationSystem.MatrixFactorization
{
    public static class LearningParameters
    {
        public const int FeatureCount = 25;
        public const float LRate = 0.001f;
        public const float K = 0.02f;
        //public const float RmseTarget = 1.35f;
        public const float RmseDiffLimit = 0.00001f;
        public const float EpochLimit = 70;
    }
}
