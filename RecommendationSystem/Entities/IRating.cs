namespace RecommendationSystem.Entities
{
    public interface IRating
    {
        int UserIndex { get; set; }
        int ArtistIndex { get; set; }
        float Value { get; set; }
        IRating Clone();
    }
}