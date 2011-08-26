namespace RecommendationSystem.QualityTesting.Testers
{
    public interface ITester
    {
        string TestName { get; set; }
        void Test();
    }
}